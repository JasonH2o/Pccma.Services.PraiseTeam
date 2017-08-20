using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using System.Web.OData.Routing;
using Pccma.PraiseTeam.Database.Contracts;
using Model = Pccma.PraiseTeam.Database.Model;
using System.Data.Entity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;
using System;

namespace Pccma.Services.PraiseTeam.Controllers
{
    public class PraiseTeamController: ODataController
    {
        protected readonly IPccmaPraiseTeamDataStoreContext _dataStoreContext;

        protected readonly IMapper _mapper;

        private const string EMPTY_DELTA_ERROR = "No properties to change in delta";

        public PraiseTeamController(IPccmaPraiseTeamDataStoreContext dataStoreContext, IMapper mapper)
        {
            _dataStoreContext = dataStoreContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ODataRoute(@"PraiseTeams")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeam>))]
        [EnableQuery]
        public IHttpActionResult GetPraiseTeams()
        {                        
            var memberToExpand = new Expression<Func<Dto.PraiseTeam, object>>[]
            {
                    x => x.PraiseTeamMembers
            };
            var praiseTeamsDto = _dataStoreContext.PraiseTeamRepository.Find().ProjectTo(_mapper.ConfigurationProvider, memberToExpand);            

            if (praiseTeamsDto != null && praiseTeamsDto.Any())
            {
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
            var memberToExpand = new Expression<Func<Dto.PraiseTeam, object>>[]
            {
                    x => x.PraiseTeamMembers
            };
            var praiseTeamDto = _dataStoreContext.PraiseTeamRepository.Find()
                                .ProjectTo(_mapper.ConfigurationProvider, memberToExpand)
                                .FirstOrDefault(x => x.Id == Id);

            if (praiseTeamDto != null)
            {                
                return Ok(praiseTeamDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ODataRoute(@"PraiseTeams")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeam>))]
        [EnableQuery]
        public IHttpActionResult CreatePraiseTeam(Dto.PraiseTeam praiseTeamDto)
        {
            //TODO: run validation for praiseTeamDto see if it's valid

            if(praiseTeamDto == null)
            {
                return InternalServerError();
            }
            
            var praiseTeam = _mapper.Map<Model.PraiseTeam>(praiseTeamDto);            

            var result = _dataStoreContext.PraiseTeamRepository.Add(praiseTeam);

            if(result != null)
            {
                return Ok(_mapper.Map<Dto.PraiseTeam>(result));
            }

            return InternalServerError();
        }

        [HttpPatch]
        [ODataRoute(@"PraiseTeams({Id})")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeam>))]
        [EnableQuery]
        public IHttpActionResult UpdatePraiseTeam(int Id, Delta<Dto.PraiseTeam> delta)
        {
            var praiseTeamDto = GetExistingPraiseTeam(Id);

            if (praiseTeamDto == null)
            {
                return NotFound();
            }
          
            if (delta == null || !delta.GetChangedPropertyNames().Any())
            {
                return BadRequest(EMPTY_DELTA_ERROR);
            }

            delta.Patch(praiseTeamDto);

            var praiseTeam = _mapper.Map<Model.PraiseTeam>(praiseTeamDto);

            var result = _dataStoreContext.PraiseTeamRepository.Update(praiseTeam);
            if(result == null)
            {
                return InternalServerError();
            }

            return Ok(_mapper.Map<Dto.PraiseTeam>(result));
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

            var praiseTeamDto = _mapper.Map<Dto.PraiseTeam>(existingPraiseTeam);

            return praiseTeamDto;
        }
    }
}