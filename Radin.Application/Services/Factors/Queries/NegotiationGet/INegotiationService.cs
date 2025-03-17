using Microsoft.AspNetCore.Identity;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Queries.AccessoryGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Others;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Queries.NegotiationGet.NegotiationService;

namespace Radin.Application.Services.Factors.Queries.NegotiationGet
{
    public interface INegotiationService
    {
        ResultDto<ResultNegotioationGet> GetInformation(long factorId, long branchCode);
        ResultDto<GetNeedForNegotiationInfo> GetNeededDataList(long branchCode);

    }
    public class NegotiationService : INegotiationService
    {
        private readonly IPriceFeeDataBaseContext _databaseCotext;
        private readonly IDataBaseContext _context;
        private readonly UserManager<User> _userManager;

        public NegotiationService(IDataBaseContext context,
            UserManager<User> userManager,
            IPriceFeeDataBaseContext dataBaseContext
            )
        {

            _context = context;
            _userManager = userManager;
            _databaseCotext = dataBaseContext;


        }
        public ResultDto<ResultNegotioationGet> GetInformation(long factorId, long branchCode)
        {
            try
            {
                var factor = _context.MainFactors.Where(p => p.Id == factorId && p.BranchCode == branchCode && !p.IsRemoved && !p.position).FirstOrDefault();
                var MainSeller = new IdLabelString();
                var AsisstantSeller = new IdLabelString();
                //var Customer = new IdLabelString();
                if (factor == null)
                {
                    return new ResultDto<ResultNegotioationGet>
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "چنین فاکتوری وجود ندارد"
                    };

                }

                string MainSellerName = _userManager.FindByIdAsync(factor.MainsellerID ?? "0")?.Result?.FullName ?? null;
                string AssistantSellerName = _userManager.FindByIdAsync(factor.AssistantSellerID ?? "0")?.Result?.FullName ?? null;
                var RecommendedDesign = _databaseCotext.Titles.FirstOrDefault(p => p.Id == factor.RecommandedDesign);
                var Customer = _context.CustomerInfo.FirstOrDefault(p => p.Id == factor.CustomerID);
                MainSeller.id = factor.MainsellerID ?? "0";
                AsisstantSeller.id = factor.AssistantSellerID ?? "0";
                MainSeller.label = MainSellerName;
                AsisstantSeller.label = AssistantSellerName;
                var ContactTypeData = _context.ContactTypeInfo.FirstOrDefault(p => p.Id == factor.ContactType);
                var ContactType = "نامعین";
                if (ContactTypeData != null)
                {
                    ContactType = ContactTypeData.type;
                }
                var statusReason = new IdLabelDto { id = 0, label = "" };
                statusReason.label = factor.ReasonStatus;

                var StatusReasonId = _context.StatusReasons.Where(p => p.Reason == factor.ReasonStatus).FirstOrDefault();
                if (StatusReasonId != null)
                {
                    statusReason.id = Convert.ToInt32(StatusReasonId.Id);
                }

                var ConnectionDataList = _context.CustomerConnections.Where(p => p.FactorID == factorId).Select(s => new ConnectionsData
                {
                    Id = s.Id,
                    ConnectinTime = ConvertToMilliseconds(s.ConnectinTime),
                    ConnectionDuration = TimeSpan.FromMinutes(s.ConnectionDuration).ToString(@"hh\:mm"),
                    ContactType = s.ContactTypeName,

                }).ToList();
                var SellerList = _userManager.Users.Where(p => p.BranchCode == branchCode&&p.Id!=MainSeller.id).Select(p => new IsDefaultIdLabel
                {
                    id = p.Id,
                    label = p.FullName
                }
                           ).ToList();
                var seller = SellerList.FirstOrDefault(s => s.id == factor.AssistantSellerID);

                if (seller != null)
                {
                    seller.IsDefault = true;
                }

                var recommendedDesignList = _databaseCotext.Titles.Select(p => new IsDefaultIdLabel
                {
                    id = p.Id.ToString(),
                    label = p.TitleName
                }
                           ).ToList();

                var ContactTypeList = _context.ContactTypeInfo.Select(p => new IdLabelDto
                {
                    id = Convert.ToInt32(p.Id),
                    label = p.type
                }
                    ).ToList();
               

                var Design = recommendedDesignList.FirstOrDefault(s => Convert.ToUInt32(s.id) == factor.RecommandedDesign);

                if (Design != null)
                {
                    Design.IsDefault = true;
                }


                var Reasons = _context.StatusReasons.Where(s => s.status == factor.status);

                //int rowsCount = 0;
                var ReasonsList = Reasons.Select(r => new IdLabelDto
                {
                    id = Convert.ToInt32(r.Id),
                    label = r.Reason,
                }).ToList();
                var Result = new ResultNegotioationGet
                {
                    branchCode = factor.BranchCode,
                    factorNumber = factor.Id,
                    mainSeller = MainSeller,
                    asisstantSeller = AsisstantSeller,
                    recommendedDesign = new IdLabelDto { id = RecommendedDesign?.Id ?? 0, label = RecommendedDesign?.TitleName ?? null },
                    description = factor.description,
                    purchaseProbability = Convert.ToString(factor.PurchaseProbability),
                    status = factor.status,
                    customer = new IdLabelDto(),
                    reasonStatus = statusReason,
                    ConnectionsCount = factor.ConnectionCount,
                    ConnectionsDuration = factor.ConnectionDuration,
                    ContactType = ContactType,
                    ConnectionsData = ConnectionDataList,
                    SellerList = SellerList,
                    recommendedDesignList = recommendedDesignList,
                    ContactTypeList= ContactTypeList,
                    ReasonsList= ReasonsList
                };
                if (Customer?.CustomerID != null)
                {
                    Result.customer = new IdLabelDto
                    {
                        id = Convert.ToInt32(Customer.CustomerID),
                        label = $"{Customer.Name} {Customer.LastName}"
                    };
                }
                return new ResultDto<ResultNegotioationGet>
                {
                    Data = Result,
                    IsSuccess = true,
                    Message = "اطلاعات مذاکره با موفقیت دریافت شد"
                };
            }
            catch
            {
                return new ResultDto<ResultNegotioationGet>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "خطا در دریافت اطلاعات"
                };
            }
        }





        public ResultDto<GetNeedForNegotiationInfo> GetNeededDataList(long branchCode)
        {
            try
            {
                var SellerList = _userManager.Users.Where(p => p.BranchCode == branchCode).Select(p => new IdLabelString
                {
                    id = p.Id,
                    label = p.FullName
                }
                           ).ToList();


                var recommendedDesignList = _databaseCotext.Titles.Select(p => new IdLabelIsDefault
                {
                    id = p.Id,
                    label = p.TitleName,
                    isDefault = false
                    
                }
                           ).ToList();

                recommendedDesignList[0].isDefault = true;
                return new ResultDto<GetNeedForNegotiationInfo>
                {
                    Data = new GetNeedForNegotiationInfo
                    {
                        SellerList= SellerList,
                        recommendedDesignList= recommendedDesignList
                    },
                    IsSuccess = true,
                    Message = "دریافت موفقیت آمیز"
                };
            }
            catch {

                return new ResultDto<GetNeedForNegotiationInfo>
                {
                    IsSuccess = false,
                    Message = "خطا در دریافت اطلاعات"
                };
            }


        }
        public static long ConvertToMilliseconds(DateTime dateTime)
        {
            // Convert the DateTime to UTC to ensure consistency
            DateTimeOffset dateTimeOffset = dateTime.ToUniversalTime();
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
    }

    public class GetNeedForNegotiationInfo
    {
        public List<IdLabelString> SellerList { get; set; }
        public List<IdLabelIsDefault> recommendedDesignList { get; set; }

    }



    public class ResultNegotioationGet
    {
        public long branchCode { get; set; }
        public long factorNumber { get; set; }
        public IdLabelString mainSeller { get; set; }
        public IdLabelString asisstantSeller { get; set; }
        public IdLabelDto customer { get; set; }
        public IdLabelDto recommendedDesign { get; set; }
        public string description { get; set; }
        public string purchaseProbability { get; set; }
        public bool status { get; set; }
        public IdLabelDto reasonStatus { get; set; }
        public int? ConnectionsDuration { get; set; }
        public int? ConnectionsCount { get; set; }
        public string ContactType { get; set; }
        public List<ConnectionsData> ConnectionsData { get; set; }
        public List<IsDefaultIdLabel> SellerList { get; set; }
        public List<IsDefaultIdLabel> recommendedDesignList { get; set; }
        public List<IdLabelDto> ReasonsList { get; set; }
        public List<IdLabelDto> ContactTypeList { get; set; }
    }


    public class ConnectionsData
    {



        public long Id { get; set; }
        public long ConnectinTime { get; set; }
        public string ConnectionDuration { get; set; }
        public string ContactType { get; set; }


    }

    public class IsDefaultIdLabel:IdLabelString {

        public bool IsDefault { get; set; } = false;
    }



    }