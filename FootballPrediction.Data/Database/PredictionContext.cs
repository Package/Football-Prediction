using FootballPrediction.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Database
{
    public class PredictionContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<GameWeek> GameWeeks { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Prediction> Predictions { get; set; }

        public PredictionContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static PredictionContext Create()
        {
            return new PredictionContext();
        }
    }
}
