//using Microsoft.EntityFrameworkCore;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Ideas.Commands.IdeaRankSet;
//using Radin.Domain.Entities.Ideas;
//using Radin.Domain.Entities.Samples;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Samples.Commands.SampleRankSet.SampleRatingService;

//namespace Radin.Application.Services.Samples.Commands.SampleRankSet
//{
//    public interface ISampleRatingService
//    {
//        Task AddRatingAsync(SampleRankRequest request);
//    }



//    public class SampleRatingService : ISampleRatingService
//    {
//        private readonly IDataBaseContext _context;

//        public SampleRatingService(IDataBaseContext context
//            )
//        {
//            _context = context;
//        }

//        public async Task AddRatingAsync(SampleRankRequest request)
//        {

//            var sample = await _context.Samples.Include(i => i.SampleRanks).Where(i => i.Id == request.sampleId).FirstAsync();
//            Console.WriteLine("2");

//            if (sample == null)
//            {

//                throw new ArgumentException("ایده مورد نظر وجود ندارد");
//            }
//            if (sample.SampleRanks == null)
//            {
//                sample.SampleRanks = new List<SampleRank>();
//            }
//            var existingRating = sample.SampleRanks.FirstOrDefault(ir => ir.UserId == request.userId);
//            Console.WriteLine(@$"h={existingRating}");

//            if (existingRating != null)
//            {

//                throw new InvalidOperationException(@$"شما قبلا نظر خود را ثبت کرده اید {existingRating.StarPoint}");

//            }


//            var newRating = new SampleRank
//            {
//                SampleId = request.sampleId,
//                UserId = request.userId,
//                StarPoint = request.starPoint,

//            };
//            Console.WriteLine("2");

//            sample.SampleRanks.Add(newRating);
//            sample.SumStar += request.starPoint;
//            sample.CountStar += 1;
//            sample.AverageStar = (float)sample.SumStar / sample.CountStar;

//            await _context.SaveChangesAsync();
//        }


//        public class SampleRankRequest
//        {
//            public long sampleId { get; set; }
//            public string? userId { get; set; }
//            public int starPoint { get; set; }
//        }

//    }
//}
