using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class Prediction
    {
        [Key]
        public Guid Id { get; set; }

        public virtual Fixture Fixture { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}
