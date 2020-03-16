using System;
using System.Collections.Generic;
using System.Linq;
using HrNewsPortal.Data.Repositories;
using HrNewsPortal.Services;
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
        public void GetStoriesTests()
        {
            var stories = _service.GetStories(1, 1);

            Assert.IsNotNull(stories);
            Assert.AreEqual(1, stories.Count());

            //var itemIds = new List<int>()
            //{
            //    1
            //};
            
            //stories = _service.GetStories(itemIds, 2);

            //Assert.IsNotNull(stories);
            //Assert.AreEqual(1, stories.Count());
        }

        [TestMethod]
        public void SearchStoriesTests()
        {

        }

        [TestMethod]
        public void GetCommentsTests()
        {
            var comments = _service.GetComments(1, 3);

            Assert.IsNotNull(comments);
            Assert.AreEqual(1, comments.Count());

            var itemIds = new List<int>()
            {
                15
            };

            comments = _service.GetComments(itemIds, 1);

            Assert.IsNotNull(comments);
            Assert.AreEqual(5, comments.Count());
        }

        [TestMethod]
        public void SearchCommentsTests()
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
