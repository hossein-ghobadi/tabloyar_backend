using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Common.Dto
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }


    public class ResultDto<T1,T2>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T1 Data { get; set; }
        public T2 SupplemantaryData { get; set; }

    }
}
