using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllUsers();
        Task RegisterUser(User newUser);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string emailAddress);
        Task UpdateUser(User user);
    }
}
