using DataAccess.Models.ReactionTypeModel;
using Business.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Utils;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Business.Repository.ReactionTypeRepo;
using static Business.Constants.ResponseMsg;
using Business.Repository.PlatformRepo;
using DataAccess.Models.Pagination;

namespace Business.Service.ReactionTypeService
{
    public class ReactionTypeService : IReactionTypeService
    {
        private readonly IReactionTypeRepository _reactionTypeRepository;
        private readonly IPlatformRepository _platformRepository;

        private readonly string ClassName = typeof(Reactiontype).Name;
        private readonly string ReferClassName = typeof(Platform).Name;

        public ReactionTypeService(
            IReactionTypeRepository reactionTypeRepository,
            IPlatformRepository platformRepository)
        {
            _reactionTypeRepository = reactionTypeRepository;
            _platformRepository = platformRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var reactionType = await GetById(id);
            if (reactionType != null)
            {
                var result = await _reactionTypeRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<ReactionTypeDto> GetById(int id)
        {
            var reactiontype = await _reactionTypeRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<ReactionTypeDto>(reactiontype);
        }

        public async Task<int> Insert(InsertReactionType dto)
        {
            if (!(await ValidPlatfrom(dto.PlatformId)))
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var reactionType = MapperConfig.GetMapper().Map<Reactiontype>(dto);
                var result = await _reactionTypeRepository.Update(reactionType);
                return reactionType.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        private async Task<bool> ValidPlatfrom(int id)
        {
            var platform = await _platformRepository.Get(x => x.Id == id);
            return platform != null;
        }


        public async Task<ReactionTypeDto> SearchByName(string name)
        {
            var reactionType = await _reactionTypeRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<ReactionTypeDto>(reactionType);
        }

        public async Task<int> Update(int id, UpdateReactionTypeDto dto)
        {
            if ((await GetById(id) == null))
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }

            if (!(await ValidPlatfrom(dto.PlatformId)))
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }

            var check = await SearchByName(dto.Name);
            if (check == null || id == check.Id)
            {
                var reactionType = MapperConfig.GetMapper().Map<Reactiontype>(dto);
                reactionType.Id = id;
                var result = await _reactionTypeRepository.Update(reactionType);
                return reactionType.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        public async Task<PaginationList<ReactionTypeDto>> SearchAsync(ReactionTypePaging paging)
        {
            var result = await _reactionTypeRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<ReactionTypeDto>>(result.Items);
            return new PaginationList<ReactionTypeDto>
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
