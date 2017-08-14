using System.Collections.Generic;

namespace Pccma.Services.PraiseTeam.Dto
{
    public class PraiseTeam: PrimaryKeyObject
    {
        public string PraiseTeamName { get; set; }
        public string TeamLeader { get; set; }
        public ICollection<PraiseTeamMember> PraiseTeamMembers { get; set; }
    }
}