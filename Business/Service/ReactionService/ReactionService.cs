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

        private readonly string ClassName = typeof(Reaction).Name;

        public ReactionService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IReactionRepository reactionRepository) : base(httpContextAccessor, userRepository)
        {
        
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
            var reaction = await _reactionRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<ReactionDto>(reaction);
        }

        public async Task<int> Insert(InsertReactionDto dto)
        {
            var reaction = MapperConfig.GetMapper().Map<Reaction>(dto);
            _reactionRepository.ValidEntity(reaction);
            reaction.CreatedDate = DateTime.Now;
            reaction.Status = true;
            var result = await _reactionRepository.Insert(reaction);
            return reaction.Id;
        }

      
        public Task<bool> PagingSearch()
        {
            throw new NotImplementedException();
        }

      

        public async Task<int> Update(int id, UpdateReactionDto dto)
        {
            var check = await GetById(id);
            if (check == null)
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
            var reaction = MapperConfig.GetMapper().Map<Reaction>(dto);
            _reactionRepository.ValidEntity(reaction);
            reaction.Id = id;
            reaction.CreatedDate = check.CreatedDate;
            reaction.UpdateDate = DateTime.Now;
            var result = await _reactionRepository.Update(reaction);
            return reaction.Id;
        }
    }
}
