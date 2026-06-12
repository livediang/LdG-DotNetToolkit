using AutoMapper;
using ProductCRUD.Application.DTOs;
using ProductCRUD.Domain.Entities;

namespace ProductCRUD.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap()
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());
        }
    }
}
