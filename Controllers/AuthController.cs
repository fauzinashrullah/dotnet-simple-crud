using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Data;
using SimpleApi.DTOs;
using SimpleApi.Models;
using SimpleApi.Helpers;

namespace SimpleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerator _tokenGen;

    public AuthController(AppDbContext context, JwtTokenGenerator tokenGen)
    {
        _context = context;
        _tokenGen = tokenGen;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Username sudah digunakan!");

        string hashPassword = PasswordHasher.CreateHash(dto.Password);

        var user = new User
        {
            Username = dto.Username,
            Password = hashPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registrasi berhasil!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user is null) return Unauthorized("User tidak ditemukan");

        if (!PasswordHasher.Verify(dto.Password, user.Password))
            return Unauthorized("Password salah");

        string token = _tokenGen.CreateToken(user);
        return Ok(new { token });
    }

}
