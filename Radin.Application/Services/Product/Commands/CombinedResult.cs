using Microsoft.VisualBasic;
using Radin.Application.Services.Product.Commands.PlasticPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Product.Commands
{
    public class CombinedResult
    {
        public List<object> Execute<T1, T2, T3>(T1 Result_Aplus, T2 Result_A, T3 Result_B)
        {
            List<object> List = new List<object>();
            List.Add(Result_Aplus);
            List.Add(Result_A);
            List.Add(Result_B);
            return List;
        }
    }
}
