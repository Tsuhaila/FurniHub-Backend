﻿using AutoMapper;
using FurniHub.Models.UserModels;
using FurniHub;
using Microsoft.EntityFrameworkCore;

namespace FurniHub.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UserService(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<List<OutPutUser>> GetUsers()
        {
            try
            {
                var users=await _context.Users.ToListAsync();
                return _mapper.Map<List<OutPutUser>>(users);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"retriving users{ex.Message}");
                return null;

            }

        }
        public async Task<OutPutUser> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }
                var mappedUser = _mapper.Map<OutPutUser>(user);
                return mappedUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"retrive user by id{ex.Message}");
                return null;

            }
        }
    }
}
