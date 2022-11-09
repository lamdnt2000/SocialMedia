using Business.Repository.ReactionRepo;
using Business.Repository.ReactionTypeRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.ReactionModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.ReactionService
{
    public class ReactionService : BaseService, IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IReactionTypeRepository _reactionTypeRepository;

        private readonly string ClassName = typeof(Reaction).Name;
        private readonly string ReferClassName = typeof(Platform).Name;

        public ReactionService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IReactionTypeRepository reactionTypeRepository,
            IReactionRepository reactionRepository) : base(httpContextAccessor, userRepository)
        {
            _reactionTypeRepository = reactionTypeRepository;
            _reactionRepository = reactionRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var reactionType = await GetById(id);
            if (reactionType != null)
            {
                var result = await _reactionRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<ReactionDto> GetById(int id)
        {
            var reaction = await _reactionTypeRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<ReactionDto>(reaction);
        }

        public async Task<int> Insert(InsertReactionDto dto)
        {
            if (!(await ValidReactionType(dto.ReactionTypeId)))
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }
            
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        private async Task<bool> ValidReactionType(int id)
        {
            var reactiontype = await _reactionTypeRepository.Get(x => x.Id == id);
            return reactiontype != null;
        }

        public Task<bool> PagingSearch()
        {
            throw new NotImplementedException();
        }

        public Task<ReactionDto> SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(int id, UpdateReactionDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
