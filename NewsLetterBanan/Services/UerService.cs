using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsLetterBanan.Data;
using NewsLetterBanan.Services.Interfaces;

namespace NewsLetterBanan.Services
{
    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;



        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        private async Task AssignRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Ensure the role exists before assigning it
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // If the role doesn't exist, create it
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Assign the role to the user

                var result = await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        public async Task RemoveRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }

        Task IUserService.AssignRoleAsync(string userId, string roleName)
        {
            throw new NotImplementedException();
        }


        public async Task<string> CreateRole()
        {
            var result = await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            if (result.Succeeded)
            {

                return "sucess";


            }
            else
            {
                return "failure";
            }
        }

        public async Task AddUserAsync(User user)

        {
            // Check if the article is null before trying to add it to the database
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        }


    }
}
