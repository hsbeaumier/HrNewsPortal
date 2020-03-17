using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using HrNewsPortal.Models;
using HrNewsPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrNewsPortal.Portal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadMoreCommentsController : ControllerBase
    {
        #region fields

        private IHrNewsDataService _service;

        #endregion

        #region constructor

        public LoadMoreCommentsController(IHrNewsDataService service)
        {
            _service = service;
        }

        #endregion

        #region methods

        [HttpGet]
        public IEnumerable<Comment> Get(int itemId, string type, int lastCommentIdLoaded, int numberToLoad)
        {
            var comments = new List<Comment>();

            if (type == "story")
            {
                var story = _service.GetStory(itemId);

                comments = _service.GetComments(story.Kids.ToList());
            }

            if (type == "comment")
            {
                var comment = _service.GetComment(itemId);

                comments = _service.GetComments(comment.Kids.ToList());
            }

            if (type == "poll")
            {
                var poll = _service.GetPoll(itemId);

                comments = _service.GetComments(poll.Kids.ToList());
            }

            if (lastCommentIdLoaded == 0)
            {
                return comments.Take(numberToLoad);
            }
            else
            {
                var indexOfLastLoaded = comments.FindIndex(comment => comment.Id == lastCommentIdLoaded);

                var take = Math.Min(numberToLoad, comments.Count - indexOfLastLoaded);

                return comments.GetRange(indexOfLastLoaded, take);
            }
        }
        
        #endregion
    }
}
