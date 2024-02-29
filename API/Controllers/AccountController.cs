using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController {

    private readonly DataContext _context;
    private readonly ITokenServices _tokenServices;
    public AccountController(DataContext context, ITokenServices tokenServices) {
        _context = context;
        _tokenServices = tokenServices;
    }

    [HttpPost("register")]               //Zwraca Task<ActionResult<...>>. Otrzymuje (RegisterDto registerDto)
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) { //[FromBody] - Gdy nie ma ApiController

        if (await UserExists(registerDto.Username))
            return BadRequest("Username is taken");
        using var hmac = new HMACSHA512();
        var user = new AppUser {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDto{
            Username = user.UserName,
            Token = _tokenServices.CreateToken(user)
        };
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto logindto) {                //Zwraca obiekt AppUser

        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == logindto.Username);   //Szuka username w bazie danych i pobiera username z loginDTO do sprawdzenia.
        if (user == null)
            return Unauthorized("Invalid username");           //Wywalilo blad, poniewaz nie mam ActionResult i nie moge zwrócić wyniku http/ 

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));
        for (int i = 0; i < computedHash.Length; i++) {
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid password");
        }
        return new UserDto{
            Username = user.UserName,
            Token = _tokenServices.CreateToken(user)
        };
    }
    
    private async Task<bool> UserExists(string username) {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
