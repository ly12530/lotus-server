using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface IUserRepository
    {
        Task RegisterUser(User newUser);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string emailAddress);
        Task UpdateUser(User user);
        IQueryable<User> GetUsers();
        IEnumerable<User> GetUserByRole(Role role);
    }
}
