using AutoMapper;
using System;
using Database = Pccma.PraiseTeam.Database.Model;

namespace Pccma.Services.PraiseTeam.App_Start
{
    public static class AutoMapperConfig
    {
        public static IMapper RegisterMappingConfig()
        {
            var mapperConfiguration = new MapperConfiguration(config => {
                MapDatabaseToDto(config);
                MapDtoToDatabase(config);
            });

            return mapperConfiguration.CreateMapper();
        }


        private static void MapDtoToDatabase(IProfileExpression mapper)
        {
            mapper.CreateMap<Database.PraiseTeam, Dto.PraiseTeam>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.Id))
                .ForMember(x => x.PraiseTeamName, config => config.MapFrom(x => x.PraiseTeamName))
                .ForMember(x => x.TeamLeader, config => config.MapFrom(x => x.TeamLeader))
                .ForMember(x => x.PraiseTeamMembers, config => config.MapFrom(x => x.PraiseTeamMembers));

            mapper.CreateMap<Database.PraiseTeamMember, Dto.PraiseTeamMember>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.Id))
                .ForMember(x => x.PraiseTeamId, config => config.MapFrom(x => x.PraiseTeamId))
                .ForMember(x => x.MemberName, config => config.MapFrom(x => x.MemberName))
                .ForMember(x => x.Specialties, config => config.MapFrom(x => x.Specialties));                
    }

        private static void MapDatabaseToDto(IProfileExpression mapper)
        {
            mapper.CreateMap<Dto.PraiseTeam, Database.PraiseTeam>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.Id))
                .ForMember(x => x.PraiseTeamName, config => config.MapFrom(x => x.PraiseTeamName))
                .ForMember(x => x.TeamLeader, config => config.MapFrom(x => x.TeamLeader))
                .ForMember(x => x.PraiseTeamMembers, config => config.MapFrom(x => x.PraiseTeamMembers));

            mapper.CreateMap<Dto.PraiseTeamMember, Database.PraiseTeamMember>()
                .ForMember(x => x.Id, config => config.MapFrom(x => x.Id))
                .ForMember(x => x.PraiseTeamId, config => config.MapFrom(x => x.PraiseTeamId))
                .ForMember(x => x.MemberName, config => config.MapFrom(x => x.MemberName))
                .ForMember(x => x.Specialties, config => config.MapFrom(x => x.Specialties))
                .ForMember(x => x.PraiseTeam, config => config.Ignore());
        }
    }
}