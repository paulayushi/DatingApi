using System;
using System.Threading.Tasks;
using DatingApi.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string user, string password){
            var username = await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync( x=> x.Username==user);
            if(username == null)
                return null;
            if(!VerifyPasswordHash(password, username.PasswordHash, username.PasswordSalt))
                return null;
            
            return username;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hkey = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var computedPassword = hkey.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i<computedPassword.Length; i++){
                    if(computedPassword[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password){
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
            using(var hkey = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hkey.Key;
                passwordHash = hkey.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> UserExist(string username)
        {
            if( await _context.Users.AnyAsync(x=> x.Username == username))
                return true;
            return false;
        }
    }
}