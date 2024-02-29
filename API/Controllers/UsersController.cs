using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]           //Ustawiamy w Program.cs z serwisami.
public class UserController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserController(IUserRepository userRepository, IMapper mapper){
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){           //async Task - Pomaga w odblokowaniu wątków.
        // var users = await _userRepository.Users.ToListAsync();                //await .ToListAsync  - Wykonuje sie i zwraca wynik gdy skonczy nie blokujac funkcji.
        // return users;                                                       //Wywala blad konwersji dlatego nizej robimy tak:
        // return Ok(await _userRepository.GetUsersAsync());                  //Stary sposób naprawy błędu.
        var users = await _userRepository.GetUsersAsync();
        var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(usersToReturn);
    }
    
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username){

        var user =  await _userRepository.GetUserByUsernameAsync(username);
        var usersToReturn = _mapper.Map<MemberDto>(user);
        return usersToReturn;
    }
}