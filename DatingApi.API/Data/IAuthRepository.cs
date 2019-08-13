using System.Threading.Tasks;
using DatingApi.API.Models;

namespace DatingApi.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string user, string password);
         Task<bool> UserExist(string username);
    }
}