using Radin.Common.Dto;

namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.Idea
{
    public class IdeaCategorySetViewModel
    {
        public string CategoryTitle { get; set; }
        public string CategoryUniqeName { get; set; }
        public int CategorySorting { get; set; } = 1;
        public string? CategoryStyle { get; set; }

        public bool CategoryIsShowMain { get; set; } = true;
        public bool CategoryIsShowMenu { get; set; } = true;
        public string? CategoryDescription { get; set; }

        public List<IdLabelDto> Validate()
        {
            var validationErrors = new List<IdLabelDto>();
            int id = 0;
            if (CategoryTitle.GetType() != typeof(string))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!عنوان دسته باید به فرمت رشته یا تکست باشد  "
                });
            }
            if (CategoryUniqeName.GetType() != typeof(string))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!نام یکتا دسته باید به فرمت رشته یا تکست باشد  "
                });
            }
            if (CategorySorting.GetType() != typeof(int))
            {
                id = id + 1;
                validationErrors.Add(new IdLabelDto
                {
                    id = id,
                    label = "!عدد مرتب سازی دسته باید به فرمت عدد باشد  "
                });
            }


            // Additional validation for other properties...

            return validationErrors;
        }
    }
}
