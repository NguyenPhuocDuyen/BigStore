using AutoMapper;
using BigStore.BusinessObject;
using BigStore.Dtos.CategoryDto;

namespace BigStore.Config.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryUpdateDto>();
        }
    }
}
