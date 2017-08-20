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

namespace Pccma.Services.PraiseTeam.Controllers
{
    public class PraiseTeamMemberController: ODataController
    {
        protected readonly IPccmaPraiseTeamDataStoreContext _dataStoreContext;

        protected readonly IMapper _mapper;

        private const string EMPTY_DELTA_ERROR = "No properties to change in delta";

        public PraiseTeamMemberController(IPccmaPraiseTeamDataStoreContext dataStoreContext, IMapper mapper)
        {
            _dataStoreContext = dataStoreContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ODataRoute(@"PraiseTeamMembers")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeamMember>))]
        [EnableQuery]
        public IHttpActionResult GetPraiseTeamMembers()
        {         
            var praiseTeamMembersDto = _dataStoreContext.PraiseTeamMemberRepository.Find()
                                        .ProjectTo<Dto.PraiseTeamMember>(_mapper.ConfigurationProvider);

            if (praiseTeamMembersDto != null && praiseTeamMembersDto.Any())
            {
                return Ok(praiseTeamMembersDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ODataRoute(@"PraiseTeamMembers({Id})")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeamMember>))]
        [EnableQuery]
        public IHttpActionResult GetPraiseTeamMember([FromODataUri] int Id)
        {         
            var praiseTeamMemberDto = _dataStoreContext.PraiseTeamMemberRepository.Find()
                                .ProjectTo<Dto.PraiseTeamMember>(_mapper.ConfigurationProvider)
                                .FirstOrDefault(x => x.Id == Id);

            if (praiseTeamMemberDto != null)
            {
                return Ok(praiseTeamMemberDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ODataRoute(@"PraiseTeamMembers")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeamMember>))]
        [EnableQuery]
        public IHttpActionResult CreatePraiseTeamMember(Dto.PraiseTeamMember praiseTeamMemberDto)
        {
            //TODO: run validation for praiseTeamMemberDto see if it's valid

            if (praiseTeamMemberDto == null)
            {
                return InternalServerError();
            }

            var praiseTeamMember = _mapper.Map<Model.PraiseTeamMember>(praiseTeamMemberDto);

            var result = _dataStoreContext.PraiseTeamMemberRepository.Add(praiseTeamMember);

            if (result != null)
            {
                return Ok(_mapper.Map<Dto.PraiseTeamMember>(result));
            }

            return InternalServerError();
        }

        [HttpPatch]
        [ODataRoute(@"PraiseTeamMembers({Id})")]
        [ResponseType(typeof(IQueryable<Dto.PraiseTeamMember>))]
        [EnableQuery]
        public IHttpActionResult UpdatePraiseTeamMember(int Id, Delta<Dto.PraiseTeamMember> delta)
        {
            var praiseTeamMemberDto = GetExistingPraiseTeamMember(Id);

            if (praiseTeamMemberDto == null)
            {
                return NotFound();
            }

            if (!delta.GetChangedPropertyNames().Any())
            {
                return BadRequest(EMPTY_DELTA_ERROR);
            }

            delta.Patch(praiseTeamMemberDto);

            var praiseTeamMember = _mapper.Map<Model.PraiseTeamMember>(praiseTeamMemberDto);

            var result = _dataStoreContext.PraiseTeamMemberRepository.Update(praiseTeamMember);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok(_mapper.Map<Dto.PraiseTeamMember>(result));
        }

        [HttpDelete]
        [ODataRoute(@"PraiseTeamMembers({Id})")]
        public IHttpActionResult DeletePraiseTeamMember([FromODataUri] int Id)
        {
            var isDeleted = _dataStoreContext.PraiseTeamMemberRepository.Delete(Id);

            if (isDeleted)
            {
                return Ok();
            }
            return InternalServerError();
        }

        private Dto.PraiseTeamMember GetExistingPraiseTeamMember(int Id)
        {
            var existingPraiseTeamMember = _dataStoreContext.PraiseTeamMemberRepository.Find().AsNoTracking().FirstOrDefault(x => x.Id == Id);

            var praiseTeamMemberDto = _mapper.Map<Dto.PraiseTeamMember>(existingPraiseTeamMember);

            return praiseTeamMemberDto;
        }
    }
}
