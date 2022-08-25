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
    public class PostLogin
    {
        public static string Route => "/api/account/login";

        public static async Task<IResult> Action([FromBody] LoginDto loginDto, [FromServices] DataContext _context, ITokenService tokenService)
        {
            if(!MiniValidator.TryValidate(loginDto,out var errors))
                return Results.ValidationProblem(errors);
            var user = await _context.Users
            .SingleOrDefaultAsync(x =>x.UserName == loginDto.UserName.ToLower());

            if(user == null) return Results.BadRequest("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i<computedHash.Length;i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Results.BadRequest("Invalid Password");
            }
            
     
            return Results.Ok(new UserDto{
                UserName = user.UserName,
                Token = tokenService.CreateToken(user)
            });
        }

    }
}