using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Domain.Entities.Factors;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Commands.UpdatePrice
{
    public interface IUpdatePrice
    {
        Task UpdateFactorPricesAsync(long factorId);
    }

    public class UpdatePrice : IUpdatePrice
    {
        private readonly IDataBaseContext _context;

        public UpdatePrice(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task UpdateFactorPricesAsync(long factorId)
        {
            // Retrieve the MainFactor with related non-removed SubFactors and their ProductFactors
            var mainFactor = await _context.MainFactors
                .Include(mf => mf.SubFactors.Where(sf => !sf.IsRemoved)) // Include only non-removed SubFactors
                    .ThenInclude(sf => sf.ProductFactors)
                .FirstOrDefaultAsync(mf => mf.Id == factorId);

            if (mainFactor == null)
            {
                throw new Exception($"MainFactor with ID {factorId} not found.");
            }

            bool hasTrueStatusSubFactor = false;
            float maxSubFactorAmount = 0;

            foreach (var subFactor in mainFactor.SubFactors)
            {
                // Calculate the total amount for this SubFactor based on non-removed ProductFactors
                
                var nonRemovedProductFactors = subFactor.ProductFactors?
                    .Where(pf => !pf.IsRemoved)
                    .ToList() ?? new List<ProductFactor>();
                // If there are no non-removed ProductFactors, set the amount to zero
                float subFactorAmount = nonRemovedProductFactors.Any()
                    ? nonRemovedProductFactors.Sum(pf => pf.price)
                    : 0;

                // Set the calculated amount to the SubFactor
                subFactor.Amount = subFactorAmount;

                // Check if this SubFactor has a true status
                if (subFactor.status)
                {
                    // Set the mainFactor's fee based on this SubFactor's amount if its status is true
                    mainFactor.fee = subFactor.Amount;
                    hasTrueStatusSubFactor = true;
                }

                // Track the maximum SubFactor amount for fallback if no SubFactor has a true status
                if (subFactor.Amount > maxSubFactorAmount)
                {
                    maxSubFactorAmount = subFactor.Amount;
                }

                // Use custom method to mark the SubFactor's Amount property as modified
                _context.MarkAsModified(subFactor);
            }

            // If no SubFactors have a true status, use the maximum amount of the SubFactors
            if (!hasTrueStatusSubFactor)
            {
                mainFactor.fee = maxSubFactorAmount;
            }

            // If the MainFactor has no SubFactors at all, set the fee to zero
            if (!mainFactor.SubFactors.Any())
            {
                mainFactor.fee = 0;
            }

            // Consider packing cost and discount when setting the final amount
            mainFactor.TotalAmount = mainFactor.fee * mainFactor.count * (1-(mainFactor.TotalDiscount * 0.01f ?? 0))
                                     + (mainFactor.TotalPackingCost ?? 0);
                                     

            // Ensure the TotalAmount is not negative
            if (mainFactor.TotalAmount < 0)
            {
                mainFactor.TotalAmount = 0;
            }

            // Use custom method to mark the MainFactor's fee and TotalAmount properties as modified
            _context.MarkPropertyAsModified(mainFactor, mf => mf.fee);
            _context.MarkPropertyAsModified(mainFactor, mf => mf.TotalAmount);

            // Save all changes to the database
            await _context.SaveChangesAsync();
        }
    }
}
