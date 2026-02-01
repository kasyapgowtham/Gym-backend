using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface Imember
    {
        Task<Member> Register(RegisterRequest request);

        Task Login(LoginRequest request);
    }
}
