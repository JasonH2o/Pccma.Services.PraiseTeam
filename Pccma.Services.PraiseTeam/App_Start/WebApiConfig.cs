using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace Pccma.Services.PraiseTeam
{
    public static class WebApiConfig
    {
        private const string PRAISE_TEAMS_ENTITY_SET_NAME = @"PraiseTeams";
        private const string PRAISE_TEAM_MEMBERS_ENTITY_SET_NAME = @"PraiseTeamMembers";
        private const string PRAISE_TEAMS_SERVICE_NAME = @"PraiseTeamsService";

        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Configure odata routing
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            builder.EntitySet<Dto.PraiseTeam>(PRAISE_TEAMS_ENTITY_SET_NAME);
            builder.EntitySet<Dto.PraiseTeamMember>(PRAISE_TEAM_MEMBERS_ENTITY_SET_NAME);

            config.MapODataServiceRoute(
                routeName: PRAISE_TEAMS_SERVICE_NAME,
                routePrefix: null,
                model: builder.GetEdmModel());            
        }        
    }
}
