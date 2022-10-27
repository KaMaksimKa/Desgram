using AutoMapper;
using Desgram.Api.Models;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public UserController(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserDTO createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            return Ok(user);
        }
    }
}
