// using Endpoint.Site.Models.ViewModels.Comment;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using NuGet.Protocol;
//using Radin.Application.Interfaces.FacadPatterns;
//using Radin.Application.Services.Contents.Commands.CommentSet;
//using Radin.Application.Services.Contents.Commands.SubCommentSet;
//using Radin.Domain.Entities.Users;
//using System.Security.Claims;

//namespace Endpoint.Site.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ApiCommentController : ControllerBase
//    {
//        private readonly IContentFacad _contentFacad;
//        //private readonly ICommentSetService _commentSetService;
//        //private readonly ISubCommentSetService _subCommentSetService;
//        public ApiCommentController(
//            //ICommentSetService commentSetService,
//            //ISubCommentSetService subCommentSetService,
//            IContentFacad contentFacad
//            )
//        {
//            //_commentSetService = commentSetService;
//            //_subCommentSetService = subCommentSetService;
//            _contentFacad = contentFacad;
//        }
//        [Authorize]
//        [HttpPost("SetComment")]
//        public IActionResult SetComment(SetCommentViewModel requestCommentSetDto)
//        {
//            var username = User.Identity.Name;
//            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
//            var userrole = User.Claims.FirstOrDefault(c => c.Type==ClaimTypes.Role)?.Value;
//            //var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

//            var result = _contentFacad.CommentSetService.Execute(new RequestCommentSetDto
//            {
//                ContentId = requestCommentSetDto.ContentId,
//                Name = username,
//                Email = useremail,
//                UserRole = userrole,
//                CommentText = requestCommentSetDto.CommentText,
//                Situation = requestCommentSetDto.Situation,


//            });
//            return Ok(result);
//        }

//        [Authorize]
//        [HttpPost("SetSubComment")]
//        public IActionResult SetSubComment(SubCommentSetViewModel requestSubCommentSetDto)
//        {
//            var username = User.Identity.Name;
//            var useremail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
//            var userrole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

//            var result = _contentFacad.SubCommentSetService.Execute(new RequestSubCommentSetDto
//            {
//                CommentId = requestSubCommentSetDto.reply,
//                Name = username,
//                Email = useremail,
//                UserRole = userrole,
//                ReplyMsg = requestSubCommentSetDto.CommentText,


//            });
//            return Ok(result);
//        }



//    }
//}
