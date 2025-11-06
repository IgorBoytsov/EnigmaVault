using AutoMapper;
using EnigmaVault.PasswordService.Domain.Models;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Application.Mappers.Profiles
{
    internal sealed class IconProfile : Profile
    {
        public IconProfile()
        {
            CreateMap<Icon, IconResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId != null ? src.UserId.Value : (Guid?)null))
                .ForMember(des => des.IconName, opt => opt.MapFrom(src => src.IconName))
                .ForMember(des => des.SvgCode, opt => opt.MapFrom(src => src.SvgCode))
                .ForMember(des => des.IconCategoryId, opt => opt.MapFrom(src => src.IconCategoryId));
        }
    }
}