//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Interfaces.FacadPatterns;
//using Radin.Application.Services.Contents.Commands.CommentRemove;
//using Radin.Application.Services.Contents.Commands.CommentSet;
//using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
//using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
//using Radin.Application.Services.Contents.Commands.ContentCategorySet;
//using Radin.Application.Services.Contents.Commands.ContentChangeIsIndex;
//using Radin.Application.Services.Contents.Commands.ContentEdit;
//using Radin.Application.Services.Contents.Commands.ContentRemove;
//using Radin.Application.Services.Contents.Commands.ContentSet;
//using Radin.Application.Services.Contents.Commands.SubCommentSet;
//using Radin.Application.Services.Contents.Queries.CategoryGet;
//using Radin.Application.Services.Contents.Queries.CommentInfoGet;
//using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
//using Radin.Application.Services.Contents.Queries.ContentGet;
//using Radin.Application.Services.Contents.Queries.HomeContentGet;
//using Radin.Application.Services.Contents.Queries.HomePageContentGet;
//using Radin.Application.Services.Ideas.Commands.IdeaSet;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Contents.FacadPattern
//{
//    public class ContentFacad:IContentFacad
//    {


//        private readonly IDataBaseContext _context;
//        public ContentFacad(
//            IDataBaseContext context

//            )
//        {

//            _context = context;

//        }



//        private IContentCategorySetService _contentCategorySetService;
//        public IContentCategorySetService ContentCategorySetService
//        {
//            get
//            {
//                return _contentCategorySetService = _contentCategorySetService ?? new ContentCategorySetService(_context);

//            }
//        }


//        private IContentCategoryGetService _contentCategoryGetService;
//        public IContentCategoryGetService ContentCategoryGetService
//        {
//            get
//            {
//                return _contentCategoryGetService = _contentCategoryGetService ?? new ContentCategoryGetService(_context);

//            }
//        }

//        //private ICategoryGetService _categoryGetService;
//        //public ICategoryGetService CategoryGetService
//        //{
//        //    get
//        //    {
//        //        return _categoryGetService = _categoryGetService ?? new CategoryGetService(_context);

//        //    }
//        //}


//        private IContentCategoryRemoveService _contentCategoryRemoveService;
//        public IContentCategoryRemoveService ContentCategoryRemoveService
//        {
//            get
//            {
//                return _contentCategoryRemoveService = _contentCategoryRemoveService ?? new ContentCategoryRemoveService(_context);

//            }
//        }


//        private IContentCategoryEditService _contentCategoryEditService;
//        public IContentCategoryEditService ContentCategoryEditService
//        {
//            get
//            {
//                return _contentCategoryEditService = _contentCategoryEditService ?? new ContentCategoryEditService(_context);

//            }
//        }



//        private IContentSetService _contentSetService;
//        public IContentSetService ContentSetService
//        {
//            get
//            {
//                return _contentSetService = _contentSetService ?? new ContentSetService(_context);

//            }
//        }


//        private IContentGetService _contentGetService;
//        public IContentGetService ContentGetService
//        {
//            get
//            {
//                return _contentGetService = _contentGetService ?? new ContentGetService(_context);

//            }
//        }

//        private IContentRemoveService _contentRemoveService;
//        public IContentRemoveService ContentRemoveService
//        {
//            get
//            {
//                return _contentRemoveService = _contentRemoveService ?? new ContentRemoveService(_context);

//            }
//        }

//        private IContentIndexService _contentIndexService;
//        public IContentIndexService ContentIndexService
//        {
//            get
//            {
//                return _contentIndexService = _contentIndexService ?? new ContentIndexService(_context);

//            }
//        }


//        private IContentEditService _contentEditService;
//        public IContentEditService ContentEditService
//        {
//            get
//            {
//                return _contentEditService = _contentEditService ?? new ContentEditService(_context);

//            }
//        }



//        private ICommentRemoveService _commentRemoveService;
//        public ICommentRemoveService CommentRemoveService
//        {
//            get
//            {
//                return _commentRemoveService = _commentRemoveService ?? new CommentRemoveService(_context);

//            }
//        }

//        private ISubCommentRemoveService _subCommentRemoveService;
//        public ISubCommentRemoveService SubCommentRemoveService
//        {
//            get
//            {
//                return _subCommentRemoveService = _subCommentRemoveService ?? new SubCommentRemoveService(_context);

//            }
//        }



//        private IContentTotalGetService _contentTotalGetService;
//        public IContentTotalGetService ContentTotalGetService
//        {
//            get
//            {
//                return _contentTotalGetService = _contentTotalGetService ?? new ContentTotalGetService(_context);

//            }
//        }

//        private IContentCategoryGetSummary _contentCategoryGetSummary;
//        public IContentCategoryGetSummary ContentCategoryGetSummary
//        {
//            get
//            {
//                return _contentCategoryGetSummary = _contentCategoryGetSummary ?? new ContentCategoryGetSummary(_context);

//            }
//        }


//        private ICategoryGetForContentService _categoryGetForContentService;
//        public ICategoryGetForContentService CategoryGetForContentService
//        {
//            get
//            {
//                return _categoryGetForContentService = _categoryGetForContentService ?? new CategoryGetForContentService(_context);

//            }
//        }


//        private IHomeContentGetService _homeContentGetService;
//        public IHomeContentGetService HomeContentGetService
//        {
//            get
//            {
//                return _homeContentGetService = _homeContentGetService ?? new HomeContentGetService(_context);

//            }
//        }


//        private IHomeUniqeContentGetService _homeUniqeContentGetService;
//        public IHomeUniqeContentGetService HomeUniqeContentGetService
//        {
//            get
//            {
//                return _homeUniqeContentGetService = _homeUniqeContentGetService ?? new HomeUniqeContentGetService(_context);

//            }
//        }


//        private IHomeGroupContentGetService _homeGroupContentGetService;
//        public IHomeGroupContentGetService HomeGroupContentGetService
//        {
//            get
//            {
//                return _homeGroupContentGetService = _homeGroupContentGetService ?? new HomeGroupContentGetService(_context);

//            }
//        }

//        private IHomePageContentGetService _homePageContentGetService;
//        public IHomePageContentGetService HomePageContentGetService
//        {
//            get
//            {
//                return _homePageContentGetService = _homePageContentGetService ?? new HomePageContentGetService(_context);

//            }
//        }

//        private ICommentSetService _commentSetService;
//        public ICommentSetService CommentSetService
//        {
//            get
//            {
//                return _commentSetService = _commentSetService ?? new CommentSetService(_context);

//            }
//        }


//        private ICommentInfoGetService _commentInfoGetService;
//        public ICommentInfoGetService CommentInfoGetService
//        {
//            get
//            {
//                return _commentInfoGetService = _commentInfoGetService ?? new CommentInfoGetService(_context);

//            }
//        }

//        private ISubCommentSetService _subCommentSetService;
//        public ISubCommentSetService SubCommentSetService
//        {
//            get
//            {
//                return _subCommentSetService = _subCommentSetService ?? new SubCommentSetService(_context);

//            }
//        }

//    }
//}
