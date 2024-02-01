using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers
{
    public class MappingProfiler : Profile
    {
        public MappingProfiler() 
        {
            CreateMap<AuctionCreated, Item>();
            CreateMap<AuctionUpdated, Item>();
        }
    }
}
