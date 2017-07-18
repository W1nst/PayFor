using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PayFor.Models;

namespace PayFor.Context
{
    public class PayForSeedData
    {
        private readonly PayForContext _context;
        private readonly UserManager<User> _userManager;

        public PayForSeedData(PayForContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SeedData()
        {
            if (await _userManager.FindByEmailAsync("test@test.ru") == null)
            {
                var user = new User() {UserName = "Test", Email = "test@test.ru",FirstName = "TestFN",LastName = "TestLN"};
                await _userManager.CreateAsync(user, "siemens24jUnG8!");
            }
            if (await _userManager.FindByEmailAsync("dimas_milk@mail.ru") == null)
            {
                var user = new User() { UserName = "Dmitriy", Email = "dimas_milk@mail.ru", FirstName = "Dmitriy", LastName = "Milykh" };
                await _userManager.CreateAsync(user, "siemens24jUnG8!");

                var cats = new List<Category>
                {
                    new Category() {Name = "Food"},
                    new Category() {Name = "Entertainment"},
                    new Category() {Name = "CarParts"},
                    new Category() {Name = "Gasoline"}
                };
                cats.ForEach(x => _context.Categories.Add(x));
                var group = new Group() {Name = "Family", Payments = new List<Payment>(), AuthorUser = user, CreateDateTime = DateTime.Now};
                _context.Groups.Add(group);

                var userGroup = new UserGroup() {User = user, Group = group};

                user.UserGroups = new List<UserGroup>() { userGroup };
                group.UserGroups = new List<UserGroup>() { userGroup };

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _context.Payments.AddRange(new List<Payment>()
                {
                    new Payment()
                    {
                        Amount = 1000,
                        Category = cats[0],
                        Date = DateTime.Now,
                        Note = "Market",
                        User = user,
                        Group = group
                    },
                    new Payment()
                    {
                        Amount = 1200,
                        Category = cats[3],
                        Date = DateTime.Now,
                        Note = "To Taman",
                        User = user,
                        Group = group
                    },
                    new Payment()
                    {
                        Amount = 1000,
                        Category = cats[0],
                        Date = DateTime.Now,
                        Note = "Market",
                        User = user,
                        Group = group
                    },
                    new Payment()
                    {
                        Amount = 1500,
                        Category = cats[1],
                        Date = DateTime.Now,
                        Note = "Cinema",
                        User = user,
                        Group = group
                    },
                    new Payment()
                    {
                        Amount = 2500,
                        Category = cats[0],
                        Date = DateTime.Now,
                        Note = "Bazar",
                        User = user,
                        Group = group
                    }
                });

                await _context.SaveChangesAsync();
            }
        } 
    }
}
