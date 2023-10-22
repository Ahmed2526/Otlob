using DAL.Data;
using DAL.Entities;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Identity.SeedData
{
    public class SeedAdmins
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> _userManager)
        {
            var user = await _userManager.FindByEmailAsync("adminA1@gmail.com");

            if (user is null)
            {
                var User = new ApplicationUser()
                {
                    FirstName = "Ahmed",
                    LastName = "Ebrahim",
                    Email = "adminA1@gmail.com",
                    UserName = "adminA1",
                    PhoneNumber = "01551759541",
                };

                var address = new Address()
                {
                    Country = "Egypt",
                    City = "KafrShokr",
                    Street = "Libya",
                    ZipCode = "13718"
                };

                User.Address = address;

                var result = await _userManager.CreateAsync(User, "adminA1@gmail.com");
            }

        }
    }
}
