using System;
using System.Collections.Generic;
using System.Linq;
using HrNewsPortal.Data.Repositories;
using HrNewsPortal.Services.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HrNewsPortal.Services.IntegrationTests
{
    [TestClass]
    public class HrNewsDataServiceTests
    {
        #region fields

        private HrNewsRepository _repo;
        private HrNewsDataService _service;

        #endregion

        #region initialize

        [TestInitialize]
        public void Initialize()
        {
            var configuration = TestHelper.InitializeConfiguration("appsettings.json");

            Assert.IsNotNull(configuration);

            var generalSettings = GeneralSettingsBuilder.Build(configuration);
            
            _repo = new HrNewsRepository(generalSettings);

            Assert.IsNotNull(_repo);

            _service = new HrNewsDataService(_repo);

            Assert.IsNotNull(_service);
        }

        #endregion

        #region methods

        [TestMethod]
        public void GetStoryTest()
        {
            var story = _service.GetStory(1);

            Assert.IsNotNull(story);
        }

        [TestMethod]
        public void GetStoriesTests()
        {
            var stories = _service.GetStories(1);

            Assert.IsNotNull(stories);
            Assert.AreEqual(1, stories.Count());

            var itemIds = new List<int>()
            {
                1
            };
            
            stories = _service.GetStories(itemIds);

            Assert.IsNotNull(stories);
            Assert.AreEqual(1, stories.Count());
        }

        [TestMethod]
        public void SearchStoriesTests()
        {
            var storiesBy = _service.SearchStories("pg", null, null, null);
            
            Assert.IsTrue(storiesBy.All(s => s.By == "pg"));

            var storiesTime = _service.SearchStories(null, "2006-10-09 19:21:14", null, null);

            Assert.IsTrue(storiesTime.All(s => s.Time == new DateTime(2006,10,09,19,21,14)));

            var storiesUrl = _service.SearchStories(null, null, "http://www.google.com", null);

            Assert.IsTrue(storiesUrl.All(s => s.Url.Contains("http://www.google.com")));

            var storiesScore = _service.SearchStories(null, null, null, "5");

            Assert.IsTrue(storiesScore.All(s => s.Score == 5));

            var storiesByWithScore = _service.SearchStories("amichail", null, null, "2");

            Assert.IsTrue(storiesScore.All(s => s.By == "amichail" && s.Score == 2));
        }

        [TestMethod]
        public void GetCommentTest()
        {
            var comment = _service.GetComment(15);

            Assert.IsNotNull(comment);
        }

        [TestMethod]
        public void GetCommentsTests()
        {
            var comments = _service.GetComments(1);

            Assert.IsNotNull(comments);
            Assert.AreEqual(1, comments.Count());

            var itemIds = new List<int>()
            {
                15,
                17,
                22,
                23,
                30
            };

            comments = _service.GetComments(itemIds);

            Assert.IsNotNull(comments);
            Assert.AreEqual(5, comments.Count());
        }

        [TestMethod]
        public void SearchCommentsTests()
        {
            var commentsBy = _service.SearchComments("pg",null, null);

            Assert.IsTrue(commentsBy.All(s => s.By == "pg"));

            var commentsTime = _service.SearchComments(null, "2007-02-20 08:34:32", null);

            Assert.IsTrue(commentsTime.All(s => s.Time == new DateTime(2007,02,20,08,34,32)));

            var commentsText = _service.SearchComments(null, null, "you");

            Assert.IsTrue(commentsText.All(s => s.Text.ToLower().Contains("you")));
        }

        [TestMethod]
        public void GetPollTest()
        {

        }

        [TestMethod]
        public void GetPollsTests()
        {

        }

        [TestMethod]
        public void SearchPollsTests()
        {

        }

        [TestMethod]
        public void GetJobsTests()
        {
            
        }

        [TestMethod]
        public void SearchJobsTests()
        {

        }

        #endregion
    }
}
