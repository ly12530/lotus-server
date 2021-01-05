﻿using Core.Domain;
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

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Id == id);
        }

        public async Task UpdateUser(User user)
        {
            await _context.SaveChangesAsync();
        }

        
    }
}
