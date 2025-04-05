//using Radin.Application.Interfaces.Contexts;
//using Radin.Common;
//using Radin.Domain.Entities.Products;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Product.Commands.PowerCalculation
//{
//    public interface IPowerCalculationService
//    {
//        CalculationResult CalculateTotalCost(float fsmd,float Bsmd, string QualityFactor);
//        float selfChoosenTotalCost(Dictionary<int,int> ChooosedPowers, string QualityFactor);

//    }

//    public class PowerCalculationService : IPowerCalculationService
//    {
//        private readonly IPriceFeeDataBaseContext _context;

//        public PowerCalculationService(IPriceFeeDataBaseContext context)
//        {
//            _context = context;
//        }

//        public CalculationResult CalculateTotalCost(float fsmd, float Bsmd, string QualityFactor)
//        {
//            var totalSMDs = (int)(fsmd + Bsmd);
//            var powerTypes = _context.Powers.Where(p => p.QualityFactor == QualityFactor).OrderByDescending(pt => pt.PowerType).ToList();
//            CalculationResult result = new CalculationResult();
//            result.Fsmd = fsmd;
//            result.Bsmd = Bsmd;
//            int remainingSMDs = totalSMDs;
//            var largestPowerType = powerTypes.First();
//            int Index = 0;
//            Console.WriteLine("................................................................................");

//            foreach (var powerType in powerTypes)
//            {

//                Console.WriteLine($@"PowerType={powerType.PowerType}");

//                if (remainingSMDs <= 0) break;
//                int periviousIndex = Index - 1;
//                int quantityNeeded = remainingSMDs / powerType.MaxSmd;
//                var Res = remainingSMDs - (quantityNeeded * powerType.MaxSmd);
               



//                if (quantityNeeded > 0 && powerType == powerTypes.First())
//                {


//                    UpdateOrAddPowerTypeCalculation(result, powerType, quantityNeeded);
                   
//                    remainingSMDs -= quantityNeeded * powerType.MaxSmd;
                    

//                }



//                else if (quantityNeeded > 1 && powerType != powerTypes.First())
//                {

//                    UpdateOrAddPowerTypeCalculation(result, powerType, quantityNeeded);

                   
//                    remainingSMDs -= quantityNeeded * powerType.MaxSmd;
                 

//                }
              
//                else if (quantityNeeded == 1 && powerType != powerTypes.First() && Res == 0)
//                {
//                    UpdateOrAddPowerTypeCalculation(result, powerType, 1);

                  
                 
//                    remainingSMDs = 0;//-= quantityNeeded * powerTypes[periviousIndex].MaxSmd;

//                    break;

//                }
//                else if (quantityNeeded == 1 && powerType != powerTypes.First())
//                {
//                    UpdateOrAddPowerTypeCalculation(result, powerTypes[periviousIndex], 1);

                    
//                    remainingSMDs = 0;//-= quantityNeeded * powerTypes[periviousIndex].MaxSmd;
                   
//                    break;

//                }

               
//                else
//                {

//                }
//                Index = Index + 1;
//                Console.WriteLine("////////////////////////////////////////");

//            }

//            // Cover remaining SMDs with the smallest available power type if necessary
//            if (remainingSMDs > 0 && powerTypes.Any())
//            {

//                var smallestPowerType = powerTypes.Last();
//                UpdateOrAddPowerTypeCalculation(result, smallestPowerType, 1);
              
//            }
//            Console.WriteLine("*****************************");

//            return result;
//        }

//        public float selfChoosenTotalCost(Dictionary<int, int> ChooosedPowers, string QualityFactor)
//        {
//            var PowerTempt = _context.Powers
//                                .Where(p => ChooosedPowers.Keys.Contains(p.PowerType) && p.QualityFactor == QualityFactor)
//                                .ToDictionary(p => p.PowerType, p => p.PowerFee);

//            float total = 0;

//            // Calculate total
//            foreach (var pair in ChooosedPowers)
//            {
//                int dataPoint = pair.Key;
//                float number = pair.Value;

//                if (PowerTempt.TryGetValue(dataPoint, out float fee))
//                {

//                    total += fee * number;
                   


//                }
//            }

//            return total;

//        }

//        private void UpdateOrAddPowerTypeCalculation(CalculationResult result, Power powerType, int quantity)
//        {
//            var existingEntry = result.PowerTypeCalculations
//                                      .FirstOrDefault(ptc => ptc.PowerType == powerType.PowerType);

//            if (existingEntry != null)
//            {
//                // If the PowerType is already in the list, update the Quantity and Cost
//                existingEntry.Quantity += quantity;
//                existingEntry.Cost += quantity * powerType.PowerFee;
//            }
//            else
//            {
//                // If the PowerType is not in the list, add a new entry
//                result.PowerTypeCalculations.Add(new PowerTypeCalculation
//                {
//                    PowerType = powerType.PowerType,
//                    Quantity = quantity,
//                    Cost = quantity * powerType.PowerFee
//                });
//            }
//        }





//    }

//    public class PowerTypeCalculation
//    {
//        public int PowerType { get; set; }
//        public int Quantity { get; set; }
//        public float Cost { get; set; }
//    }
//    public class PowerList
//    {
//        public int PowerType { get; set; }
//        public int Quantity { get; set; }
//    }

//    public class CalculationResult
//    {
//        public float Fsmd { get; set; } 
//        public float Bsmd { get; set; }
//        public List<PowerTypeCalculation> PowerTypeCalculations { get; set; } = new List<PowerTypeCalculation>();
//        public float TotalCost => PowerTypeCalculations.Sum(p => p.Cost);
//        public List<PowerList> PowerList => PowerTypeCalculations.Select(c => new PowerList
//        {
//            PowerType = c.PowerType,
//            Quantity = c.Quantity
//        }).ToList();
//    }


   
//}
