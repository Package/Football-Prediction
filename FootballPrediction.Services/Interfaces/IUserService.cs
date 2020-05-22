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
        Task<ApplicationUser> GetUserByName(string userName);
        Task<ApplicationUser> GetById(string id);
    }

    public class UserService : IUserService
    {
        private readonly PredictionContext context;

        public UserService(PredictionContext context)
        {
            this.context = context;
        }

        public async Task<ApplicationUser> GetUserByName(string userName)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
