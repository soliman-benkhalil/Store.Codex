using Microsoft.AspNetCore.Identity;
using Store.Codex.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Codex.Repository.Identity.DataSeed
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    Email = "solimanbenkhali74@gmail.com",
                    DisplayName = "Soliman Khalil",
                    UserName = "soliman.khalil",
                    PhoneNumber = "1234567890",
                    Address = new Address
                    {
                        FName = "Mostafa",
                        LName = "Nahas",
                        City = "Nasr City",
                        Country = "Egypt",
                        Street = "Elzahraa"
                    }
                };
                await _userManager.CreateAsync(user, "P@ssW0rd");
            }
        }
    }
}
