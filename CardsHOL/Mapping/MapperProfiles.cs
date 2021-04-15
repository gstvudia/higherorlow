using AutoMapper;
using CardsHOL.Api.DTOs;
using CardsHOL.Api.Entities;

namespace CardsHOL.Api.Mapping
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Card, CardDTO>()
                .ForMember(property => property.Number, options => options.MapFrom(
                    source => source.Number))
                .ForMember(property => property.Suit, options => options.MapFrom(
                    source => source.Suit));

            CreateMap<CardDTO, Card>()
                .ForMember(property => property.Number, options => options.MapFrom(
                    source => source.Number))
                .ForMember(property => property.Suit, options => options.MapFrom(
                    source => source.Suit));

        }
    }
}
