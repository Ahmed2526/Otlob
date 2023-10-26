using AutoMapper;
using DAL.Consts;
using DAL.Dto_s;
using DAL.Entities;
using DAL.Entities.Identity;

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

            CreateMap<ApplicationUser, UserAccountDto>()
                .ForMember(dest => dest.Country, from => from.MapFrom(S => S.Address!.Country))
                .ForMember(dest => dest.City, from => from.MapFrom(S => S.Address!.City))
                .ForMember(dest => dest.Street, from => from.MapFrom(S => S.Address!.Street))
                .ForMember(dest => dest.ZipCode, from => from.MapFrom(S => S.Address!.ZipCode));

        }
    }
}
