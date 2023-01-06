using AutoFilterer.Extensions;
using Business.Repository.GenericRepo;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Models.FeaturePlanModel;
using DataAccess.Models.PackageModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlanModel;
using DataAccess.Models.PriceModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Repository.PackageRepo
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(SocialMediaContext context) : base(context)
        {
        }

        public async Task<PackageDto> GetPlanOfPackage(int id)
        {
            return await context.Packages.Where(x => x.Id == id)

                .Select(x => new PackageDto()
                {
                    Id = x.Id,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate.Value,
                    UpdateDate = x.UpdateDate.Value,
                    Plans = x.Plans.Select(x => new PlanDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        PackageId = x.PackageId ,
                        Status = x.Status,
                        CreatedDate= x.CreatedDate.Value,
                        UpdateDate= x.UpdateDate.Value,
                        PlanPrices = x.PlanPrices.Select(x=> new PriceDto()
                        {
                            Id= x.Id,
                            PriceType = x.PriceType,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            PlanId = x.PlanId,
                        }).ToList(),
                        FeaturePlans = x.FeaturePlans.Select(x=> new FeaturePlanDto()
                        {
                            FeatureId = x.FeatureId,
                            PlanId = x.PlanId,
                            Quota = x.Quota,
                            Valid = x.Valid,
                            FeatureType = x.Feature.Type,
                        }).ToList()
                    }).ToList(),


                })
                .FirstOrDefaultAsync();
        }

        public async Task<PaginationList<Package>> SearchPackageAsync(PakagePaging paging)
        {
            var totalItem = await context.Packages.ApplyFilterWithoutPagination(paging).CountAsync();
            var currentPage = paging.Page;
            var pageSize = paging.PerPage;
            var totalPage = Math.Ceiling((decimal)totalItem / pageSize);
            var result = context.Packages.ApplyFilter(paging).ToList();
            return new PaginationList<Package>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = (int)totalPage,
                Items = result
            };
        }
    }
}
