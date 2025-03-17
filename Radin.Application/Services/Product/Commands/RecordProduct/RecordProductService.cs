using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands.RecordProduct
{
    public class RecordProductService:IRecordProductService
    {

        private readonly IDataBaseContext _context;

        public RecordProductService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<string>> HandleRecording(RecordRequest request)
        {
            // Check if the price calculation was successful
            if (!request.priceIsSuccess)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = request.priceMessage
                };
            }

            // Ensure FactorId is provided
            if (request.factorId<1)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "شماره فاکتور وجود ندارد"
                };
            }

            // Retrieve the product factor along with related subfactors
            var productFactor = await _context.MainFactors
                                              .Include(m => m.SubFactors)
                                              .FirstOrDefaultAsync(p => p.Id == request.factorId);

            if (productFactor == null)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "فاکتوری با شماره مورد نظر وجود ندارد"
                };
            }
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
                    if (request.subFactorId <1)
                    {
                        if (request.productId > 0)
                        {
                            return new ResultDto<string>
                            {
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
                            fee = request.productCost,
                            price = request.productCost,
                            ProductDetails=request.ProductDetails
                        };

                        await _context.ProductFactors.AddAsync(newProduct);
                        await _context.SaveChangesAsync();
                        await UpdateSubFactorAmount(newSubFactor.Id);

                        await transaction.CommitAsync();

                        return new ResultDto<string>
                        {
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
                                                                                      p.Id == request.productId);
                        var subFactorExists = await _context.SubFactors
                                    .AnyAsync(sf => sf.Id == request.subFactorId && sf.FactorID == request.factorId.Value);


                 
                        if (!subFactorExists)
                        {
                            return new ResultDto<string>
                            {
                                IsSuccess = false,
                                Message = "زیرفاکتور نامعتبر است"
                            };
                        }


                        if (existingProduct != null)
                        {
                            // Update the existing product
                            existingProduct.Name = request.productName;
                            /*existingProduct.count = 1;*/  // Use count from request if available
                            existingProduct.fee = request.productCost;
                            existingProduct.price = request.productCost ;  // Update the price based on the count
                            existingProduct.UpdateTime = DateTime.UtcNow;
                            existingProduct.ProductDetails = request.ProductDetails;
                            _context.ProductFactors.Update(existingProduct);
                            _context.MainFactors.Update(productFactor);

                            await _context.SaveChangesAsync();

                            await UpdateSubFactorAmount(existingProduct.SubFactorID);
                            await transaction.CommitAsync();

                            return new ResultDto<string>
                            {
                                IsSuccess = true,
                                Message = "محصول موجود با موفقیت ویرایش شد"
                            };
                        }
                        else
                        {
                            // Create a new product factor linked to the existing subfactor
                            var newProduct = new ProductFactor
                            {
                                FactorID = request.factorId.Value,
                                SubFactorID = request.subFactorId.Value,
                                Name = request.productName,  // Example: Use productId to generate name
                                fee = request.productCost,
                                price = request.productCost,
                                ProductDetails = request.ProductDetails
                            };
                            await _context.ProductFactors.AddAsync(newProduct);
                            _context.MainFactors.Update(productFactor);
                            await _context.SaveChangesAsync();

                            await UpdateSubFactorAmount(newProduct.SubFactorID);

                            await transaction.CommitAsync();

                            return new ResultDto<string>
                            {
                                IsSuccess = true,
                                Message = "محصول جدید به زیرفاکتور موجود اضافه شد"
                            };
                        }
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new ResultDto<string>
                    {
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
                                         .Where(p => p.SubFactorID == subFactorId)
                                         .ToListAsync();

            foreach (var product in products)
            {
                product.price = product.fee * product.count - (product.fee * product.count * product.Discount);
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
            }
        }

    }


}
