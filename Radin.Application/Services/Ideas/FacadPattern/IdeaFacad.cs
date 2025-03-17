using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Ideas.Commands.CommentRemove;
using Radin.Application.Services.Ideas.Commands.CommentSet;
using Radin.Application.Services.Ideas.Commands.IdeaCategoryEdit;
using Radin.Application.Services.Ideas.Commands.IdeaCategoryRemove;
using Radin.Application.Services.Ideas.Commands.IdeaCategorySet;
using Radin.Application.Services.Ideas.Commands.IdeaIndex;
using Radin.Application.Services.Ideas.Commands.IdeaRankSet;
using Radin.Application.Services.Ideas.Commands.IdeaSet;
using Radin.Application.Services.Ideas.Commands.IIdeaEdit;
using Radin.Application.Services.Ideas.Commands.IIdeaRemove;
using Radin.Application.Services.Ideas.Queries.IdeaCategoryGet;
using Radin.Application.Services.Ideas.Queries.IdeaGet;
using Radin.Application.Services.Product.Commands.Mapping;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Ideas.FacadPattern
{
    public class IdeaFacad: IIdeaFacad
    {
        private readonly IDataBaseContext _context;
        public IdeaFacad(
            IDataBaseContext context

            )
        {

            _context = context;

        }

        private IIdeaSetService _ideaSetService;
        public IIdeaSetService IdeaSetService
        {
            get
            {
                return _ideaSetService = _ideaSetService ?? new IdeaSetService(_context);

            }
        }


        private IIdeaCategorySetService _ideaCategorySetService;
        public IIdeaCategorySetService IdeaCategorySetService
        {
            get
            {
                return _ideaCategorySetService = _ideaCategorySetService ?? new IdeaCategorySetService(_context);

            }
        }



        private IIdeaGetService _ideaGetService;
        public IIdeaGetService IdeaGetService
        {
            get
            {
                return _ideaGetService = _ideaGetService ?? new IdeaGetService(_context);

            }
        }

        private IIdeaEditService _ideaEditService;
        public IIdeaEditService IdeaEditService
        {
            get
            {
                return _ideaEditService = _ideaEditService ?? new IdeaEditService(_context);

            }
        }

        private IIdeaRemoveService _ideaRemoveService;
        public IIdeaRemoveService IdeaRemoveService
        {
            get
            {
                return _ideaRemoveService = _ideaRemoveService ?? new IdeaRemoveService(_context);

            }
        }



        private IIdeaIndexService _ideaIndexService;
        public IIdeaIndexService IdeaIndexService
        {
            get
            {
                return _ideaIndexService = _ideaIndexService ?? new IdeaIndexService(_context);

            }
        }

        private IIdeaCategoryGetService _ideaCategoryGetService;
        public IIdeaCategoryGetService IdeaCategoryGetService
        {
            get
            {
                return _ideaCategoryGetService = _ideaCategoryGetService ?? new IdeaCategoryGetService(_context);

            }
        }



        private IIdeaCategoryEditService _ideaCategoryEditService;
        public IIdeaCategoryEditService IdeaCategoryEditService
        {
            get
            {
                return _ideaCategoryEditService = _ideaCategoryEditService ?? new IdeaCategoryEditService(_context);

            }
        }


        private IIdeaCategoryRemoveService _ideaCategoryRemoveService;
        public IIdeaCategoryRemoveService IdeaCategoryRemoveService
        {
            get
            {
                return _ideaCategoryRemoveService = _ideaCategoryRemoveService ?? new IdeaCategoryRemoveService(_context);

            }
        }



        private IIdeaCommentSetService _ideaCommentSetService;
        public IIdeaCommentSetService IdeaCommentSetService
        {
            get
            {
                return _ideaCommentSetService = _ideaCommentSetService ?? new IdeaCommentSetService(_context);

            }
        }




        private IIdeaSubCommentSetService _ideaSubCommentSetService;
        public IIdeaSubCommentSetService IdeaSubCommentSetService
        {
            get
            {
                return _ideaSubCommentSetService = _ideaSubCommentSetService ?? new IdeaSubCommentSetService(_context);

            }
        }


        private IIdeaCommentRemoveService _ideaCommentRemoveService;
        public IIdeaCommentRemoveService IdeaCommentRemoveService
        {
            get
            {
                return _ideaCommentRemoveService = _ideaCommentRemoveService ?? new IdeaCommentRemoveService(_context);

            }
        }


        private IIdeaSubCommentRemoveService _ideaSubCommentRemoveService;
        public IIdeaSubCommentRemoveService IdeaSubCommentRemoveService
        {
            get
            {
                return _ideaSubCommentRemoveService = _ideaSubCommentRemoveService ?? new IdeaSubCommentRemoveService(_context);

            }
        }
        
        private IIdeaRatingService _ideaRatingService;
        public IIdeaRatingService IdeaRatingService
        {
            get
            {
                return _ideaRatingService = _ideaRatingService ?? new IdeaRatingService(_context);

            }
        }
    }
}
