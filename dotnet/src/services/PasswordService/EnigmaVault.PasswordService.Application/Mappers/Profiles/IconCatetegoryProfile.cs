using AutoMapper;
using EnigmaVault.PasswordService.Domain.Models;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Mappers.Profiles
{
    internal sealed class IconCategoryProfile : Profile
    {
        public IconCategoryProfile()
        {
            CreateMap<IconCategory, IconCategoryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId != null ? src.UserId.Value : (Guid?)null))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}