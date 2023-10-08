using AutoMapper;
using DAL.Dto_s;
using DAL.Entities;

namespace Otlob_API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, productDto>()
                .ForMember(dest => dest.ProductBrand, from => from.MapFrom(d => d.ProductBrand!.Name))
                .ForMember(dest => dest.ProductType, from => from.MapFrom(d => d.ProductType!.Name));


        }
    }
}
