using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int InternalId { get; set; }

        public int InternalCode { get; set; }

        public int Strength { get; set; }
    }
}
