using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller.UsersController
{
    public class GetId
    {
        public static string Route => "/api/GetUsers/{Id}";

        public static async Task<IResult> Action([FromRoute] int id, DataContext db)
        {
            var user = await db.Users.FindAsync(id);

            return Results.Ok(user);
        }

    }
}