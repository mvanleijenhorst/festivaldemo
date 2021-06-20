using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FestivalDemo.WebServer.Authentication
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "demo01", Password = "qwerty2012" },
            new User { Id = 1, Username = "demo02", Password = "azerty2012" }
        };

        public async Task<User?> Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return await Task.FromResult(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.FromResult(_users);
        }
    }
}

