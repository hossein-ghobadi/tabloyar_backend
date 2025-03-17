using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Contents.Commands.ContentSet
{
    public interface IContentSetService
    {
        ResultDto<ResultContentSetDto> Execute(RequestContentSetDto request);
    }


    public class ContentSetService: IContentSetService
    {

        private readonly IDataBaseContext _context;

        public ContentSetService(IDataBaseContext context)
        {
            _context = context;


        }
        public ResultDto<ResultContentSetDto> Execute(RequestContentSetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {

                int id = 0;
                var TitleDup = _context.Contents.FirstOrDefault(c => c.ContentTitle == request.ContentTitle);
                var NameDup = _context.Contents.FirstOrDefault(c => c.ContentUniqeName == request.ContentUniqeName);

                if (string.IsNullOrWhiteSpace(request.ContentTitle))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عنوان محتوی را وارد نمایید"
                    });
                }
                if (TitleDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این عنوان محتوی قبلا ثبت شده است"
                    });
                }

                // if(request.ContentTitle.Length > 60 || request.ContentTitle.Length<35) 
                // {
                //     id = id + 1;
                //     Errors.Add(new IdLabelDto
                //     {
                //         id = id,
                //         label = "!طول متن عنوان محتوی باید بین 35 الی 60 کاراکتر باشد"
                //     });
                // }

                if (string.IsNullOrWhiteSpace(request.ContentUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!نام یکتا را وارد نمایید"
                    });
                }
                if (NameDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این نام یکتای محتوی قبلا ثبت شده است"
                    });
                }
                if (request.ContentUniqeName.Length > 75 || request.ContentUniqeName.Length < 3)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    { 
                        id = id,
                        label = "!طول متن نام یکتا محتوی باید بین 3 الی 75 کاراکتر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.ContentSorting.ToString()))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (request.ContentSorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }

                if (request.ContentSorting is int & request.ContentSorting < 1)
                {

                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.ContentLongDescription))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن محتوی را وارد نمایید"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.ContentMetaDesc))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن توضیحات متا را وارد نمایید"
                    });
                }

                // if (request.ContentMetaDesc.Length > 170 || request.ContentMetaDesc.Length < 140)
                // {
                //     id = id + 1;
                //     Errors.Add(new IdLabelDto
                //     {
                //         id = id,
                //         label = "!طول متن توضیحات متا محتوی باید بین 140 الی 170 کاراکتر باشد"
                //     });
                // }
                if (string.IsNullOrWhiteSpace(request.CategoryUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی را انتخاب نمایید"
                    });
                }

                var categoryinfo = _context.Categories.FirstOrDefault(c => c.CategoryUniqeName == request.CategoryUniqeName);
                if (categoryinfo == null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی با این نام وجود ندارد"
                    });
                }
                if (Errors.Count() < 1)
            {

                Content content = new Content()
                {
                    ContentTitle = request.ContentTitle,
                    ContentUniqeName = request.ContentUniqeName,
                    CommentSituation = request.CommentSituation,
                    CommentShow = request.CommentShow,
                    ContentSorting = request.ContentSorting,
                    ContentLongDescription = request.ContentLongDescription,
                    ContentMetaDesc = request.ContentMetaDesc,
                    ContentImageAlt = request.ContentImageAlt,
                    ContentPublish = request.ContentPublish,
                    ContentImage = request.ContentImage,
                    CategoryUniqeName = categoryinfo.CategoryUniqeName,
                    CategoryTitle = categoryinfo.CategoryTitle,
                    Canonical= request.Canonical,
                    IsIndex = request.IsIndex,
                    IsRemoved=true,
                    ContentImageTitle = request.ContentImageTitle,
                };
                content.UpdateTime = content.InsertTime;

                _context.Contents.Add(content);
                _context.SaveChanges();

                return new ResultDto<ResultContentSetDto>()
                {
                    Data = new ResultContentSetDto()
                    {
                        ContentId = content.Id,
                        Errors = Errors,
                    },
                    IsSuccess = true,
                    Message = " محتوی جدید با موفقیت درج شد",
                };
            }

                else
                {
                    return new ResultDto<ResultContentSetDto>()
                    {
                        Data = new ResultContentSetDto()
                        {
                            ContentId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!محتوی جدید درج نشد"
                    };

                }

            }

            catch (Exception)
            {
                return new ResultDto<ResultContentSetDto>()
                {
                    Data = new ResultContentSetDto()
                    {
                        ContentId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!کتچش شد برنامه"
                };

            }
            }
    }


    public class RequestContentSetDto
    {
        public string ContentTitle { get; set; }
        public string ContentUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int ContentSorting { get; set; }
        public string ContentLongDescription { get; set; }
        public string ContentMetaDesc { get; set; }
        public string ContentImageAlt { get; set; }
        public string? ContentImageTitle { get; set; }
        public bool ContentPublish { get; set; }
        public string ContentImage {  get; set; }
        public string CategoryUniqeName { get; set; }
        public string CategoryTitle { get; set; }
        public string Canonical { get; set; }
        public bool IsIndex { get; set; }

    }
    public class ResultContentSetDto
    {
        public long ContentId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
