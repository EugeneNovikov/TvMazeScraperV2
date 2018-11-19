using AutoMapper;
using Shared.Models.Communication;
using Shared.Models.Presentation;

namespace IntegrationBl.Configurations
{
    public class IntegrationBlMappingProfile : Profile
    {
        public IntegrationBlMappingProfile()
        {
            CreateMap<TvShowInfo, TvShow>()
                .ForMember(d => d.Cast, opt => opt.MapFrom(s => s.Embedded.Cast));

            CreateMap<CastInfo, Actor>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Person.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Person.Name))
                .ForMember(d => d.Birthday, opt => opt.MapFrom(s => s.Person.Birthday));
        }
    }
}
