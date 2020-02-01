using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class BootstrapStatic
    {
        public List<BootstrapStaticEvent> events { get; set; }

        public List<BootstrapStaticTeam> teams { get; set; }
    }

    public class BootstrapStaticEvent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string deadline_time { get; set; }
    }

    public class BootstrapStaticTeam
    {
        public string code { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string strength { get; set; }
        public string id { get; set; }
    }

    public class BootstrapFixture
    {
        public string code { get; set; }
        public string @event { get; set; }
        public string team_h { get; set; }
        public string team_a { get; set; }
        public string team_a_score { get; set; }
        public string team_h_score { get; set; }
        public string kickoff_time { get; set; }
    }
}
