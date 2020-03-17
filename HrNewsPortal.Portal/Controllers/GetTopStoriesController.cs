using System.Collections.Generic;
using HrNewsPortal.Models;
using HrNewsPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrNewsPortal.Portal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetTopStoriesController : ControllerBase
    {
        #region fields

        private IHrNewsDataService _service;

        #endregion

        #region constructor

        public GetTopStoriesController(IHrNewsDataService service)
        {
            _service = service;
        }

        #endregion

        #region methods

        [HttpGet("{topStories}")]
        public IEnumerable<Story> Get(int topStories)
        {
            return _service.GetStories(topStories).ToArray();
        }

        #endregion
    }
}
