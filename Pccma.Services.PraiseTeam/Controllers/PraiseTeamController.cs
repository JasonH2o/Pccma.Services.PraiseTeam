using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using System.Web.OData.Routing;
using Pccma.PraiseTeam.Database.Contracts;
using Model = Pccma.PraiseTeam.Database.Model;
using System.Collections.Generic;
using System.Data.Entity;

namespace Pccma.Services.PraiseTeam.Controllers
{
    public class PraiseTeamController: ODataController
    {
        protected readonly IPccmaPraiseTeamDataStoreContext _dataStoreContext;

        private const string EMPTY_DELTA_ERROR = "No properties to change in delta";

        public PraiseTeamController(IPccmaPraiseTeamDataStoreContext dataStoreContect)
        {
            _dataStoreContext = dataStoreContect;
        }

        [HttpGet]
        [ODataRoute(@"PraiseTeams")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeam>))]
        [EnableQuery]
        public IHttpActionResult GetPraiseTeam()
        {            
            var praiseTeams = _dataStoreContext.PraiseTeamRepository.Find().Include(x => x.PraiseTeamMembers);
            var praiseTeamsDto = new List<Dto.PraiseTeam>();

            if (praiseTeams != null && praiseTeams.Any())
            {
                foreach (var praiseTeam in praiseTeams)
                {
                    Dto.PraiseTeam praiseTeamDto = new Dto.PraiseTeam()
                    {
                        Id = praiseTeam.Id,
                        PraiseTeamName = praiseTeam.PraiseTeamName,
                        TeamLeader = praiseTeam.TeamLeader                        
                    };
                    var memberList = new List<Dto.PraiseTeamMember>();
                    foreach (var member in praiseTeam.PraiseTeamMembers)
                    {
                        var praiseTeamMember = new Dto.PraiseTeamMember()
                        {
                            Id = member.Id,
                            PraiseTeamId = member.PraiseTeamId,
                            MemberName = member.MemberName,
                            Specialties = member.Specialties
                        };
                        memberList.Add(praiseTeamMember);
                    }
                    praiseTeamDto.PraiseTeamMembers = memberList;
                    praiseTeamsDto.Add(praiseTeamDto);
                }
                return Ok(praiseTeamsDto);
            }else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ODataRoute(@"PraiseTeams({Id})")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeam>))]
        [EnableQuery]
        public IHttpActionResult GetPraiseTeam([FromODataUri] int Id)
        {
            var praiseTeam = _dataStoreContext.PraiseTeamRepository.Find().FirstOrDefault(x => x.Id == Id);
            
            if(praiseTeam != null)
            {
                var praiseTeamDto = new Dto.PraiseTeam { Id = praiseTeam.Id, PraiseTeamName = praiseTeam.PraiseTeamName, TeamLeader = praiseTeam.TeamLeader };
                return Ok(praiseTeamDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ODataRoute(@"PraiseTeams")]
        public IHttpActionResult CreatePraiseTeam(Dto.PraiseTeam praiseTeamDto)
        {
            //TODO: run validation for praiseTeamDto see if it's valid

            if(praiseTeamDto == null)
            {
                return InternalServerError();
            }
            var praiseTeam = new Model.PraiseTeam()
            {
                Id = praiseTeamDto.Id,
                PraiseTeamName = praiseTeamDto.PraiseTeamName,
                TeamLeader = praiseTeamDto.TeamLeader,
                PraiseTeamMembers = null
            };

            var result = _dataStoreContext.PraiseTeamRepository.Add(praiseTeam);

            if(result != null)
            {
                return Ok(result);
            }

            return InternalServerError();
        }

        [HttpPatch]
        [ODataRoute(@"PraiseTeams({Id})")]
        public IHttpActionResult UpdatePraiseTeam(int Id, Delta<Dto.PraiseTeam> delta)
        {
            var praiseTeamDto = GetExistingPraiseTeam(Id);

            if (praiseTeamDto == null)
            {
                return NotFound();
            }

            if (!delta.GetChangedPropertyNames().Any())
            {
                return BadRequest(EMPTY_DELTA_ERROR);
            }

            delta.Patch(praiseTeamDto);

            var praiseTeam = new Model.PraiseTeam()
            {
                Id = praiseTeamDto.Id,
                PraiseTeamName = praiseTeamDto.PraiseTeamName,
                TeamLeader = praiseTeamDto.TeamLeader,
                PraiseTeamMembers = null
            };

            var result = _dataStoreContext.PraiseTeamRepository.Update(praiseTeam);
            if(result == null)
            {
                return InternalServerError();
            }

            return Ok(result);
        }

        [HttpDelete]
        [ODataRoute(@"PraiseTeams({Id})")]
        public IHttpActionResult DeletePraiseTeam([FromODataUri] int Id)
        {            
            var isDeleted = _dataStoreContext.PraiseTeamRepository.Delete(Id);

            if (isDeleted)
            {
                return Ok();
            }
            return InternalServerError();
        }

        private Dto.PraiseTeam GetExistingPraiseTeam(int Id)
        {
            var existingPraiseTeam = _dataStoreContext.PraiseTeamRepository.Find().AsNoTracking().FirstOrDefault(x => x.Id == Id);

            var praiseTeamDto = new Dto.PraiseTeam() { Id = existingPraiseTeam.Id, PraiseTeamMembers = null,
                PraiseTeamName = existingPraiseTeam.PraiseTeamName, TeamLeader = existingPraiseTeam.TeamLeader };

            return praiseTeamDto;
        }
    }
}