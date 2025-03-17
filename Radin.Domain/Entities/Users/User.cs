﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Users
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string? Gender { get; set; }
        public string? Job { get; set; }
        public int? Country {  get; set; }
        public int? Province { get; set; }//Province
        public int? City { get; set; }
        public DateTime? Age { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsRemove { get; set; }=false;
        public bool IsActive { get; set;}=true;
        public long BranchCode {  get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
