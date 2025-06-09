
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ToDoApi.Contracts;
using ToDoApi.Dto;
using ToDoApi.Infrastructure;
using ToDoApi.Models;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[action]")]
public class AuthenticationController(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    LoginUserWithRefreshToken loginUserWithRefreshToken,
    RevokeRefreshTokens revokeRefreshTokens)

    : Controller
{
    [HttpPost]
    public async Task<IActionResult> Registration(AuthenticationDto authenticationDto)
    {
        var user = await userRepository.GetUser(authenticationDto.email);
        if (user != null)
        {
            return BadRequest("Пользователь с такой почтой уже зарегестрирован");
        }
        var newUser = new UserModel
        {
            Email = authenticationDto.email,
            HashedPassword = passwordHasher.GenerateHash(authenticationDto.password),
        };
        await userRepository.AddUser(newUser);
        return Ok("Вы успешно зарегестрировались");

    }

    [HttpPost]
    
    public async Task<IActionResult> Login(AuthenticationDto authenticationDto)
    {
        var context = HttpContext;
        var user = await userRepository.GetUser(authenticationDto.email);
        if(user == null)
            throw new NullReferenceException("User not found");
        
        var result = passwordHasher.VerifyHash(authenticationDto.password,user.HashedPassword);
        if (!result)
        { 
           return BadRequest("Пароль неправильный");
        }
        var token = tokenProvider.GenerateToken(user);
        context.Response.Cookies.Append("access_token", token);
        var refreshToken = await tokenProvider.GenerateRefreshTokenModel(user);
         
        return Ok(new JwtDataDto(user.Id.ToString(),token, DateTime.Now.AddMinutes(Convert.ToDouble(20)).ToString("g"), user.Email,refreshToken.Token));
    }

    [HttpPost]
    public async Task<IActionResult> LoginRefreshToken(RefreshTokenAuthDto dto)
    {
        var context = HttpContext;
        var result = await loginUserWithRefreshToken.Handle(dto);
        context.Response.Cookies.Append("access_token", result.AccessToken);
        return Ok(result);
    }

    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> RevokeRefreshTokens(Guid id)
    {
        var result = await revokeRefreshTokens.Handle(id);
        if(!result)
            return BadRequest();
        return Ok("Token revoked");
    }
    
    
    
    [HttpPost]
    [Authorize]
    public IActionResult CheckTokens()
    {
        var context = HttpContext;
        var token = context.Request.Cookies["token"];
        return Ok(token);
    }
    
    
}