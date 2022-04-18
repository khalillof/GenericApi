using Microsoft.EntityFrameworkCore;
using GenericApi.Data;
using GenericApi.Models;
using Microsoft.AspNetCore.Identity;

namespace GenericApi
{
    public class SeedData
    {
        public static List<ApplicationUser> Users
        {
            get
            {
                var users = new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        UserName = "bob",
                        Email = "bob@bob.com",
                        PhoneNumber = "+111111111111",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        PasswordHash="Khalil$55"
                    },
                    new ApplicationUser()
                    {
                        UserName = "alice",
                        Email = "alice@alice.com",
                        PhoneNumber = "+111111111111",
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        PasswordHash="Khalil$55"

                    }
                };
                users.ForEach(c => {
                    c.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(c, c.PasswordHash);
                    c.NormalizedUserName = c.UserName.ToUpper().Normalize();
                    c.NormalizedEmail = c.Email.ToUpper().Normalize();
                });
                return users;
            }
        }
        public static void EnsureSeedData(IServiceProvider services)
        {

            using (var serviceProvider = services.CreateScope())
            {
                var context = serviceProvider.ServiceProvider.GetService<ApplicationDbContext>()!;

                     if(!context.AppUsers.Any())
                     {
                       CreateUsers(context).ConfigureAwait(true);
                     }

                    if (!context.Customers.Any())
                    {
  
                        foreach (var client in Customer.Customers)
                        {
                            context.Customers.Add(client);
                        }
                        context.SaveChanges();
                    }
            }
        }

        public static async Task CreateUsers(ApplicationDbContext context)
        {

            //roles
            string[] roles = new string[] { "Admin", "Owner", "User"};


            foreach (string role in roles)
            {

                if (!await context!.Roles.AnyAsync(r => r.Name == role))
                {
                    try
                    {

                        await context.Roles.AddAsync(new IdentityRole() { Name = role, NormalizedName = role.ToUpper().Normalize() });

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }


            }

            await context!.SaveChangesAsync();


            foreach (var user in Users)
            {

                if (!context!.AppUsers.Any(r => r.UserName == user.UserName))
                {
                    try
                    {
                        // user
                        await context.AppUsers.AddAsync(user);

                        // User role
                        var _role = await context.Roles.FirstOrDefaultAsync(c => c.Name == "Admin");
                        if (_role != null && !context.UserRoles.Any(c => c.UserId == user.Id && c.RoleId == _role.Id))
                        {
                            // add userRole
                            await context.UserRoles.AddAsync(new IdentityUserRole<string>() { UserId = user.Id, RoleId = _role.Id });
                            // add userRoleClaim
                            await context.RoleClaims.AddAsync(new IdentityRoleClaim<string>() { RoleId = _role.Id, ClaimType = _role.Name, ClaimValue = _role.Name });

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            await context!.SaveChangesAsync();

        }
    }
}
