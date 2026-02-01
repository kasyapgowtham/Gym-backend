using backend.DTOs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly Imember _member;
        private readonly IEmailService _emailService;
        public MemberController(Imember member, IEmailService emailService)
        {
            _member = member;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterMember([FromBody] RegisterRequest request)
        {
            await   _member.Register(request);
            await _emailService.SendWelcomeMail(request.Email);
            return Ok();
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request) {
            var loginmember = _member.Login(request);
            return Ok(loginmember);
        }
    }
}
