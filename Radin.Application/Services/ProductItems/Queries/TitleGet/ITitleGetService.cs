using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.ProductItems.Queries.TitleGet
{
    public interface ITitleGetService
    {
        ResultDto<List<GetTitleDto>> ExistTitles();
        ResultDto<List<GetTitleDto>> NotExistTitles();

    }


    public class TitleGetService : ITitleGetService
    {
        private readonly IPriceFeeDataBaseContext _context;
        public TitleGetService(IPriceFeeDataBaseContext Context)
        {
            _context = Context;

        }

        public ResultDto<List<GetTitleDto>> ExistTitles()
        {
            var titles = _context.Titles;
            int[] validTitles = { 6,7,9,10 };
            //int rowsCount = 0;
            var titlesList = titles.Where(f=> validTitles.Contains(f.Id)).Select(p => new GetTitleDto
            {
                id = p.Id,
                label = p.TitleName,

                isDefault = (bool)p.IsDefault,


            }).ToList();
            return new ResultDto<List<GetTitleDto>>
            {
                Data = titlesList,
                IsSuccess = true,
                Message = "",

            };

        }




        public ResultDto<List<GetTitleDto>> NotExistTitles()
        {
            var titles = _context.Titles;
            int[] validTitles = { 6, 7, 9, 10 };
            //int rowsCount = 0;
            var titlesList = titles.Where(f => !validTitles.Contains(f.Id)).Select(p => new GetTitleDto
            {
                id = p.Id,
                label = p.TitleName,

                isDefault = (bool)p.IsDefault,

            }).ToList();
            if (titlesList.FirstOrDefault(p => p.isDefault == true) != null)
            {
                titlesList[0].isDefault = true;

            }
            return new ResultDto<List<GetTitleDto>>
            {
                Data = titlesList,
                IsSuccess = true,
                Message = "",

            };

        }

    }


    public class GetTitleDto
    {
        public long id { get; set; }
        public string label { get; set; }
        public bool isDefault { get; set; }


    }
}
