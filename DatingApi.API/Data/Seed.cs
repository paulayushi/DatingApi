using System.Collections.Generic;
using DatingApi.API.Models;
using Newtonsoft.Json;

namespace DatingApi.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers(){
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
            using(var hkey = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hkey.Key;
                passwordHash = hkey.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
    }
}