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
        public MemberController(Imember member)
        {
            _member = member;
        }
        [HttpPost("register")]
        public  IActionResult RegisterMember([FromBody] RegisterRequest request)
        {
            var registermemebr =  _member.Register(request);
            return Ok(registermemebr);
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request) {
            var loginmember = _member.Login(request);
            return Ok(loginmember);
        }
    }
}
