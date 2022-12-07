using Business.Repository.LocationRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.LocationModel;
using DataAccess.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;
namespace Business.Service.LocationService
{
    public class LocationService: ILocationService
    {
        private ILocationRepository _locationRepository;
        private readonly string ClassName = typeof(Location).Name;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var platform = await GetById(id);
            if (platform != null)
            {
                var result = await _locationRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<LocationDto> GetById(int id)
        {
            var platform = await _locationRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<LocationDto>(platform);
        }

        public async Task<int> Insert(InsertLocationDto dto)
        {
            var location = MapperConfig.GetMapper().Map<Location>(dto);
            var result = await _locationRepository.Update(location);
            return location.Id;
        }


        public async Task<PaginationList<LocationDto>> SearchAsync(LocationPaging paging)
        {
            var result = await _locationRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<LocationDto>>(result.Items);
            return new PaginationList<LocationDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }


        public async Task<int> Update(int id, UpdateLocationDto dto)
        {
     
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var location = MapperConfig.GetMapper().Map<Location>(dto);
            location.Id = id;
            var result = await _locationRepository.Update(location);
            return location.Id;
        }
    }
}
