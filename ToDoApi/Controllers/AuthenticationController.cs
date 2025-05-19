using Microsoft.AspNetCore.Mvc;
using ToDoApi.Contracts;
using ToDoApi.Dto;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[action]")]
public class AuthenticationController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider; 
    public AuthenticationController(IUserRepository userRepository ,IPasswordHasher passwordHasher, ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }

    [HttpPost]
    public async Task<IActionResult> Registration(AuthenticationDto authenticationDto)
    {
        var user = await _userRepository.GetUser(authenticationDto.email);
        if (user != null)
        {
            return BadRequest("Пользователь с такой почтой уже зарегестрирован");
        }
        var newUser = new UserModel
        {
            Email = authenticationDto.email,
            HashedPassword = _passwordHasher.GenerateHash(authenticationDto.password),
        };
        await _userRepository.AddUser(newUser);
        return Ok("Вы успешно зарегестрировались");

    }

    [HttpPost]
    public async Task<IActionResult> Login(AuthenticationDto authenticationDto)
    {
        var context = HttpContext;
        var user = await _userRepository.GetUser(authenticationDto.email);
        if(user == null)
            throw new NullReferenceException("User not found");
        
        var result = _passwordHasher.VerifyHash(authenticationDto.password,user.HashedPassword);
        if (!result)
        { 
           return BadRequest("Пароль неправильный");
        }
        var token = _tokenProvider.GenerateToken(user);
        context.Response.Cookies.Append("token", token);
        return Ok(new JwtDataDto(token, DateTime.Now.AddMinutes(Convert.ToDouble(20)).ToString("g"), user.Email));
    }
    
    
}