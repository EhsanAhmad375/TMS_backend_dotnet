using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TMS.src.Data;
using TMS.src;
namespace TMS.src
{
    public interface IUserRepo
    {
        Task SaveChangesAsync();
        Task<UserModel> GetUserByEmail(string email); 
        Task<UserModel> GetUserById(int id);
        Task<UserModel> GetUserByNumber(string number);
        Task<UserModel> PostRegister(UserModel userModel);
        Task<UserModel> GetUserProfile(int userId);
        IQueryable<UserModel> GetAllUsers();
        

        

    }







    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _dbContext;
        public UserRepo(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        



        public async Task<UserModel?> GetUserByEmail(string email)
        {
            return await _dbContext.users.FirstOrDefaultAsync(u=>u.email==email|| u.phone_no==email);
        }

        public async Task<UserModel?> GetUserById(int id)
        {
            return await _dbContext.users.FindAsync(id);
        }

        public async Task<UserModel?> GetUserByNumber(string number)
        {
            return await _dbContext.users.FirstOrDefaultAsync(u=>u.phone_no==number);
        }


        public async Task<UserModel> PostRegister(UserModel userModel)
        {
             var user= await _dbContext.users.AddAsync(userModel);
             await SaveChangesAsync();
             return user.Entity;


        }
    




        public IQueryable<UserModel> GetAllUsers()
        {
            var users=_dbContext.users;
            return users;   
        }

        public Task<UserModel> GetUserProfile(int userId)
        {
            var user=GetUserById(userId);
            return user;
        }

    }
}