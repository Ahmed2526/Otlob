using AutoMapper;
using DAL.Consts;
using DAL.Dto_s;
using DAL.Entities;

namespace Otlob_API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, productDto>()
                .ForMember(dest => dest.ProductBrand, from => from.MapFrom(S => S.ProductBrand!.Name))
                .ForMember(dest => dest.ProductType, from => from.MapFrom(S => S.ProductType!.Name))
                .ForMember(dest => dest.PictureUrl, from => from.MapFrom(S => string.IsNullOrEmpty(S.PictureUrl) ? null : $"{miscellaneous.baseUrl}{S.PictureUrl}"));

        }
    }
}
