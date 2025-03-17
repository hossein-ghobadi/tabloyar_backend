using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Common
{
    public class SortValidation
    {
        public bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is int)
            {
                int getal;
                if (int.TryParse(value.ToString(), out getal))
                {

                    if (getal >= 0)
                        return true;
                }
                return true;
            }

            return false;
        }
    }
}
