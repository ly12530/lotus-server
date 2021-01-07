using Core.Domain;
using Core.DomainServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly LotusDbContext _context;

        public UserRepository(LotusDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users
                .Include(user => user.Requests)
                .Include(user => user.Jobs)
                .SingleOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetUserByEmail(string emailAddress)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.EmailAddress == emailAddress);
        }

        public async Task UpdateUser(User user)
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<User> GetUsers()
        {
            return _context.Users;
        }

        public IEnumerable<User> GetUserByRole(Role role)
        {
            return _context.Users.Where(g => g.Role == role);
        }
    }
}
