using NewsLetterBanan.Data;

namespace NewsLetterBanan.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task AssignRoleAsync(string userId, string roleName);
        Task RemoveRoleAsync(string userId, string roleName);
        public Task AddUserAsync(User user);
        public Task<string> CreateRole();
    }
}
