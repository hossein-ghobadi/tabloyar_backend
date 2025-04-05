//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Excelloading
//{
//    using System.Globalization;

//    public class ExcelHelper
//    {

//        public static bool TryConvertToFloat(object value, out float result, params string[] formats)
//        {
//            result = 0;
//            if (value == null) return false;

//            string stringValue = value.ToString();

//            // Default culture (InvariantCulture) for the fallback conversion
//            if (float.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
//            {
//                return true;
//            }

//            // Attempt to parse using additional specified formats and cultures
//            foreach (var format in formats)
//            {
//                if (float.TryParse(stringValue, NumberStyles.Any, new CultureInfo(format), out result))
//                {
//                    return true;
//                }
//            }

//            // Attempt to handle common decimal separators manually if predefined cultures fail
//            stringValue = stringValue.Replace("/", ".");
//            stringValue = stringValue.Replace(",", ".");
//            return float.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
//        }

//        public static bool TryConvertToInt(object value, out int result, params string[] cultureCodes)
//        {
//            result = 0;
//            if (value == null) return false;

//            // Attempt to convert directly to int without considering culture-specific formats.
//            // This is because integers don't usually have culture-specific decimal separators.
//            if (int.TryParse(value.ToString(), out result))
//            {
//                return true; // Successful conversion directly to int
//            }

//            // Optionally handle culture-specific formatting for integers (e.g., group separators)
//            foreach (var cultureCode in cultureCodes)
//            {
//                var cultureInfo = new System.Globalization.CultureInfo(cultureCode);
//                if (int.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, cultureInfo, out result))
//                {
//                    return true; // Successful conversion with culture consideration
//                }
//            }

//            return false; // Conversion failed
//        }
//    }
//}
