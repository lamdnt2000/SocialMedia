using Business.Repository.CategoryRepo;
using Business.Repository.PlatformRepo;
using Business.Repository.UserRepo;
using Business.Utils;
using DataAccess.Entities;
using DataAccess.Models.BranModel;
using DataAccess.Models.CategoryModel;
using DataAccess.Models.Pagination;
using DataAccess.Models.PlatFormModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Business.Constants.ResponseMsg;

namespace Business.Service.CategoryService
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlatformRepository _platformRepository;

        private readonly string ClassName = typeof(Category).Name;
        private readonly string ReferClassName = typeof(Platform).Name;


        public CategoryService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IPlatformRepository platformRepository) : base(httpContextAccessor, userRepository)
        {
            _categoryRepository = categoryRepository;
            _platformRepository = platformRepository;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await GetById(id);
            if (category != null)
            {
                var result = await _categoryRepository.Delete(id);
                return (result > 0);
            }
            else
            {
                throw new Exception(ClassName + " " + NOT_FOUND);
            }
        }

        public async Task<CategoryDto> GetById(int id)
        {

            var category = await _categoryRepository.Get(x => x.Id == id);
            return MapperConfig.GetMapper().Map<CategoryDto>(category);
        }

        public async Task<int> Insert(InsertCategoryDto dto)
        {
            if (!(await ValidPlatfrom(dto.PlatformId)))
            {
                throw new Exception(ReferClassName + " " + NOT_FOUND);
            }
            var check = await SearchByName(dto.Name);
            if (check == null)
            {
                var category = MapperConfig.GetMapper().Map<Category>(dto);
                var result = await _categoryRepository.Update(category);
                return category.Id;
            }
            else
            {
                throw new Exception(DUPLICATED + " " + ClassName);
            }
        }

        private async Task<bool> ValidPlatfrom(int id)
        {
            var platform = await _categoryRepository.Get(x => x.Id == id);
            return platform != null;
        }

        

        public async Task<CategoryDto> SearchByName(string name)
        {
            var category = await _categoryRepository.Get(x => x.Name.Equals(name));
            return MapperConfig.GetMapper().Map<CategoryDto>(category);
        }

        public async Task<int> Update(int id, UpdateCategoryDto dto)
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
                var category = MapperConfig.GetMapper().Map<Category>(dto);
                category.Id = id;
                var result = await _categoryRepository.Update(category);
                return category.Id;
            }
            else
            {
                throw new Exception(DUPLICATED+ " " + ClassName);
            }
        }

       

        public async Task<PaginationList<CategoryDto>> SearchAsync(CategoryPaging paging)
        {
            var result = await _categoryRepository.SearchAsync(paging);
            var items = MapperConfig.GetMapper().Map<List<CategoryDto>>(result.Items);
            return new PaginationList<CategoryDto>
            {
                Items = items,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalPage = result.TotalPage
            };
        }
    }
}
