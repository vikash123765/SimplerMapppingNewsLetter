using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsLetterBanan.Data;
using NewsLetterBanan.Services.Interfaces;

namespace NewsLetterBanan.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUserService userService, ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }




        // user place comments and like on articles 


        // regstered users MyPAge 

        // user edit comment 

        // user delete comment 

        // user subreibe  on my page for reg user 

        //user unsubribe
    }
}
