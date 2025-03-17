using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.UpdatePrice;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Common;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.RecordProduct
{
    public class RecordProductService:IRecordProductService
    {

        private readonly IDataBaseContext _context;
        private readonly IUpdatePrice _updatePrice;
        public RecordProductService(IDataBaseContext context,IUpdatePrice updatePrice)
        {
            _context = context;
            _updatePrice = updatePrice;
        }

        public async Task<ResultDto<long,long>> HandleRecording(RecordRequest request)
        {
            // Check if the price calculation was successful
            if (!request.priceIsSuccess)
            {
                return new ResultDto<long, long>
                {
                    Data = 0,
                    IsSuccess = false,
                    Message = request.priceMessage
                };
            }

            // Ensure FactorId is provided
            if (request.factorId==0)
            {
                return new ResultDto<long, long>
                {
                    Data = 0,
                    IsSuccess = false,
                    Message = "شماره فاکتور وجود ندارد"
                };
            }

            // Retrieve the product factor along with related subfactors
            var productFactor = await _context.MainFactors
                                              .Include(m => m.SubFactors)
                                              .FirstOrDefaultAsync(p => p.Id == request.factorId && !p.IsRemoved);
            
            if (productFactor == null)
            {
                return new ResultDto<long, long>
                {
                    Data = 0,
                    IsSuccess = false,
                    Message = "فاکتوری با شماره مورد نظر وجود ندارد"
                };
            }
            float Price = request.QualityFactor switch
            {
                ConstantMaterialName.QualityFactor_A2plus => request.productCost.Price_A2plus,
                ConstantMaterialName.QualityFactor_Aplus => request.productCost.Price_Aplus,
                ConstantMaterialName.QualityFactor_A => request.productCost.Price_A,
                ConstantMaterialName.QualityFactor_B => request.productCost.Price_B,
                _ => 0 // Default case if QF doesn't match
            };
            if (request.QualityFactor == ConstantMaterialName.QualityFactor_Aplus)
            {
                Price = request.productCost.Price_Aplus;
            }
            Console.WriteLine($@"price={Price}");

            var state = productFactor.state;
            
            // Price calculation for new or existing subfactor
            using (var transaction = await _context.BeginTransactionAsync())
            {
                try
                {
                    if (state == 0)
                    {
                        productFactor.state = 1;
                    }
                    // Check if SubFactorId is null (create a new subfactor)
                    if (request.subFactorId ==0)
                    {
                        if (request.productId > 0)
                        {
                            return new ResultDto<long, long>
                            {
                                Data=0,
                                IsSuccess = false,
                                Message = "محصولی وجود ندارد"
                            };
                        }

                        // Create a new subfactor
                        var newSubFactor = new SubFactor
                        {
                            FactorID = request.factorId.Value,
                            Amount = 0,
                            Description = request.description,
                            QualityFactor=request.QualityFactor?? ConstantMaterialName.QualityFactor_Aplus,
                        };

                        await _context.SubFactors.AddAsync(newSubFactor);
                        await _context.SaveChangesAsync();
                        _context.MainFactors.Update(productFactor);
                        // Create a new product factor associated with the new subfactor
                        var newProduct = new ProductFactor
                        {
                            FactorID = request.factorId.Value,
                            SubFactorID = newSubFactor.Id,
                            Name = request.productName,  // Example: Use productId to generate name
                            fee = Price,
                            price = Price,
                            priceA2plus= request.productCost.Price_A2plus,
                            priceAplus =request.productCost.Price_Aplus,
                            priceA=request.productCost.Price_A,
                            priceB=request.productCost.Price_B,
                            NestingResult=request.NestingResult,
                            ProductDetails=request.ProductDetails,
                            IsAccessory=request.IsAccessory,

                            
                        };

                        await _context.ProductFactors.AddAsync(newProduct);
                        await _context.SaveChangesAsync();
                        await UpdateSubFactorAmount(newSubFactor.Id, request.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus);

                        await transaction.CommitAsync();

                        return new ResultDto<long, long>
                        {
                            SupplemantaryData= newProduct.Id,
                            Data = newSubFactor.Id,
                            IsSuccess = true,
                            Message = "فاکتور جدید با زیر فاکتور و محصول درج شد"
                        };
                    }
                    else
                    {
                        // SubFactorId is provided, find the existing subfactor
                        var existingProduct = await _context.ProductFactors
                                                            .FirstOrDefaultAsync(p => p.FactorID == request.factorId.Value &&
                                                                                      p.SubFactorID == request.subFactorId &&
                                                                                      p.Id == request.productId && !p.IsRemoved);
                        var subFactorExists = await _context.SubFactors
                                    .AnyAsync(sf => sf.Id == request.subFactorId && sf.FactorID == request.factorId.Value && !sf.IsRemoved );


                 
                        if (!subFactorExists)
                        {
                            return new ResultDto<long, long>
                            {
                                Data=0,
                                IsSuccess = false,
                                Message = "زیرفاکتور نامعتبر است"
                            };
                        }


                        if (existingProduct != null)
                        {
                            // Update the existing product
                            existingProduct.Name = request.productName;
                            /*existingProduct.count = 1;*/  // Use count from request if available
                            existingProduct.fee = Price;
                            existingProduct.price = Price;  // Update the price based on the count
                            existingProduct.priceA2plus = request.productCost.Price_A2plus;
                            existingProduct.priceAplus = request.productCost.Price_Aplus;
                            existingProduct.priceA = request.productCost.Price_A;
                            existingProduct.priceB = request.productCost.Price_B;
                            existingProduct.UpdateTime = DateTime.UtcNow;
                            existingProduct.ProductDetails = request.ProductDetails;
                            existingProduct.NestingResult = request.NestingResult;
                            existingProduct.PurchaseFee = Price / 2;

                            _context.ProductFactors.Update(existingProduct);
                            _context.MainFactors.Update(productFactor);

                            await _context.SaveChangesAsync();

                            await UpdateSubFactorAmount(existingProduct.SubFactorID, request.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus);
                            await transaction.CommitAsync();

                            return new ResultDto<long, long>
                            {
                                SupplemantaryData = existingProduct.Id,
                                Data =existingProduct.SubFactorID,
                                IsSuccess = true,
                                Message = "محصول موجود با موفقیت ویرایش شد"
                            };
                        }
                        else
                        {
                            //Console.WriteLine($@"?????????????????????????????????price={Price}");
                            // Create a new product factor linked to the existing subfactor
                            var newProduct = new ProductFactor
                            {
                                FactorID = request.factorId.Value,
                                SubFactorID = request.subFactorId.Value,
                                Name = request.productName,  // Example: Use productId to generate name
                                fee = Price,
                                price = Price,
                                ProductDetails = request.ProductDetails,
                                NestingResult = request.NestingResult,
                                IsAccessory=request.IsAccessory,
                                priceA2plus = request.productCost.Price_A2plus,
                                priceAplus = request.productCost.Price_Aplus,
                                priceA = request.productCost.Price_A,
                                priceB = request.productCost.Price_B,
                                PurchaseFee= Price/2,
                            };
                            await _context.ProductFactors.AddAsync(newProduct);
                            _context.MainFactors.Update(productFactor);
                            await _context.SaveChangesAsync();

                            await UpdateSubFactorAmount(newProduct.SubFactorID,request.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus);
                            
                            await transaction.CommitAsync();

                            return new ResultDto<long, long>
                            {
                                Data= newProduct.SubFactorID,
                                IsSuccess = true,
                                Message = "محصول جدید به زیرفاکتور موجود اضافه شد"
                            };
                        }
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new ResultDto<long, long>
                    {
                        Data=0,
                        IsSuccess = false,
                        Message = "An error occurred while processing the request."
                    };
                }
            }
        }




        public async Task<ResultDto> ChangeQualityFactor(UpdateQualityFactorRequest request)
        {
            // Update the prices of products based on discount before calculating the amount
            var products = await _context.ProductFactors
                                         .Where(p => p.SubFactorID == request.subFactorId && !p.IsRemoved )
                                         .ToListAsync();


            foreach (var product in products)
            {
                if(!product.IsAccessory && !product.IsUndefinedProduct&& !product.IsService) 
                {
                    product.fee = (request.QualityFactor == ConstantMaterialName.QualityFactor_A2plus ? product.priceA2plus :
                        request.QualityFactor == ConstantMaterialName.QualityFactor_Aplus ? product.priceAplus :
                                 request.QualityFactor == ConstantMaterialName.QualityFactor_A ? product.priceA :
                                 request.QualityFactor == ConstantMaterialName.QualityFactor_B ? product.priceB : product.fee);
                    product.PurchaseFee=product.fee/2;

                }
                
                product.price = product.fee * product.count * (1 - product.Discount * 0.01f);
                
            }
            //using (var transaction = await _context.BeginTransactionAsync())
            //{
            //    try
            //    {
                    // Save the updated product prices
                    _context.ProductFactors.UpdateRange(products);
                    await _context.SaveChangesAsync();

                    // Calculate the total amount for the subfactor based on updated prices
                    var totalAmount = products.Sum(p => p.price);

                    // Update the SubFactor's amount
                    var subFactor = await _context.SubFactors.FirstOrDefaultAsync(s => s.Id == request.subFactorId);
                    if (subFactor != null)
                    {
                        subFactor.Amount = totalAmount;
                        subFactor.Description = products.Any()
                        ? string.Join("-", products.Select(p => p.Name).Distinct())
                        : string.Empty;
                        subFactor.QualityFactor = request.QualityFactor;

                        _context.SubFactors.Update(subFactor);
                        await _context.SaveChangesAsync();
                        await _updatePrice.UpdateFactorPricesAsync(products[0].FactorID);
                        return new ResultDto
                        {
                            IsSuccess = true,
                            Message = "ثبت موفق"
                        };
                    }
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "زیرفاکتور وجود ندارد"
                };

            //}
            //    catch
            //    {
            //    await transaction.RollbackAsync();
            //    return new ResultDto
            //    {
            //        IsSuccess = false,
            //        Message = "An error occurred while processing the request."
            //    };
            //}
        //}
        }

        private async Task UpdateSubFactorAmount(long subFactorId,string QualityFactor)
        {
            // Update the prices of products based on discount before calculating the amount
            var products = await _context.ProductFactors
                                         .Where(p => p.SubFactorID == subFactorId && !p.IsRemoved)
                                         .ToListAsync();


            foreach (var product in products)
              {
                if (!product.IsAccessory && !product.IsUndefinedProduct&& !product.IsService) 
                {
                    product.fee = 

                        (

                        QualityFactor == ConstantMaterialName.QualityFactor_A2plus ? product.priceA2plus :
                        QualityFactor == ConstantMaterialName.QualityFactor_Aplus ? product.priceAplus :
                                 QualityFactor == ConstantMaterialName.QualityFactor_A ? product.priceA :
                                 QualityFactor == ConstantMaterialName.QualityFactor_B ? product.priceB : product.fee);
                }
                
                product.price = product.fee * product.count * (1 - product.Discount*0.01f);
            }

            // Save the updated product prices
            _context.ProductFactors.UpdateRange(products);
            await _context.SaveChangesAsync();

            // Calculate the total amount for the subfactor based on updated prices
            var totalAmount = products.Sum(p => p.price);

            // Update the SubFactor's amount
            var subFactor = await _context.SubFactors.FirstOrDefaultAsync(s => s.Id == subFactorId);
            if (subFactor != null)
            {
                subFactor.Amount = totalAmount;
                subFactor.Description = products.Any()
            ? string.Join("-", products.Select(p => p.Name).Distinct())
            : string.Empty;
                subFactor.QualityFactor = QualityFactor;

                _context.SubFactors.Update(subFactor);
                await _context.SaveChangesAsync();
                await _updatePrice.UpdateFactorPricesAsync(products[0].FactorID);

            }
        }



        

    }


}
