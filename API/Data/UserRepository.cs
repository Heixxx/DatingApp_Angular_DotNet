using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class UserRepository : IUserRepository {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context, IMapper mapper){
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams) { //NoTracking nie śledził tego, co wzwrócimy
        var query = _context.Users.AsQueryable();
        query = query.Where(u => u.UserName != userParams.CurrentUsername);
        query = query.Where(u => u.Gender == userParams.Gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

        return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider),userParams.PageNumber, userParams.PageSize);
    }

    public async Task<MemberDto> GetMemberAsync(string username) {
        // return await _context.Users.Where(x => x.UserName == username).Select(user => new MemberDto{       //PISANIE BEZ AUTOMAPPERA
        //     Id = user.Id,
        //     UserName = user.UserName,
        //     KnowAs = user.KnowAs //...
        // }).SingleOrDefaultAsync();

        return await _context.Users.Where(x => x.UserName == username).ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();

    }

    public async Task<AppUser> GetUserByIdAsync(int id) {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username) {
        return await _context.Users
        .Include(p => p.Photos)
        .SingleOrDefaultAsync(x=> x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync() {
        return await _context.Users
        .Include(p=>p.Photos)
        .ToListAsync();
    }

    public async Task<bool> SaveAllAsync() {
        return await _context.SaveChangesAsync() > 0;     //Sprawdza, czy zmiany są większe, niż 0.
    }

    public void Update(AppUser user) {
        _context.Entry(user).State = EntityState.Modified; 
    }
}