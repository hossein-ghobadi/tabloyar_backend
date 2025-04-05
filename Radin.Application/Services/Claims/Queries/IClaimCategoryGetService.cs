//using Radin.Application.Interfaces.Contexts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Claims.Queries.ClaimCategoryGetService
//{
//    public interface IClaimCategoryGetService
//    {
//        List<GetDto> Execute();

//    }

//    public class ClaimCategoryGetService : IClaimCategoryGetService
//    {
//        private readonly IDataBaseContext _context;
//        public ClaimCategoryGetService(IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public List<GetDto> Execute()
//        {
//            var categories = _context.ClaimCategories;

//            //int rowsCount = 0;
//            var categoriesList = categories.Select(p => new GetDto
//            {
//                id = p.Id,
//                label = p.Description,



//            }).ToList();

//            return categoriesList;
//        }


//    }


//    public class GetDto
//    {
//        public long id { get; set; }
//        public string label { get; set; }


//    }
//}
