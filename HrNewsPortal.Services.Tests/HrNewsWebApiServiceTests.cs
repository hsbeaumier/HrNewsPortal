using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HrNewsPortal.Core.Extensions;
using HrNewsPortal.Services.Builders;

namespace HrNewsPortal.Services.IntegrationTests
{
    [TestClass]
    public class HrNewsWebApiServiceTests
    {
        #region fields

        private HrNewsWebApiService _service;

        #endregion

        #region initialize

        [TestInitialize]
        public void Initialize()
        {
            var configuration = TestHelper.InitializeConfiguration("appsettings.json");

            Assert.IsNotNull(configuration);

            var hrNewClientSettings = HrNewsClientSettingsBuilder.Build(configuration);

            Assert.IsNotNull(hrNewClientSettings);

            const string expectedUrl = "https://hacker-news.firebaseio.com/v0/";

            Assert.AreEqual(expectedUrl, hrNewClientSettings.HrNewsApiUrl);

            _service = new HrNewsWebApiService(hrNewClientSettings);

            Assert.IsNotNull(_service);
        }

        #endregion

        #region methods

        [TestMethod]
        public void GetMaxItemIdTest()
        {
            if (_service != null)
            {
                var task = _service.GetMaxItemId();
                task.Wait();
                var maxItemId = task.Result;

                Assert.IsTrue(maxItemId > 1);
            }
        }

        [TestMethod]
        public void GetItemTest()
        {
            if (_service != null)
            {
                var task = _service.GetItem(1);
                task.Wait();
                var item = task.Result;

                Assert.IsNotNull(item);
                Assert.AreEqual(1, item.Id);
                Assert.AreEqual("pg",item.By);
                Assert.AreEqual(15, item.Descendants);
            
                int[] expectedKids = {15, 234509, 487171, 454426, 454424, 454410, 82729};
                Assert.IsTrue(item.Kids.Any());
                for (var i = 0; i < item.Kids.Length; i++)
                {
                    Assert.AreEqual(expectedKids[i], item.Kids[i]);
                }

                Assert.AreEqual(57, item.Score);

                var expectedTime = 1160418111.GetDateTime();
                var actualTime = item.Time.GetDateTime();
                Assert.AreEqual(expectedTime, actualTime);

                Assert.AreEqual("Y Combinator", item.Title);
                Assert.AreEqual("story", item.Type);
                Assert.AreEqual(@"http://ycombinator.com", item.Url);
            }
        }

        [TestMethod]
        public void GetItemsTest()
        {
            if (_service != null)
            {
                var getMaxItemIdTask = _service.GetMaxItemId();
                getMaxItemIdTask.Wait();
                var maxItemId = getMaxItemIdTask.Result;

                Assert.IsTrue(maxItemId > 1);

                var getItemsTask = _service.GetItems(1, maxItemId);
                getItemsTask.Wait();
                var items = getItemsTask.Result;

                Assert.IsNotNull(items);
                Assert.AreEqual(maxItemId, items.Count);
            }
        }

        #endregion
    }
}
