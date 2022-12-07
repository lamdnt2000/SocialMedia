using Business.Repository.GenericRepo;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.SubscriptionRepo
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        bool ValidateCSubscription(Subscription entity);
    }
}
