using System;
using System.Collections.Generic;
using System.Linq;
using HrNewsPortal.Data.Repositories;
using HrNewsPortal.Services;
using HrNewsPortal.Services.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HrNewsPortal.Data.IntegrationTests
{
    [TestClass]
    public class HrNewsRepositoryTests
    {
        #region fields

        private HrNewsRepository _repo;
        private HrNewsWebApiService _service;

        #endregion

        #region initialize

        [TestInitialize]
        public void Initialize()
        {
            var configuration = TestHelper.InitializeConfiguration("appsettings.json");

            Assert.IsNotNull(configuration);

            var generalSettings = GeneralSettingsBuilder.Build(configuration);

            var hrNewClientSettings = HrNewsClientSettingsBuilder.Build(configuration);

            Assert.IsNotNull(hrNewClientSettings);

            const string expectedUrl = "https://hacker-news.firebaseio.com/v0/";

            Assert.AreEqual(expectedUrl, hrNewClientSettings.HrNewsApiUrl);

            const string expectedConnectionString = "DefaultEndpointsProtocol=https;AccountName=hsbhrnewssa;AccountKey=0pJza+M3O0ynLUuiOFwFHje81cU9RlTOp1PbObbcrpFTYy8bk1ryibaWNd9PA5ey7CbBoQBgfITWuxWm+5bFvQ==;EndpointSuffix=core.windows.net";

            Assert.AreEqual(expectedConnectionString, generalSettings.AzureStorageConnectionString);

            _repo = new HrNewsRepository(generalSettings);

            Assert.IsNotNull(_repo);

            _service = new HrNewsWebApiService(hrNewClientSettings);

            Assert.IsNotNull(_service);
        }

        #endregion

        [TestMethod]
        public void InsertItemRecordsTest()
        {
            var startItemId = 1001;
            var takeItems = 1000;
            var newItemsTask = _service.GetItems(startItemId, takeItems);
            newItemsTask.Wait();
            var newItems = newItemsTask.Result;

            _repo.InsertItemRecords(newItems);
        }
        
        [TestMethod]
        public void GetMaxItemIdTest()
        {
            var maxItemId = _repo.GetMaxItemId();
            var maxItemIdForStory = _repo.GetMaxItemId("story");
            var maxItemIdForComment = _repo.GetMaxItemId("comment");
        }

        [TestMethod]
        public void GetRangeItemRecords()
        {
            var getTask = _repo.GetRangeItemRecords("comment", 100, 1, false);
            getTask.Wait();
            var items = getTask.Result;

            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());

            getTask = _repo.GetRangeItemRecords("comment", 100, 100, true);
            getTask.Wait();
            items = getTask.Result;

            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());

            getTask = _repo.GetRangeItemRecords("story", 100, 1, false);
            getTask.Wait();
            items = getTask.Result;

            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());

            getTask = _repo.GetRangeItemRecords("story", 100, 100, true);
            getTask.Wait();
            items = getTask.Result;

            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());

            var rangeItemIds = new List<int>();
            rangeItemIds.Add(1);
            rangeItemIds.Add(2);
            rangeItemIds.Add(3);
            rangeItemIds.Add(4);
            rangeItemIds.Add(5);

            getTask = _repo.GetRangeItemRecords(rangeItemIds);
            getTask.Wait();
            items = getTask.Result;

            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());
            Assert.AreEqual(5, items.Count);
        }

        [TestMethod]
        public void SearchItemRecords()
        {
            var keyValues = new Dictionary<string, object>();
            keyValues.Add("By", "andres");
            keyValues.Add("Score", 2);
            keyValues.Add("Title", "up");
            keyValues.Add("Url", "fam");
            var items = _repo.SearchItemRecords("story",keyValues);
            
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());

            keyValues = new Dictionary<string, object>();
            keyValues.Add("By", "altay");
            keyValues.Add("Text", "it");
            keyValues.Add("Time", new DateTime(2007,2,24,1,22,16));
            items = _repo.SearchItemRecords("comment", keyValues);
            
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());
        }

        [TestMethod]
        public void GetItemTest()
        {
            var storyRecord = _repo.GetItem("story", 1);

            Assert.IsNotNull(storyRecord);

            var commentRecord = _repo.GetItem("comment", 15);

            Assert.IsNotNull(commentRecord);
        }
    }
}
