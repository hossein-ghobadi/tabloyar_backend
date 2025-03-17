using Radin.Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Queries.CategoryGet
{
    public interface ICategoryGetForContentService
    {
        List<GetDto> Execute();
        List<GetDto> InPublic();

    }

    public class CategoryGetForContentService : ICategoryGetForContentService
    {
        private readonly IDataBaseContext _context;
        public CategoryGetForContentService(IDataBaseContext Context)
        {
            _context = Context;

        }

        public List<GetDto> Execute()
        {
            var categories = _context.Categories;

            //int rowsCount = 0;
            var categoriesList = categories.Select(p => new GetDto
            {
                id = p.CategoryUniqeName,
                label = p.CategoryTitle,



            }).ToList();

            return categoriesList;
        }

        public List<GetDto> InPublic()
        {
            var categories = _context.Categories;

            //int rowsCount = 0;
            var categoriesList = categories.Where(p => !p.IsRemoved).Select(p => new GetDto
            {
                id = p.CategoryUniqeName,
                label = p.CategoryTitle,



            }).ToList();

            return categoriesList;
        }


    }


    public class GetDto
    {
        public string id { get; set; }
        public string label { get; set; }


    }
}
