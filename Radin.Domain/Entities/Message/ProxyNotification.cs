using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Message
{
    public class ProxyNotification:BaseEntity
    {
        public int Id { get; set; } 
        public long FactorId { get; set; }
        public long BranchCode {  get; set; }
        public string WorkName { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime? SeenTime { get; set; }
        public bool IsActive { get; set; }=true;
        
    }
}
