using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radin.Application.Interfaces.Contexts;
using Radin.Domain.Entities.Ideas;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Radin.Application.Services.Ideas.Commands.IdeaRankSet.IdeaRatingService;

namespace Radin.Application.Services.Ideas.Commands.IdeaRankSet
{
    public interface IIdeaRatingService
    {

        Task AddRatingAsync(IdeaRankRequest request);


    }



    public class IdeaRatingService : IIdeaRatingService
    {
        private readonly IDataBaseContext _context;

        public IdeaRatingService(IDataBaseContext context
            )
        {
            _context = context;
        }

        public async Task AddRatingAsync(IdeaRankRequest request)
        {
           
            var idea = await _context.Ideas.Include(i => i.IdeaRanks).Where(i => i.Id == request.ideaId).FirstAsync();
            Console.WriteLine("2");

            if (idea == null)
            {

                throw new ArgumentException("ایده مورد نظر وجود ندارد");
            }
            if (idea.IdeaRanks == null)
            {
                idea.IdeaRanks = new List<IdeaRank>();
            }
            var existingRating = idea.IdeaRanks.FirstOrDefault(ir => ir.UserId == request.userId );
            Console.WriteLine(@$"h={existingRating}");

            if (existingRating!=null )
            {

                throw new InvalidOperationException(@$"شما قبلا نظر خود را ثبت کرده اید {existingRating.StarPoint}");
                
            }
            

            var newRating = new IdeaRank
            {
                IdeaId = request.ideaId,
                UserId = request.userId,
                StarPoint = request.starPoint,

            };
            Console.WriteLine("2");

            idea.IdeaRanks.Add(newRating);
            idea.SumStar += request.starPoint;
            idea.CountStar += 1;
            idea.AverageStar = (float)idea.SumStar / idea.CountStar;

            await _context.SaveChangesAsync();
        }


        public class IdeaRankRequest
        {
            public long ideaId {  get; set; }
            public string? userId { get; set; }
            public int starPoint { get; set; }
        }

    }   
    
}
