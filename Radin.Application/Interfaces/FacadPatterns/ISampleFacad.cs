using Radin.Application.Services.Samples.Commands.SampleCategoryEdit;
using Radin.Application.Services.Samples.Commands.SampleCategoryRemove;
using Radin.Application.Services.Samples.Commands.SampleCategorySet;
using Radin.Application.Services.Samples.Commands.SampleSet;

using Radin.Application.Services.Samples.Queries.SampleCategoryGet;
using Radin.Application.Services.Samples.Queries.SampleGet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radin.Application.Services.Samples.Commands.SampleEdit;
using Radin.Application.Services.Samples.Commands.SampleRemove;
using Radin.Application.Services.Ideas.Commands.CommentRemove;
using Radin.Application.Services.Ideas.Commands.CommentSet;
using Radin.Application.Services.Samples.Commands.CommentSet;
using Radin.Application.Services.Samples.Commands.CommentRemove;
using Radin.Application.Services.Samples.Commands.SampleRankSet;
using Radin.Application.Services.Samples.Commands.SampleIndex;

namespace Radin.Application.Interfaces.FacadPatterns
{
    public interface ISampleFacad
    {
        ISampleSetService SampleSetService { get; }
        ISampleCategorySetService SampleCategorySetService { get; }
        ISampleGetService SampleGetService { get; }
        ISampleRemoveService SampleRemoveService { get; }
        ISampleIndexService SampleIndexService { get; }
        ISampleEditService SampleEditService { get; }

        ISampleCategoryGetService SampleCategoryGetService { get; }
        ISampleCategoryEditService SampleCategoryEditService { get; }
        ISampleCategoryRemoveService SampleCategoryRemoveService { get; }





        ISampleCommentSetService SampleCommentSetService { get; }
        ISampleSubCommentSetService SampleSubCommentSetService { get; }
        ISampleCommentRemoveService SampleCommentRemoveService { get; }
        ISampleSubCommentRemoveService SampleSubCommentRemoveService { get; }
        ISampleRatingService SampleRatingService { get; }
    }
}
