using Radin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.Branches
{
    public class BranchINFO: BaseEntity
    {
        public long Id { get; set; }
        public long BranchCode { get; set; }
        public string BranchName { get; set; }
        public int BranchCity { get; set; }
        public int BranchProvince { get; set; }//BranchProvince
        public int BranchCountry { get; set; }
        public string? BranchAddress { get; set; }
        public string? PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BranchPhone1 { get; set; }
        public string? BranchPhone2 { get; set;}
        public string? TelegramId { get; set; }
        public string? EitaId { get; set; }
        public string? WhatsAppId { get; set; }
        public string? InstagramId { get; set; }
        public float BranchDiscount {  get; set; }
        public float InitialPayment { get; set;}
        public float NonCashAddingPayment { get; set; } = 0;

        public int? Star {  get; set; }
        public int? ActivityHistory { get; set; }
        public string? Description { get; set; }
        public string? MainImage { get; set; }
        public string? Images { get; set; }
        public string? DashboardLink { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? CloseingTime { get; set;}
        public string? apiKey  { get; set;}
        public string? loginToken { get; set; }
        public string? HesabfaUserId { get; set; }
        public string? HesabfaPass { get; set; }
    }
}
