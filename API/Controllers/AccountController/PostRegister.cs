using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

namespace API.Controller.AccountController
{
    public class PostRegister
    {
        public static string Route => "/api/account/register";

        public static async Task<IResult> Action([FromBody] RegisterDto registerDto, [FromServices] DataContext _context, ITokenService tokenService)
        {
            if(!MiniValidator.TryValidate(registerDto,out var errors))
                return Results.ValidationProblem(errors);

            if(await UserExists(registerDto.UserName, _context)) return Results.BadRequest("Username is taken");
            using var hmac = new HMACSHA512();
            
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Results.Ok(new UserDto{
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            });
        }

        private static async Task<bool> UserExists(string username, [FromServices] DataContext _context)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}