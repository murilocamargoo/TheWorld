using System;
using Microsoft.AspNetCore.Identity;

namespace TheWorld.Models
{
    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}
