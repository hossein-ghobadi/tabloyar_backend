using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.NegotiationSet;
using Radin.Application.Services.Factors.Queries.ConnectionsGet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Branches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Queries.HesabfaBanksGet.GetBanksService;

namespace Radin.Application.Services.Factors.Queries.HesabfaBanksGet
{
    public interface IGetBanksService
    {
        Task<ResultDto<List<BankItem>>> BanksGetList(HttpClient client,long BranchCode);
    }

    public class GetBanksService : IGetBanksService
    {
        private readonly IDataBaseContext _context;
        public GetBanksService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<List<BankItem>>> BanksGetList(HttpClient client, long BranchCode)
        {
            try
            {
                string url1 = Environment.GetEnvironmentVariable("HESABFA_GET_BANKS");
                string url2 = Environment.GetEnvironmentVariable("HESABFA_GET_CASHES");
                string url3 = Environment.GetEnvironmentVariable("HESABFA_GET_PETTYCASHES");

                var BranchInfos = _context.BranchINFOs.FirstOrDefault(p => p.BranchCode == BranchCode);
                if (BranchInfos==null)
                {
                    return new ResultDto<List<BankItem>>
                    {
                        IsSuccess = false,
                        Message = "چنین شعبه ای وجود ندارد"
                    };
                }
                var Input = new
                {
                    apiKey = BranchInfos.apiKey,//"snoTL1UnT7KBb6LYTGjztaQhpVIVTVOX",//BranchInfos.apiKey,
                    userId = BranchInfos.HesabfaUserId,//"09101050112",// BranchInfos.HesabfaUserId,
                    password = BranchInfos.HesabfaPass,//"hossein50112",//BranchInfos.HesabfaPass,
                    loginToken = BranchInfos.loginToken,//"836677609bf81efb0004e70a3041af884a799d2115cde9d17a28c8707662c269fbb7240531bbf2f937dac8bad3933ec4",//BranchInfos.loginToken,
                };
                HttpResponseMessage response1 = await client.PostAsJsonAsync(url1, Input);////لیست بانک ها
                HttpResponseMessage response2 = await client.PostAsJsonAsync(url2, Input);////لیست صندوق ها
                HttpResponseMessage response3 = await client.PostAsJsonAsync(url3, Input);////لیست تن خواه گردان ها

                string responseBanksGet1 = await response1.Content.ReadAsStringAsync();
                string responseBanksGet2 = await response2.Content.ReadAsStringAsync();
                string responseBanksGet3 = await response3.Content.ReadAsStringAsync();

                var HesabfaApiResponseBanksGet1 = JsonSerializer.Deserialize<BanksGetResult>(responseBanksGet1);
                var HesabfaApiResponseBanksGet2 = JsonSerializer.Deserialize<BanksGetResult>(responseBanksGet2);
                var HesabfaApiResponseBanksGet3 = JsonSerializer.Deserialize<BanksGetResult>(responseBanksGet3);
                int Id = 0;

                if (!HesabfaApiResponseBanksGet1.Success||!HesabfaApiResponseBanksGet2.Success||!HesabfaApiResponseBanksGet3.Success)
                {
                    return new ResultDto<List<BankItem>>
                    {                   
                        IsSuccess = false,
                        Message = "دریافت ناموفق"
                    };
                }
                var BankListResult = new List<BankItem>();
                foreach (var bank in HesabfaApiResponseBanksGet1.Result)
                {
                    Id = Id + 1;
                    BankListResult.Add(new BankItem
                    {
                        id = Id,
                        label = $"بانک {bank.Name}",
                        banckId = bank.Id,
                        bankType = 0

                    }) ;
                }

                if (HesabfaApiResponseBanksGet2.Result.Count > 0)
                {
                    foreach (var bank in HesabfaApiResponseBanksGet2.Result)
                    {
                        Id = Id + 1;

                        BankListResult.Add(new BankItem
                        {
                            id = Id,
                            label = $"صندوق {bank.Name}",
                            banckId = Convert.ToInt32(bank.Code),
                            bankType = 1

                        });
                    }
                }


                
                if (HesabfaApiResponseBanksGet3.Result.Count > 0)
                {
                    foreach (var bank in HesabfaApiResponseBanksGet2.Result)
                    {
                        Id = Id + 1;
                        BankListResult.Add(new BankItem
                        {
                            id = Id,
                            label = $"تن خواه گردان  {bank.Name}",
                            banckId = Convert.ToInt32(bank.Code),
                            bankType = 2

                        });
                    }
                }
                BankListResult[0].isDefault = true;
                return new ResultDto<List<BankItem>>
                {
                    Data = BankListResult,
                    IsSuccess = true,
                    Message = "دریافت موفقیت آمیز"
                };
            }
            catch
            {
                return new ResultDto<List<BankItem>>
                {
                    IsSuccess = false,
                    Message = "خطا"
                };
            }
        }

        public class BanksGetResult
        {
            public bool Success { get; set; }
            public int ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            public List<HesabfaBanksDto> Result { get; set; }

        }
        public class BankItem
        {
            public int id { get; set; }
            public int banckId { get; set; }= 0;
            public int bankType { get; set; } = 0;   
            public string label { get; set; }
            public bool isDefault { get; set; }
        }
        public class HesabfaBanksDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }

    }
}
