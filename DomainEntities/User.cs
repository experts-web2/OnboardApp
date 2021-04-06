using Microsoft.AspNetCore.Identity;
using System;

namespace DomainEntities
{
    public class User : IdentityUser
    {
        public DateTime CreatedDate { get; set; }
    }
}