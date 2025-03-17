using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentEdit;
using Radin.Common.Dto;
using Radin.Domain.Entities.Ideas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Commands.IIdeaEdit
{
    public interface IIdeaEditService
    {
        ResultDto<ResultEditIdeaDto> Execute(UpdateIdeaDto request);

    }



    public class IdeaEditService : IIdeaEditService
    {
        private readonly IDataBaseContext _context;

        public IdeaEditService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultEditIdeaDto> Execute(UpdateIdeaDto updateDto)
        {
            var Errors = new List<IdLabelDto>();
            int id = 0;
            try
            {
                var idea = _context.Ideas.FirstOrDefault(c => c.Id == updateDto.Id);
                if (idea == null)
                {
                    return new ResultDto<ResultEditIdeaDto>()
                    {
                        Data = new ResultEditIdeaDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!محتوی یافت نشد"
                    };
                }

                if (string.IsNullOrWhiteSpace(updateDto.IdeaTitle))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عنوان ایده را وارد نمایید"
                    });
                }
                //if (updateDto.IdeaTitle.Length > 60 || updateDto.IdeaTitle.Length < 35)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!طول متن عنوان ایده باید بین 35 الی 60 کاراکتر باشد"
                //    });
                //}
                if (string.IsNullOrWhiteSpace(updateDto.IdeaUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام یکتا را وارد نمایید"
                    });
                }

                if (updateDto.IdeaUniqeName.Length > 75 || updateDto.IdeaUniqeName.Length < 3)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!طول متن نام یکتای ایده باید بین 3 الی 75 کاراکتر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(updateDto.IdeaSorting.ToString()))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (updateDto.IdeaSorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }
                if (updateDto.IdeaSorting is int & updateDto.IdeaSorting < 1)
                {

                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
                    });
                }
                if (string.IsNullOrWhiteSpace(updateDto.IdeaLongDescription))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن محتوی را وارد نمایید"
                    });
                }
                if (string.IsNullOrWhiteSpace(updateDto.IdeaMetaDesc))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن توضیحات متا را وارد نمایید"
                    });
                }

                //if (updateDto.IdeaMetaDesc.Length > 170 || updateDto.IdeaMetaDesc.Length < 140)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!طول متن توضیحات متا  باید بین 140 الی 170 کاراکتر باشد"
                //    });
                //}
                if (string.IsNullOrWhiteSpace(updateDto.IdeaCategoryUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی را انتخاب نمایید"
                    });
                }
                var categoryinfo = _context.IdeaCategories.FirstOrDefault(c => c.IdeaCategoryUniqeName == updateDto.IdeaCategoryUniqeName);
                if (categoryinfo == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی با این نام وجود ندارد"
                    });
                }
                //UpdateIdeaDto
                var ideasTab = _context.Ideas.ToList();
                var OtherIdea = ideasTab.Remove(idea);
                if (OtherIdea)
                {
                    var NameDup = ideasTab.FirstOrDefault(p => p.IdeaUniqeName == updateDto.IdeaUniqeName);
                    var TitleDup = ideasTab.FirstOrDefault(c => c.IdeaTitle == updateDto.IdeaTitle);
                    if (TitleDup != null)
                    {
                        id = id + 1;
                        Errors.Add(new IdLabelDto
                        {
                            id = id,
                            label = "!این عنوان ایده قبلا ثبت شده است"
                        });
                    }
                    if (NameDup != null)
                    {
                        id = id + 1;
                        Errors.Add(new IdLabelDto
                        {
                            id = id,
                            label = "!این نام یکتای ایده قبلا ثبت شده است"
                        });
                    }
                }

                string imageUrlsString = string.Join(",", updateDto.IdeaImages.Where(img => !string.IsNullOrWhiteSpace(img)));

                if (Errors.Count() < 1)
                {

                    idea.IdeaTitle = updateDto.IdeaTitle;
                    idea.IdeaUniqeName = updateDto.IdeaUniqeName;
                    idea.CommentSituation = updateDto.CommentSituation;
                    idea.CommentShow = updateDto.CommentShow;
                    idea.IdeaSorting = updateDto.IdeaSorting;
                    idea.IdeaLongDescription = updateDto.IdeaLongDescription;
                    idea.IdeaMetaDesc = updateDto.IdeaMetaDesc;
                    idea.IdeaImageAlt = updateDto.IdeaImageAlt;
                    idea.IdeaPublish = updateDto.IdeaPublish;
                    idea.MainImage = updateDto.MainImage;
                    idea.IdeaImage = imageUrlsString;
                    idea.IdeaUniqeName = updateDto.IdeaUniqeName;
                    idea.IdeaCategoryTitle = categoryinfo.IdeaCategoryTitle;
                    idea.IsIndex = updateDto.IsIndex;
                    _context.SaveChanges();

                    

                    return new ResultDto<ResultEditIdeaDto>()
                    {
                        Data = new ResultEditIdeaDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = "!ویرایش محتوی با موفقیت انجام شد"
                    };
                }
                else
                {
                    return new ResultDto<ResultEditIdeaDto>()
                    {
                        Data = new ResultEditIdeaDto()
                        {
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!ویرایش دسته بندی محتوی انجام نشد"
                    };
                }

            }
            catch
            {
                return new ResultDto<ResultEditIdeaDto>()
                {
                    Data = new ResultEditIdeaDto()
                    {
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!ویرایش دسته بندی محتوی انجام نشد"
                };
            }
        }
    }


    public class UpdateIdeaDto
    {
        public long Id { get; set; }


        public string IdeaTitle { get; set; }
        public string IdeaUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int IdeaSorting { get; set; }
        public string IdeaLongDescription { get; set; }
        public string IdeaMetaDesc { get; set; }
        public string IdeaImageAlt { get; set; }
        public bool IdeaPublish { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public string IdeaCategoryTitle { get; set; }
        public bool IsIndex { get; set; }
        public string MainImage {  get; set; }
        public List<string> IdeaImages { get; set; }
        

    }

    public class ResultEditIdeaDto
    {
        public List<IdLabelDto> Errors { get; set; }
    }
}
