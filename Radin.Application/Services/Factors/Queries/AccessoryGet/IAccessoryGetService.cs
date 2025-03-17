using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Queries.OrderGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Queries.AccessoryGet
{
    public interface IAccessoryGetService
    {
        ResultDto<List<AccessoryItem>> GetAccessories();
        ResultDto<AccessoryItemsEdit> GetForEdit(long FactorId,long subfactorId,long ProductId);
    }

    //

    public class AccessoryGetService : IAccessoryGetService
    {
        private readonly IDataBaseContext _context;

        public AccessoryGetService(IDataBaseContext context)
        {
            _context = context;


        }
        public ResultDto<List<AccessoryItem>> GetAccessories()
        {

            var Accessories = _context.Accessories.Select(p => new AccessoryItem
            {
                id=p.Id, label=p.Name,MinimumNumber=p.MinimumQuantity,fee=p.fee
            }).ToList();
            if (Accessories != null)
            {
                Accessories.First().IsDefault = true;
                return new ResultDto<List<AccessoryItem>>
                {
                    Data = Accessories,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            else
            {
                return new ResultDto<List<AccessoryItem>>
                {
                    Data = new List<AccessoryItem>(),
                    IsSuccess = false,
                    Message = "آیتمی موجود نیست"
                };
            }


        }





        public ResultDto<AccessoryItemsEdit> GetForEdit(long FactorId, long subfactorId, long ProductId)
        {

            var Accessory = _context.ProductFactors.Where(p => p.FactorID == FactorId && p.SubFactorID == subfactorId && p.Id == ProductId && !p.IsRemoved && p.IsAccessory&&!p.IsUndefinedProduct)
                .FirstOrDefault();
            if (Accessory == null)
            {
                return new ResultDto<AccessoryItemsEdit>
                {
                    Data = new AccessoryItemsEdit(),
                    IsSuccess = false,
                    Message = "آیتمی موجود نیست"
                };
            }
            var AccessoryInfo = _context.Accessories.Where(p => p.Name == Accessory.Name).Select(p => new {p.Id,p.MinimumQuantity}).FirstOrDefault();
            if (AccessoryInfo == null)
            {
                return new ResultDto<AccessoryItemsEdit>
                {
                    Data = new AccessoryItemsEdit(),
                    IsSuccess = false,
                    Message = "کالای مورد نظر به اشتباه ثبت شده است"
                };
            }
            var Result = new AccessoryItemsEdit
            {
                Acessory=new AccessoryItem2
                {
                    id =Convert.ToInt32(AccessoryInfo.Id),
                    label= Accessory.Name,
                    MinimumNumber = AccessoryInfo.MinimumQuantity,
                    fee = Accessory.fee,

                },
                count=Accessory.count,
                Discount=Accessory.Discount,
                price=Accessory.price,


            };
            
            return new ResultDto<AccessoryItemsEdit>
            {
                Data = Result,
                IsSuccess = true,
                Message = "دریافت موفق"

            };


        }
    }
    public class AccessoryItem
    {
        public long id { get; set; }
        public string label { get; set; }
        public float MinimumNumber {  get; set; }
        public float? fee { get; set; }
        public bool IsDefault { get; set; } = false;
        
    }

    public class AccessoryItem2
    {
        public long id { get; set; }
        public string label { get; set; }
        public float MinimumNumber { get; set; }
        public float? fee { get; set; }

    }
    public class AccessoryItemsEdit
    {
        public AccessoryItem2 Acessory { get; set; }
        
        public float Discount { set; get; }
        public int count { get; set; }
        public float price { set; get; }

    }
}
