using Business.Repository.TransactionDepositRepo;
using Business.Repository.UserRepo;
using Business.Repository.WalletRepo;
using Business.Utils;
using CorePush.Apple;
using DataAccess.Entities;
using DataAccess.Models.ConfigModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PaymentModel;
using DataAccess.Models.TransectionDepositModel;
using DataAccess.Models.WalletModel;
using DataAccess.Repository.UserTypeRepo;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.WalletService
{
    public class WalletService : BaseService, IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionDepositRepository _transactionDepositRepository;
        private readonly PaymentConfig _paymentConfig;
        private readonly string ClassName = typeof(Wallet).Name;

        public WalletService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUserTypeRepository userTypeRepository,
            IWalletRepository walletRepository,
            ITransactionDepositRepository transactionDepositRepository,
            PaymentConfig paymentConfig) : 
            base(httpContextAccessor, userRepository, userTypeRepository)
        {
            _walletRepository = walletRepository;
            _transactionDepositRepository = transactionDepositRepository;
            _paymentConfig = paymentConfig;
        }

        public async Task<bool> Delete(int userId)
        {
            var wallet = await GetByUserId(userId);
            if (wallet != null)
            {
                var result = await _walletRepository.Delete(wallet.Id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }



        public async Task<WalletDto> GetByUserId(int userId)
        {
            var wallet = await _walletRepository.Get(x => x.UserId == userId);
            return wallet != null ? MapperConfig.GetMapper().Map<WalletDto>(wallet) : null;
        }

        public async Task<int> Insert()
        {
            int userId = this.GetCurrentUserId();
            var check = await GetByUserId(userId);
            if (check != null)
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
            Wallet wallet = new Wallet() { Currency = "VND", Status = true, UserId = userId, Balance = 0 };
            var result = await _walletRepository.Insert(wallet);
            return wallet.Id;
        }



        public async Task<int> DisableWallet(int userId)
        {
            
            var check = await GetByUserId(userId);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var wallet = MapperConfig.GetMapper().Map<Wallet>(check);
            wallet.Status = false;
            var result = await _walletRepository.Update(wallet);
            return wallet.Id;
        }

        public async Task<int> UpdateBalance(VnPayLibrary vnpay)
        {
            int userId = this.GetCurrentUserId();
            var check = await GetByUserId(userId);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (vnpay != null)
            {
                string vnp_HashSecret = _paymentConfig.HashSecret; //Chuoi bi mat


                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
                String orderInfor = vnpay.GetResponseData("vnp_OrderInfo");
                String cardType = vnpay.GetResponseData("vnp_CardType");
                String bankTransNo = vnpay.GetResponseData("vnp_BankTranNo");
                String TerminalID = vnpay.GetResponseData("vnp_TmnCode");
                int vnp_Amount = Convert.ToInt32(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = vnpay.GetResponseData("vnp_BankCode");

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                TransactionDeposit transactionDeposit = await _transactionDepositRepository.Get(x => x.WalletId == check.Id && x.TxnRef == orderId);

                if (transactionDeposit == null)
                {
                    throw new Exception("Transaction "+ NOT_FOUND);
                }
                transactionDeposit = new TransactionDeposit()
                {
                    Id = transactionDeposit.Id,
                    Amount = vnp_Amount,
                    ResponseCode = vnp_ResponseCode,
                    TransNoId = vnpayTranId,
                    PayDate = DateTime.Now,
                    OrderInfor = orderInfor,
                    TmnCode = TerminalID,
                    BankTranNo = bankTransNo,
                    CardType = cardType,
                    TxnRef = orderId,
                    WalletId = check.Id,
                    CurrentBlance = check.Balance,
                    NewBalance = check.Balance + vnp_Amount,
                    BankCode = bankCode,
                    TransactionStatus = vnp_TransactionStatus,
                    Status = false
                };
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        transactionDeposit.Status = true;
                        await _transactionDepositRepository.Update(transactionDeposit);
                        check.Balance = (int)transactionDeposit.NewBalance;
                        var wallet = MapperConfig.GetMapper().Map<Wallet>(check);
                        await _walletRepository.Update(wallet);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        await _transactionDepositRepository.Insert(transactionDeposit);
                        return 0;
                    }
                }
                else
                {
                    throw new Exception(INVALID + " signature");
                }
            }
            else
            {
                throw new Exception(INVALID + " parameter");
            }

            return 1;
        }

        public async Task<WalletDto> GetCurrentWallet()
        {
            int userId = GetCurrentUserId();

            var result = await GetByUserId(userId);
            if (result == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            return result;
        }

        public async Task<string> CreateDepositLink(PaymentDto dto)
        {
            WalletDto wallet = await GetCurrentWallet();
            if (wallet == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            string vnp_Returnurl = _paymentConfig.ReturnUrl ; //URL nhan ket qua tra ve 
            string vnp_Url = _paymentConfig.VnpUrl; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _paymentConfig.TmnCode; //Ma website
            string vnp_HashSecret = _paymentConfig.HashSecret; //Chuoi bi mat

            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = dto.Money; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.OrderDesc = wallet.UserId + " deposit " + dto.Money;
            order.CreatedDate = DateTime.Now;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "::1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "Deposit money"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            TransactionDeposit transactionDeposit = new TransactionDeposit()
            {

                WalletId = wallet.Id,
                TxnRef = order.OrderId,
                PayDate = order.CreatedDate,
                Amount = dto.Money,
                Status = false
            };
            await _transactionDepositRepository.Insert(transactionDeposit);

            return paymentUrl;
        }

        public async Task<PaginationList<TransactionDepositDto>> SearchTransaction(TransactionDepositPaging paging)
        {
            var wallet = await GetCurrentWallet();
            if (wallet == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            paging.WalletId = wallet.Id;
            var result = await _transactionDepositRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<TransactionDepositDto>>(result.Items);
            return new PaginationList<TransactionDepositDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItem = result.TotalItem,
                TotalPage = result.TotalPage
            };
        }
    }
}
