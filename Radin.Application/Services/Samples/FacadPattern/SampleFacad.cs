using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Samples.Commands.CommentRemove;
using Radin.Application.Services.Samples.Commands.CommentSet;
using Radin.Application.Services.Samples.Commands.SampleCategoryEdit;
using Radin.Application.Services.Samples.Commands.SampleCategoryRemove;
using Radin.Application.Services.Samples.Commands.SampleCategorySet;
using Radin.Application.Services.Samples.Commands.SampleEdit;
using Radin.Application.Services.Samples.Commands.SampleIndex;
using Radin.Application.Services.Samples.Commands.SampleRankSet;
using Radin.Application.Services.Samples.Commands.SampleRemove;
using Radin.Application.Services.Samples.Commands.SampleSet;

using Radin.Application.Services.Samples.Queries.SampleCategoryGet;
using Radin.Application.Services.Samples.Queries.SampleGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Samples.FacadPattern
{
    public class SampleFacad:ISampleFacad
    {

        private readonly IDataBaseContext _context;
        public SampleFacad(
            IDataBaseContext context

            )
        {

            _context = context;

        }

        private ISampleSetService _sampleSetService;
        public ISampleSetService SampleSetService
        {
            get
            {
                return _sampleSetService = _sampleSetService ?? new SampleSetService(_context);

            }
        }


        private ISampleCategorySetService _sampleCategorySetService;
        public ISampleCategorySetService SampleCategorySetService
        {
            get
            {
                return _sampleCategorySetService = _sampleCategorySetService ?? new SampleCategorySetService(_context);

            }
        }



        private ISampleGetService _sampleGetService;
        public ISampleGetService SampleGetService
        {
            get
            {
                return _sampleGetService = _sampleGetService ?? new SampleGetService(_context);

            }
        }

        private ISampleEditService _sampleEditService;
        public ISampleEditService SampleEditService
        {
            get
            {
                return _sampleEditService = _sampleEditService ?? new SampleEditService(_context);

            }
        }

        private ISampleRemoveService _sampleRemoveService;
        public ISampleRemoveService SampleRemoveService
        {
            get
            {
                return _sampleRemoveService = _sampleRemoveService ?? new SampleRemoveService(_context);

            }
        }


        private ISampleIndexService _sampleIndexService;
        public ISampleIndexService SampleIndexService
        {
            get
            {
                return _sampleIndexService = _sampleIndexService ?? new SampleIndexService(_context);

            }
        }

        private ISampleCategoryGetService _sampleCategoryGetService;
        public ISampleCategoryGetService SampleCategoryGetService
        {
            get
            {
                return _sampleCategoryGetService = _sampleCategoryGetService ?? new SampleCategoryGetService(_context);

            }
        }



        private ISampleCategoryEditService _sampleCategoryEditService;
        public ISampleCategoryEditService SampleCategoryEditService
        {
            get
            {
                return _sampleCategoryEditService = _sampleCategoryEditService ?? new SampleCategoryEditService(_context);

            }
        }


        private ISampleCategoryRemoveService _sampleCategoryRemoveService;
        public ISampleCategoryRemoveService SampleCategoryRemoveService
        {
            get
            {
                return _sampleCategoryRemoveService = _sampleCategoryRemoveService ?? new SampleCategoryRemoveService(_context);

            }
        }



        private ISampleCommentSetService _sampleCommentSetService;
        public ISampleCommentSetService SampleCommentSetService
        {
            get
            {
                return _sampleCommentSetService = _sampleCommentSetService ?? new SampleCommentSetService(_context);

            }
        }




        private ISampleSubCommentSetService _sampleSubCommentSetService;
        public ISampleSubCommentSetService SampleSubCommentSetService
        {
            get
            {
                return _sampleSubCommentSetService = _sampleSubCommentSetService ?? new SampleSubCommentSetService(_context);

            }
        }


        private ISampleCommentRemoveService _sampleCommentRemoveService;
        public ISampleCommentRemoveService SampleCommentRemoveService
        {
            get
            {
                return _sampleCommentRemoveService = _sampleCommentRemoveService ?? new SampleCommentRemoveService(_context);

            }
        }


        private ISampleSubCommentRemoveService _sampleSubCommentRemoveService;
        public ISampleSubCommentRemoveService SampleSubCommentRemoveService
        {
            get
            {
                return _sampleSubCommentRemoveService = _sampleSubCommentRemoveService ?? new SampleSubCommentRemoveService(_context);

            }
        }


        private ISampleRatingService _SampleRatingService;
        public ISampleRatingService SampleRatingService
        {
            get
            {
                return _SampleRatingService = _SampleRatingService ?? new SampleRatingService(_context);

            }
        }
    }
}
