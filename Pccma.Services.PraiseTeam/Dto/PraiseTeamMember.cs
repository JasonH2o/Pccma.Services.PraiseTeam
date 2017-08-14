using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pccma.Services.PraiseTeam.Dto
{
    public class PraiseTeamMember: PrimaryKeyObject
    {
        public int PraiseTeamId { get; set; }        

        public string MemberName { get; set; }

        public string Specialties { get; set; }
    }
}