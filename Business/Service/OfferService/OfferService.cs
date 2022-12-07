using Business.Repository.OfferRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.OfferModel;
using DataAccess.Models.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.OfferService
{
    public class OfferService : BaseService, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly string ClassName = typeof(Offer).Name;

        public OfferService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IOfferRepository offerRepositoryy) : base(httpContextAccessor, userRepository)
        {
            _offerRepository = offerRepositoryy;
        }

        public async Task<bool> Delete(int id)
        {
            var platform = await GetById(id);
            if (platform != null)
            {
                var result = await _offerRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<OfferDto> GetById(int id)
        {
            var offer = await _offerRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<OfferDto>(offer);
        }

        public async Task<int> Insert(InsertOfferDto dto)
        {
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var offer = MapperConfig.GetMapper().Map<Offer>(dto);
                var result = await _offerRepository.Update(offer);
                return offer.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<OfferDto>> SearchAsync(OfferPaging paging)
        {
            var result = await _offerRepository.SearchPlatformAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<OfferDto>>(result.Items);
            return new PaginationList<OfferDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }

        public async Task<OfferDto> SearchByName(string name)
        {
            var offer = await _offerRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<OfferDto>(offer);
        }

        public async Task<int> Update(int id, UpdateOfferDto dto)
        {
            var check = await SearchByName(dto.Name);
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            if (check == null || id == check.Id)
            {
                var offer = MapperConfig.GetMapper().Map<Offer>(dto);
                offer.Id = id;
                var result = await _offerRepository.Update(offer);
                return offer.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }
    }
}
