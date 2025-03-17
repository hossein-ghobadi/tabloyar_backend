using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.ProductItems.Commands.EdgeSizeSet;
using Radin.Application.Services.ProductItems.Queries.ChannelliumGet;

using Radin.Application.Services.ProductItems.Queries.PlasticGet;
using Radin.Application.Services.ProductItems.Queries.SwediMaxGet;
using Radin.Application.Services.ProductItems.Queries.TitleGet;
using Radin.Domain.Entities.Contents;

namespace Endpoint.Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemsGetController : ControllerBase
    {

        private readonly IProductItemsFacad _productItemsFacad;
        private readonly IPriceFeeDataBaseContext _context;
        public ProductItemsGetController(
        IProductItemsFacad productItemsFacad,
        IPriceFeeDataBaseContext context
        
        )


        {
            _productItemsFacad = productItemsFacad;
            _context= context;
        }



        [HttpGet]
        [Route("Channellium")]

        public IActionResult Channellium(int id,string type)
        {

            try
            {
                var Count = _context.Titles.Count();

                //if (!(id > 0 && id < Count))
                //{
                //    return BadRequest("چنین دسته ای وجود ندارد");
                //}
                string name = _context.Titles.Where(p => p.Id == id).Select(p => p.TitleName).First();
                //int id = 7;

                switch (id)
                {
                    case 7: // چنلیوم
                        return Ok(_productItemsFacad.ChannelliumGet.Execute(name));
                    case 6: // پلاستیک
                        return Ok(_productItemsFacad.PlasticGetService.Execute(name));
                    case 10: // سوئدی
                        return Ok(_productItemsFacad.ChannelliumGet.Execute(name));
                    case 9: // سوئدی
                        return Ok(_productItemsFacad.SwediMaxGetService.Execute(name));
                    default:
                        return BadRequest("چنین موردی وجود ندارد");

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, "خطا");
            }
        }

        /// ////////////////////////////////

        [HttpGet]
        [Route("GetTitle")]

        public IActionResult GetTitle()
        {
            return Ok(_productItemsFacad.TitleGetService.ExistTitles());
        }
        // GET: api/<ProductItemsController>
        [HttpGet]
        [Route("OtherTitles")]

        public IActionResult OtherTitles()
        {
            return Ok(_productItemsFacad.TitleGetService.NotExistTitles());
        }



    }
}
