using FootballPrediction.Data.Database;
using FootballPrediction.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByName(string userName, PredictionContext context);
        Task<ApplicationUser> GetById(string id, PredictionContext context);
    }

    public class UserService : IUserService
    {
        public async Task<ApplicationUser> GetUserByName(string userName, PredictionContext context)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<ApplicationUser> GetById(string id, PredictionContext context)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
