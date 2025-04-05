//using Radin.Application.Interfaces.Contexts;
//using Radin.Common.Dto;
//using Radin.Common.StaticClass;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

//namespace Radin.Application.Services.Factors.Queries.ConnectionsGet
//{
//    public interface IConnectionsGetService
//    {
//        ResultDto<List<ConnectionItem>> GetConnectionsList(long factorId);
//        ResultDto<List<IdLabelDto>> GetConactTypeList();

//    }



//    public class ConnectionsGetService : IConnectionsGetService
//    {
//        private readonly IDataBaseContext _context;

//        public ConnectionsGetService(IDataBaseContext context)
//        {
//            _context = context;


//        }
//        public ResultDto<List<ConnectionItem>> GetConnectionsList(long factorId)
//        {
//            try
//            {
//                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

//                var Connections =_context.CustomerConnections.Where(p=>p.FactorID==factorId).Select(t=>new ConnectionItem

//                {

//                    id=t.Id,
//                    ConnectionTime= SimpleMethods.DateTimeToTimeStamp(t.ConnectinTime).ToString(),
//                    ConnectionDuration= TimeSpan.FromMinutes(t.ConnectionDuration),
//                    ContactType=t.ContactTypeName,



//                }
//                ).ToList();


//                return new ResultDto<List<ConnectionItem>>
//                {
//                    Data = Connections,
//                    IsSuccess = true,
//                    Message = "دریافت موفق"
//                };
                
               
                

//            }
//            catch {

//                return new ResultDto<List<ConnectionItem>>
//                {
//                    IsSuccess = false,
//                    Message = "دریافت ناموفق"
//                };

//            }
//        }



//        public ResultDto<List<IdLabelDto>> GetConactTypeList()
//        {

//            try
//            {
//                var ContactTypeList = _context.ContactTypeInfo.Select(p => new IdLabelDto
//                {
//                    id = Convert.ToInt32(p.Id),
//                    label = p.type
//                }
//                    ).ToList();
//                return new ResultDto<List<IdLabelDto>>
//                {
//                    Data = ContactTypeList,
//                    IsSuccess = true,
//                    Message = "دریافت موفقیت آمیز"
//                };
//            }
//            catch {
//                return new ResultDto<List<IdLabelDto>>
//                {
//                    IsSuccess = false,
//                    Message = "دریافت ناموفق"
//                };
//            }

//        }
        

//        }
//    public class ConnectionItem
//    {
//        public long id {  get; set; }
//        public string ConnectionTime { get; set; }

//        public string ContactType {  get; set; }
//        public TimeSpan ConnectionDuration { get; set; }

//    }
//}