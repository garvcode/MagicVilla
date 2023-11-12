using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();

            CreateMap<Villa, VillaCreateDto>();
            CreateMap<VillaCreateDto, Villa>();

            CreateMap<Villa, VillaUpdateDto>();
            CreateMap<VillaUpdateDto, Villa>();

        }
    }
}
