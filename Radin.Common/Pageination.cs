using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Common
{
    public static class Pageination
    {
        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int pageSize, out int rowsCount)
        {
            rowsCount = source.Count();
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
//var categoriesList = categories.ToPaged(request.Page, request.PageSize, out rowsCount).Select(p => new GetContentCategorySummaryDto
//{

//    CategoryTitle = p.CategoryTitle,
//    id = p.CategoryUniqeName,
//    IsRemoved = p.IsRemoved,

//}).ToList();