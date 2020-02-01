using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FootballPrediction.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(maximumLength:25,ErrorMessage ="First Name should be 25 characters or less.")]
        public string FirstName { get; set; }

        [StringLength(maximumLength: 25, ErrorMessage = "Last Name should be 25 characters or less.")]
        public string LastName { get; set; }

        [StringLength(maximumLength: 25, ErrorMessage = "Team Name should be 25 characters or less.")]
        public string TeamName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
