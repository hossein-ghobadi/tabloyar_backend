//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
//using Radin.Application.Services.Contents.Commands.ContentSet;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Contents;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Contents.Commands.ContentEdit
//{
//    public interface IContentEditService
//    {
//        ResultDto<ResultUpdateContentDto> Execute(UpdateContentDto request);
//    }
//    public class ContentEditService : IContentEditService
//    {
//        private readonly IDataBaseContext _context;

//        public ContentEditService(IDataBaseContext context)
//        {
//            _context = context;
//        }
//        public ResultDto<ResultUpdateContentDto> Execute(UpdateContentDto updateDto)
//        {
//            var Errors = new List<IdLabelDto>();
//            int id = 0;
//            try
//            {
//                var content = _context.Contents.FirstOrDefault(c => c.Id == updateDto.Id);
//                if (content == null)
//                {
//                    return new ResultDto<ResultUpdateContentDto>()
//                    {
//                        Data = new ResultUpdateContentDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!محتوی یافت نشد"
//                    };
//                }

//                if (string.IsNullOrWhiteSpace(updateDto.ContentTitle))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عنوان محتوی را وارد نمایید"
//                    });
//                }
//                // if (updateDto.ContentTitle.Length > 60 || updateDto.ContentTitle.Length < 35)
//                // {
//                //     id = id + 1;
//                //     Errors.Add(new IdLabelDto
//                //     {
//                //         id = id,
//                //         label = "!طول متن عنوان محتوی باید بین 35 الی 60 کاراکتر باشد"
//                //     });
//                // }
//                if (string.IsNullOrWhiteSpace(updateDto.ContentUniqeName))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!نام یکتا را وارد نمایید"
//                    });
//                }

//                if (updateDto.ContentUniqeName.Length > 75 || updateDto.ContentUniqeName.Length < 3)
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!طول متن نام یکتا محتوی باید بین 3 الی 75 کاراکتر باشد"
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(updateDto.ContentSorting.ToString()))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی را وارد نمایید"
//                    });
//                }

//                if (updateDto.ContentSorting.GetType() != typeof(int))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید مقدار صحیح باشد"
//                    });
//                }
//                if (updateDto.ContentSorting is int & updateDto.ContentSorting < 1)
//                {

//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!عدد مرتب سازی باید بزرگتر از صفر باشد"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.ContentLongDescription))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!متن محتوی را وارد نمایید"
//                    });
//                }
//                if (string.IsNullOrWhiteSpace(updateDto.ContentMetaDesc))
//                {
//                    id = id + 1;
//                    Errors.Add(new IdLabelDto
//                    {
//                        id = id,
//                        label = "!متن توضیحات متا را وارد نمایید"
//                    });
//                }

//                // if (updateDto.ContentMetaDesc.Length > 170 || updateDto.ContentMetaDesc.Length < 140)
//                // {
//                //     id = id + 1;
//                //     Errors.Add(new IdLabelDto
//                //     {
//                //         id = id,
//                //         label = "!طول متن توضیحات متا محتوی باید بین 140 الی 170 کاراکتر باشد"
//                //     });
//                // }
//                //if (string.IsNullOrWhiteSpace(updateDto.CategoryUniqeName))
//                //{
//                //    id = id + 1;
//                //    Errors.Add(new IdLabelDto
//                //    {
//                //        id = id,
//                //        label = "!دسته بندی را انتخاب نمایید"
//                //    });
//                //}
//                var categoryinfo = _context.Categories.FirstOrDefault(c => c.CategoryUniqeName == updateDto.CategoryUniqeName);
//                //if (categoryinfo == null)
//                //{
//                //    id = id + 1;
//                //    Errors.Add(new IdLabelDto
//                //    {
//                //        id = id,
//                //        label = "!دسته بندی با این نام وجود ندارد"
//                //    });
//                //}

//                var contentsTab = _context.Contents.ToList();
//                var OtherContent = contentsTab.Remove(content);
//                if (OtherContent)
//                {
//                    var NameDup = contentsTab.FirstOrDefault(p => p.ContentUniqeName == updateDto.ContentUniqeName);
//                    var TitleDup = contentsTab.FirstOrDefault(c => c.ContentTitle == updateDto.ContentTitle);
//                    if (TitleDup != null)
//                    {
//                        id = id + 1;
//                        Errors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این عنوان محتوی قبلا ثبت شده است"
//                        });
//                    }
//                    if (NameDup != null)
//                    {
//                        id = id + 1;
//                        Errors.Add(new IdLabelDto
//                        {
//                            id = id,
//                            label = "!این نام یکتای محتوی قبلا ثبت شده است"
//                        });
//                    }
//                }


//                if (Errors.Count() < 1)
//                {
//                    content.ContentTitle = updateDto.ContentTitle;
//                    content.ContentUniqeName = updateDto.ContentUniqeName;
//                    content.CommentSituation = updateDto.CommentSituation;
//                    content.CommentShow = updateDto.CommentShow;
//                    content.ContentSorting = updateDto.ContentSorting;
//                    content.ContentLongDescription = updateDto.ContentLongDescription;
//                    content.ContentMetaDesc = updateDto.ContentMetaDesc;
//                    content.ContentImageAlt = updateDto.ContentImageAlt;
//                    content.ContentPublish = updateDto.ContentPublish;
//                    content.ContentImage = updateDto.ContentImage;
//                    if (categoryinfo != null)
//                    {
//                        content.CategoryTitle = categoryinfo.CategoryTitle;
//                        content.CategoryUniqeName = updateDto.CategoryUniqeName;

//                    }
//                    content.Canonical=updateDto.Canonical;
//                    content.IsIndex = updateDto.IsIndex;
//                    _context.SaveChanges();
//                    return new ResultDto<ResultUpdateContentDto>()
//                    {
//                        Data = new ResultUpdateContentDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = true,
//                        Message = "!ویرایش محتوی با موفقیت انجام شد"
//                    };
//                }
//                else
//                {
//                    return new ResultDto<ResultUpdateContentDto>()
//                    {
//                        Data = new ResultUpdateContentDto()
//                        {
//                            Errors = Errors,
//                        },
//                        IsSuccess = false,
//                        Message = "!ویرایش دسته بندی محتوی انجام نشد"
//                    };
//                }

//            }
//            catch
//            {
//                return new ResultDto<ResultUpdateContentDto>()
//                {
//                    Data = new ResultUpdateContentDto()
//                    {
//                        Errors = Errors,
//                    },
//                    IsSuccess = false,
//                    Message = "!ویرایش دسته بندی محتوی انجام نشد"
//                };
//            }
//        }
//    }


//    public class UpdateContentDto
//    {
//        public long Id { get; set; }
//        public string ContentTitle { get; set; }
//        public string ContentUniqeName { get; set; }
//        public bool CommentSituation { get; set; }
//        public bool CommentShow { get; set; }
//        public int ContentSorting { get; set; }
//        public string ContentLongDescription { get; set; }
//        public string ContentMetaDesc { get; set; }
//        public string ContentImageAlt { get; set; }
//        public string? ContentImageTitle { get; set; }
//        public bool ContentPublish { get; set; }
//        public string ContentImage { get; set; }
//        public string CategoryUniqeName { get; set; }
//        public string CategoryTitle { get; set; }
//        public string Canonical { get; set; }
//        public bool IsIndex { get; set; }
//    }

//    public class ResultUpdateContentDto
//    {
//        public List<IdLabelDto> Errors { get; set; }
//    }


//}
