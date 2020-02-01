using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class GameWeek
    {
        [Key]
        public Guid Id { get; set; }

        public int InternalId { get; set; }

        public string Name { get; set; }

        public DateTime DeadlineDate { get; set; }

        public virtual List<Fixture> Fixtures { get; set; }
    }
}
