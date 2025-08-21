using AutoMapper;
using UrlShortener.DTOs;
using UrlShortener.Models;

namespace UrlShortener.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShortUrl, ShortUrlDto>();
        }
    }
}