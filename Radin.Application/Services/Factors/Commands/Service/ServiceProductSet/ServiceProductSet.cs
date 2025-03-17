using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Factors.Commands.UpdatePrice;
using Radin.Common;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using System.Threading.Tasks;
using NewtonsoftJson = Newtonsoft.Json;

namespace Radin.Application.Services.Factors.Commands.Service.ServiceProductSet
{
    public class ServiceProductSet:IServiceProductSet
    {
        private readonly IDataBaseContext _context;
        private readonly IUpdatePrice _updatePrice;

        public ServiceProductSet(IDataBaseContext context, IUpdatePrice updatePrice)
        {
            _context = context;
            _updatePrice = updatePrice;
        }


        public async Task<ResultDto<long>> ProductSet(ServiceProductRequestDto request)
        {

            if (request.factorId == 0)
            {
                return new ResultDto<long>
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
                return new ResultDto<long>
                {
                    Data = 0,
                    IsSuccess = false,
                    Message = "فاکتوری با شماره مورد نظر وجود ندارد"
                };
            }


            //if(string.IsNullOrEmpty(request.Image))
            //{
            //    return new ResultDto<long>
            //    {
            //        Data = 0,
            //        IsSuccess = false,
            //        Message = "فایل کار را بارگذاری کنید"
            //    };
            //}

            var state = productFactor.state;

            // Price calculation for new or existing subfactor
            using (var transaction = await _context.BeginTransactionAsync())
            {
                try
                {
                    
                    string image = "";
                    //image = request.Image;
                    if (request.Image != null) { image = string.Join(",", request.Image.Where(img => !string.IsNullOrWhiteSpace(img))); }

                    var model = new DescImage { Description = request.Description, Image = image, Id = request.Id };
                    string ProductDetails = NewtonsoftJson.JsonConvert.SerializeObject(model);
                    if (state == 0)
                    {
                        productFactor.state = 1;
                    }
                    // Check if SubFactorId is null (create a new subfactor)
                    if (request.subFactorId == 0)
                    {
                        if (request.productId > 0)
                        {
                            return new ResultDto<long>
                            {
                                Data = 0,
                                IsSuccess = false,
                                Message = "خدمتی وجود ندارد"
                            };
                        }

                        // Create a new subfactor
                        var newSubFactor = new SubFactor
                        {
                            FactorID = request.factorId.Value,
                            Amount = 0,
                            QualityFactor = request.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus,
                        };

                        await _context.SubFactors.AddAsync(newSubFactor);
                        await _context.SaveChangesAsync();
                        _context.MainFactors.Update(productFactor);
                        // Create a new product factor associated with the new subfactor
                        var newProduct = new ProductFactor
                        {
                            FactorID = request.factorId.Value,
                            SubFactorID = newSubFactor.Id,
                            Name = request.label,  // Example: Use productId to generate name
                            fee = request.fee,
                            price = request.fee,
                            priceAplus = request.fee,
                            priceA = request.fee,
                            priceB = request.fee,
                            count = request.count.Value,
                            Discount = request.Discount,
                            IsService = true,
                            ProductDetails = ProductDetails,
                            ServiceCode=Convert.ToInt32(request.Id)
                            
                        };

                        await _context.ProductFactors.AddAsync(newProduct);
                        await _context.SaveChangesAsync();
                        await UpdateSubFactorAmount(newSubFactor.Id);

                        await transaction.CommitAsync();

                        return new ResultDto<long>
                        {
                            Data = newSubFactor.Id,
                            IsSuccess = true,
                            Message = "فاکتور جدید با زیر فاکتور و محصول درج شد"
                        };
                    }
                    else
                    {
                        // SubFactorId is provided, find the existing subfactor
                        var existingProduct = await _context.ProductFactors
                                                            .FirstOrDefaultAsync(p => p.FactorID == request.factorId &&
                                                                                      p.SubFactorID == request.subFactorId &&
                                                                                      p.Id == request.productId && !p.IsRemoved);
                        var subFactorExists = await _context.SubFactors
                                    .AnyAsync(sf => sf.Id == request.subFactorId && sf.FactorID == request.factorId && !sf.IsRemoved);



                        if (!subFactorExists)
                        {
                            return new ResultDto<long>
                            {
                                Data = 0,
                                IsSuccess = false,
                                Message = "زیرفاکتور نامعتبر است"
                            };
                        }


                        if (existingProduct != null)
                        {
                            // Update the existing product
                            existingProduct.Name = request.label;
                            /*existingProduct.count = 1;*/  // Use count from request if available
                            existingProduct.fee = request.fee;
                            existingProduct.price = request.fee;  // Update the price based on the count
                            existingProduct.priceAplus = request.fee;
                            existingProduct.priceA = request.fee;
                            existingProduct.priceB = request.fee;
                            existingProduct.UpdateTime = DateTime.UtcNow;
                            existingProduct.count = request.count.Value;
                            existingProduct.Discount = request.Discount;
                            existingProduct.IsService = true;
                            existingProduct.ProductDetails = ProductDetails;
                            existingProduct.ServiceCode = Convert.ToInt32(request.Id);
                            _context.ProductFactors.Update(existingProduct);
                            _context.MainFactors.Update(productFactor);

                            await _context.SaveChangesAsync();

                            await UpdateSubFactorAmount(existingProduct.SubFactorID);
                            await transaction.CommitAsync();

                            return new ResultDto<long>
                            {
                                Data = existingProduct.SubFactorID,
                                IsSuccess = true,
                                Message = "خدمت موجود با موفقیت ویرایش شد"
                            };
                        }
                        else
                        {
                            // Create a new product factor linked to the existing subfactor
                            var newProduct = new ProductFactor
                            {
                                FactorID = request.factorId.Value,
                                SubFactorID = request.subFactorId.Value,
                                Name = request.label,  // Example: Use productId to generate name
                                fee = request.fee,
                                price = request.fee,
                                priceAplus = request.fee,
                                priceA = request.fee,
                                priceB = request.fee,
                                count = request.count.Value,
                                Discount = request.Discount,
                                ProductDetails = ProductDetails,
                                IsService = true,
                                ServiceCode= Convert.ToInt32(request.Id)
                            };
                            await _context.ProductFactors.AddAsync(newProduct);
                            _context.MainFactors.Update(productFactor);
                            await _context.SaveChangesAsync();

                            await UpdateSubFactorAmount(newProduct.SubFactorID);

                            await transaction.CommitAsync();

                            return new ResultDto<long>
                            {
                                Data = newProduct.SubFactorID,
                                IsSuccess = true,
                                Message = "خدمت جدید به زیرفاکتور موجود اضافه شد"
                            };
                        }
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new ResultDto<long>
                    {
                        Data = 0,
                        IsSuccess = false,
                        Message = "An error occurred while processing the request."
                    };
                }
            }
        }

        private async Task UpdateSubFactorAmount(long subFactorId)
        {
            // Update the prices of products based on discount before calculating the amount
            var products = await _context.ProductFactors
                                         .Where(p => p.SubFactorID == subFactorId && !p.IsRemoved)
                                         .ToListAsync();



            foreach (var product in products)
            {
                if (product.IsService)
                {
                    product.price = product.fee * product.count * (1 - product.Discount * 0.01f);

                }
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
                _context.SubFactors.Update(subFactor);
                await _context.SaveChangesAsync();
                await _updatePrice.UpdateFactorPricesAsync(products[0].FactorID);
            }
        }


    }
}

