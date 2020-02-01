using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class Fixture
    {
        [Key]
        public Guid Id { get; set; }

        public virtual GameWeek GameWeek { get; set; }

        public virtual Team HomeTeam { get; set; }

        public virtual Team AwayTeam { get; set; }

        public int? HomeScore { get; set; }

        public int? AwayScore { get; set; }

        public DateTime KickoffDate { get; set; }

        public int InternalId { get; set; }
    }
}
