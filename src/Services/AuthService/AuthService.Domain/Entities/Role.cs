﻿using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<UserCompanyRole> UserCompanyRoles { get; set; } = new List<UserCompanyRole>();
    }
}
