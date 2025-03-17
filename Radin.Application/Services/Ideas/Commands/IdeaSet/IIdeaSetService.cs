using OfficeOpenXml;
using Radin.Application.Interfaces.Contexts;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Common.Dto;
using Radin.Domain.Entities.Contents;
using Radin.Domain.Entities.Ideas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.Commands.IdeaSet
{
    public interface IIdeaSetService
    {
        ResultDto<ResultIdeaSetDto> AdminSet(RequestIdeaSetDto request);

    }

    public class IdeaSetService : IIdeaSetService
    {

        private readonly IDataBaseContext _context;

        public IdeaSetService(IDataBaseContext context)
        {
            _context = context;


        }
        public ResultDto<ResultIdeaSetDto> AdminSet(RequestIdeaSetDto request)
        {

            var Errors = new List<IdLabelDto>();
            try
            {

                int id = 0;
                var TitleDup = _context.Ideas.FirstOrDefault(c => c.IdeaTitle == request.IdeaTitle);
                var NameDup = _context.Ideas.FirstOrDefault(c => c.IdeaUniqeName == request.IdeaUniqeName);

                if (string.IsNullOrWhiteSpace(request.IdeaTitle))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عنوان ایده را وارد نمایید"
                    });
                }
                if (TitleDup != null)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!این عنوان ایده قبلا ثبت شده است"
                    });
                }

                //if (request.IdeaTitle.Length > 60 || request.IdeaTitle.Length < 35)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!طول متن عنوان ایده باید بین 35 الی 60 کاراکتر باشد"
                //    });
                //}

                if (string.IsNullOrWhiteSpace(request.IdeaUniqeName))
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
                        label = "!این نام یکتا قبلا ثبت شده است"
                    });
                }
                if (request.IdeaUniqeName.Length > 75 || request.IdeaUniqeName.Length < 3)
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!طول متن نام یکتا  باید بین 3 الی 75 کاراکتر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.IdeaSorting.ToString()))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی را وارد نمایید"
                    });
                }

                if (request.IdeaSorting.GetType() != typeof(int))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
                    });
                }

                if (request.IdeaSorting is int & request.IdeaSorting < 1)
                {

                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.IdeaLongDescription))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن ایده را وارد نمایید"
                    });
                }

                if (string.IsNullOrWhiteSpace(request.IdeaMetaDesc))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!متن توضیحات متا را وارد نمایید"
                    });
                }

                //if (request.IdeaMetaDesc.Length > 170 || request.IdeaMetaDesc.Length < 140)
                //{
                //    id = id + 1;
                //    Errors.Add(new IdLabelDto
                //    {
                //        id = id,
                //        label = "!طول متن توضیحات متا محتوی باید بین 140 الی 170 کاراکتر باشد"
                //    });
                //}
                if (string.IsNullOrWhiteSpace(request.IdeaUniqeName))
                {
                    id = id + 1;
                    Errors.Add(new IdLabelDto
                    {
                        id = id,
                        label = "!دسته بندی را انتخاب نمایید"
                    });
                }

                var categoryinfo = _context.IdeaCategories.FirstOrDefault(c => c.IdeaCategoryUniqeName == request.IdeaCategoryUniqeName);

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
                    string imageUrlsString = string.Join(",", request.IdeaImages.Where(img => !string.IsNullOrWhiteSpace(img)));

                    Idea idea = new Idea()
                    {

                        IdeaTitle = request.IdeaTitle,
                        OnlinePrice = false,
                        IdeaOwnerName = request.IdeaOwnerName,
                        IdeaOwnerId = request.IdeaOwnerId,
                        AverageStar = 0,
                        SumStar = 0,
                        CountStar = 0,
                        IdeaUniqeName = request.IdeaUniqeName,
                        CommentSituation = request.CommentSituation,
                        CommentShow = request.CommentShow,
                        IdeaSorting = request.IdeaSorting,
                        IdeaLongDescription = request.IdeaLongDescription,
                        IdeaMetaDesc = request.IdeaMetaDesc,
                        IdeaImageAlt = request.IdeaImageAlt,
                        IdeaPublish = request.IdeaPublish,
                        MainImage = request.MainImage,
                        IdeaImage = imageUrlsString,
                        IdeaCategoryUniqeName = categoryinfo.IdeaCategoryUniqeName,
                        IdeaCategoryTitle = categoryinfo.IdeaCategoryTitle,
                        IsIndex = request.IsIndex,
                        IsRemoved = true
                         
                    };
                    idea.UpdateTime = idea.InsertTime;
                    _context.Ideas.Add(idea);
                    _context.SaveChanges();
                    


                    return new ResultDto<ResultIdeaSetDto>()
                    {
                        Data = new ResultIdeaSetDto()
                        {
                            IdeaId = idea.Id,
                            Errors = Errors,
                        },
                        IsSuccess = true,
                        Message = " محتوی جدید با موفقیت درج شد",
                    };
                }

                else
                {
                    return new ResultDto<ResultIdeaSetDto>()
                    {
                        Data = new ResultIdeaSetDto()
                        {
                            IdeaId = 0,
                            Errors = Errors,
                        },
                        IsSuccess = false,
                        Message = "!محتوی جدید درج نشد"
                    };

                }

            }

            catch (Exception)
            {
                return new ResultDto<ResultIdeaSetDto>()
                {
                    Data = new ResultIdeaSetDto()
                    {
                        IdeaId = 0,
                        Errors = Errors,
                    },
                    IsSuccess = false,
                    Message = "!محتوی جدید درج نشد"
                };

            }
        }
    }


    public class RequestIdeaSetDto
    {
        public string IdeaTitle { get; set; }
        public string IdeaOwnerName { get; set; }
        public string IdeaOwnerId { get; set; }

        public string IdeaUniqeName { get; set; }
        public bool CommentSituation { get; set; }
        public bool CommentShow { get; set; }
        public int IdeaSorting { get; set; }
        public string IdeaLongDescription { get; set; }
        public string IdeaMetaDesc { get; set; }
        public string IdeaImageAlt { get; set; }
        public bool IdeaPublish { get; set; }
        public string MainImage {  get; set; }
        public List<string> IdeaImages { get; set; }
        public string IdeaCategoryUniqeName { get; set; }
        public string IdeaCategoryTitle { get; set; }
        public bool IsIndex {  get; set; }
    }
    public class ResultIdeaSetDto
    {
        public long IdeaId { get; set; }
        public List<IdLabelDto> Errors { get; set; }
    }
}
