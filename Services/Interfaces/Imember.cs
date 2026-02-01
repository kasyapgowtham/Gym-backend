using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface Imember
    {
        Task Register(RegisterRequest request);

        Task Login(LoginRequest request);
    }
}
