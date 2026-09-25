using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace E_Commerce.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
