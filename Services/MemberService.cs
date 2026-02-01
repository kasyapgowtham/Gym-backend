using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace backend.Services
{
    public class MemberService : Imember
    {
        private readonly GymDbContext _dbContext;
        private readonly PasswordHasher<Member> _passwordHasher = new PasswordHasher<Member>();

        public MemberService(GymDbContext dbContext)
        {
            _passwordHasher = new PasswordHasher<Member>();
            _dbContext = dbContext;
        }
        public async Task<Member> Register(RegisterRequest request) {
            var member = new Member
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,

            };
            member.passwordhash=_passwordHasher.HashPassword(member, request.password);
            _dbContext.Members.Add(member);
            await _dbContext.SaveChangesAsync();
            return member;
        }
        public async Task Login(LoginRequest request) { 
            var user=_dbContext.Members.FirstOrDefault(u=>u.Email == request.Email);
            if (user == null) {
                throw new Exception("Email not found");
            }
            var verifypassword = _passwordHasher.VerifyHashedPassword(user, user.passwordhash, request.Password);
            if (verifypassword == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invaalid password");
            }
        }
    }
}
