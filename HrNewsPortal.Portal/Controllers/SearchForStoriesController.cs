using System.Collections.Generic;
using HrNewsPortal.Models;
using HrNewsPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrNewsPortal.Portal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchForStoriesController : ControllerBase
    {
        #region fields

        private IHrNewsDataService _service;

        #endregion

        #region constructor

        public SearchForStoriesController(IHrNewsDataService service)
        {
            _service = service;
        }

        #endregion

        #region methods

        [HttpGet]
        public IEnumerable<Story> Get(string by = null, string time = null, string url = null, string score = null)
        {
            return _service.SearchStories(by, time, url, score).ToArray();
        }

        #endregion
    }
}
