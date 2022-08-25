using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace API.Controller.UsersController
{
    public class Get
    {
        public static string Route => "/api/GetUsers";
        
        [AllowAnonymous]
        public static async Task<IResult> Action(DataContext db)
        {
            var users = await db.Users.ToListAsync();

            return Results.Ok(users);
        }

    }
}