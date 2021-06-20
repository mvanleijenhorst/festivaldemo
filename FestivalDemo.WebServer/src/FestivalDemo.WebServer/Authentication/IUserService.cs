using System.Collections.Generic;
using System.Threading.Tasks;

namespace FestivalDemo.WebServer.Authentication
{
    public interface IUserService
    {
        public Task<User?> Authenticate(string username, string password);
        public Task<IEnumerable<User>> GetAll();
    }
}
