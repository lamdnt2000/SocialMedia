using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.SubscriptionRepo
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(SocialMediaContext context) : base(context)
        {
        }

        public bool ValidateCSubscription(Subscription entity)
        {
            if (!context.Wallets.Any(x => x.Id == entity.WalletId))
            {
                throw new Exception("Wallet not exist!");
            }
            if (!context.Offers.Any(x => x.Id == entity.OfferId))
            {
                throw new Exception("Offer not exist!");
            }
            if (!context.Packages.Any(x => x.Id == entity.PackageId))
            {
                throw new Exception("Package not exist!");
            }
            return true;
        }
    }
}
