//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Branch.Queries.BranchInfoGetService;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.GoesArea.Queries.StateGetService
//{
//    public interface IStateGetService
//    {
//        ResultDto<List<GetStateCodeDto>> Execute();
//    }

//    public class StateGetService : IStateGetService
//    {
//        private readonly IDataBaseContext _context;
//        public StateGetService(IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<List<GetStateCodeDto>> Execute()
//        {
//            var city = _context.Cities;

//            //int rowsCount = 0;
//            var stateList = city.GroupBy(p => new {p.ProvinceId, p.province}).Select(g => new GetStateCodeDto
//            {
//                Id = g.Key.ProvinceId,
//                label = g.Key.province,
//            }).ToList();
//            return new ResultDto<List<GetStateCodeDto>>
//            {
//                Data = stateList,
//                IsSuccess = true,
//                Message = "",

//            };

//        }


//    }

//    public class GetStateCodeDto
//    {
//        public int Id { get; set; }
//        public string label { get; set; }


//    }
//}
