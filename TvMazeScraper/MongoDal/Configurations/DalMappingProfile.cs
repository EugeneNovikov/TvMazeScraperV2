using AutoMapper;
using MongoDal.Models;
using Shared.Models.Integration;

namespace MongoDal.Configurations
{
    public class DalMappingProfile : Profile
    {
        public DalMappingProfile()
        {
            CreateMap<HistoryRecord, HistoryRecordEntity>();

            CreateMap<HistoryRecordEntity, HistoryRecord>();
        }
    }
}