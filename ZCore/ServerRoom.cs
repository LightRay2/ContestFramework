using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{

    //todo удаленные программы?
    public class ServerRoom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool VisibleOnlyFromAdminsAndParticipants { get; set; }

        public bool AllowWatchOnlyMineMatches { get; set; }

        public bool AllowAddMatchesToAll { get; set; }

        public int MatchAddLimitPerDay { get; set; }

        public string PlayersSeparatedBySpace
    }
}
