using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.ContactUs
{
    public class ContactMessage: BaseEntity
    {
        public int Id { get; set; } 
        public string ReleventUnit { get; set; }
        public string? Title { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }=false; 
    }
}
