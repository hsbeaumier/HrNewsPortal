using System;
using System.Collections.Generic;
using System.Linq;
using HrNewsPortal.Data.Repositories;
using HrNewsPortal.Models;

namespace HrNewsPortal.Services
{
    public class HrNewsDataService : IHrNewsDataService
    {
        #region fields

        private HrNewsRepository _repo;

        #endregion

        #region constructor

        public HrNewsDataService(HrNewsRepository repo)
        {
            _repo = repo;
        }

        #endregion

        #region methods

        public List<Story> GetStories(int topStories, int commentLevels)
        {
            var maxStoryId = _repo.GetMaxItemId("story");

            var task = _repo.GetRangeItemRecords("story", topStories, maxStoryId, true);

            task.Wait();

            var itemRecords = task.Result;

            return TransformItemToStory(itemRecords, commentLevels);
        }
        
        public List<Story> GetStories(List<int> storyIds, int commentLevels)
        {
            var task = _repo.GetRangeItemRecords(storyIds);
            task.Wait();

            var itemRecords = task.Result;
            
            return TransformItemToStory(itemRecords, commentLevels);
        }

        public List<Story> SearchStories(string by, string time, string url, string score, int commentLevels)
        {
            var criteria = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(by))
            {
                criteria.Add("By", by);
            }

            if (!string.IsNullOrWhiteSpace(time))
            {
                if (DateTime.TryParse(time, out DateTime criteriaTime))
                {
                    criteria.Add("Time", criteriaTime);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(url))
            {
                criteria.Add("Url", url);
            }

            if (!string.IsNullOrWhiteSpace(score))
            {
                if (Int32.TryParse(time, out int criteriaScore))
                {
                    criteria.Add("Score", criteriaScore);
                }
            }

            var itemRecords = _repo.SearchItemRecords("story", criteria);
            
            return TransformItemToStory(itemRecords, commentLevels);
        }

        public List<Comment> GetComments(int topComments, int commentLevels)
        {
            var maxCommentId = _repo.GetMaxItemId("comment");

            var task = _repo.GetRangeItemRecords("comment", topComments, maxCommentId, true);
            
            task.Wait();

            var itemRecords = task.Result;

            return TransformItemToComment(itemRecords, commentLevels, 0);
        }

        public List<Comment> GetComments(List<int> commentIds, int commentLevels)
        {
            var task = _repo.GetRangeItemRecords(commentIds);
            task.Wait();

            var itemRecords = task.Result;

            return TransformItemToComment(itemRecords, commentLevels, 0);
        }

        public List<Comment> SearchComments(string by, string time, string text, int commentLevels)
        {
            var criteria = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(by))
            {
                criteria.Add("By", by);
            }

            if (!string.IsNullOrWhiteSpace(time))
            {
                if (DateTime.TryParse(time, out DateTime criteriaTime))
                {
                    criteria.Add("Time", criteriaTime);
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                criteria.Add("Text", text);
            }

            var itemRecords = _repo.SearchItemRecords("comment", criteria);

            return TransformItemToComment(itemRecords, commentLevels, 0);
        }

        public List<Poll> GetPolls(int topPolls, int commentLevels)
        {
            var maxPollId = _repo.GetMaxItemId("poll");

            var task = _repo.GetRangeItemRecords("poll", topPolls, maxPollId, true);
            task.Wait();
            var itemRecords = task.Result;

            return TransformItemToPoll(itemRecords, commentLevels);
        }

        public List<Poll> GetPolls(List<int> pollIds, int commentLevels)
        {
            var task = _repo.GetRangeItemRecords(pollIds);
            task.Wait();

            var itemsRecords = task.Result;

            return TransformItemToPoll(itemsRecords, commentLevels);
        }

        public List<Poll> SearchPolls(string by, string time, string text, string score, string title, int commentLevels)
        {
            var criteria = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(by))
            {
                criteria.Add("By", by);
            }

            if (!string.IsNullOrWhiteSpace(time))
            {
                if (DateTime.TryParse(time, out DateTime criteriaTime))
                {
                    criteria.Add("Time", criteriaTime);
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                criteria.Add("Text", text);
            }
            
            if (!string.IsNullOrWhiteSpace(score))
            {
                if (Int32.TryParse(time, out int criteriaScore))
                {
                    criteria.Add("Score", criteriaScore);
                }
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                criteria.Add("Title", title);
            }

            var itemRecords = _repo.SearchItemRecords("poll", criteria);

            return TransformItemToPoll(itemRecords, commentLevels);
        }

        public List<PollOption> GetPollsOptions(List<int> pollOptionIds)
        {
            var task = _repo.GetRangeItemRecords(pollOptionIds);
            task.Wait();

            var itemRecords = task.Result;

            return TransformItemToPollOption(itemRecords);
        }

        public List<Job> GetJobs(int topJobs)
        {
            var maxPollId = _repo.GetMaxItemId("job");

            var task = _repo.GetRangeItemRecords("job", topJobs, maxPollId, true);
            task.Wait();
            var itemRecords = task.Result;

            return TransformItemToJob(itemRecords);
        }

        public List<Job> GetJobs(List<int> pollIds)
        {
            var task = _repo.GetRangeItemRecords(pollIds);
            task.Wait();

            var itemsRecords = task.Result;

            return TransformItemToJob(itemsRecords);
        }

        public List<Job> SearchJobs(string by, string time, string text, string url, string score,  string title)
        {
            var criteria = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(by))
            {
                criteria.Add("By", by);
            }

            if (!string.IsNullOrWhiteSpace(time))
            {
                if (DateTime.TryParse(time, out DateTime criteriaTime))
                {
                    criteria.Add("Time", criteriaTime);
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                criteria.Add("Text", text);
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                criteria.Add("Url", url);
            }

            if (!string.IsNullOrWhiteSpace(score))
            {
                if (Int32.TryParse(time, out int criteriaScore))
                {
                    criteria.Add("Score", criteriaScore);
                }
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                criteria.Add("Title", title);
            }

            var itemRecords = _repo.SearchItemRecords("job", criteria);

            return TransformItemToJob(itemRecords);
        }

        //public List<Update> GetUpdates(int topUpdates)
        //{
        //    var update = new List<Update>();

        //    return update.ToList();
        //}

        //public List<Update> GetUpdates(List<int> updatesIds)
        //{
        //    var update = new List<Update>();

        //    return update.ToList();
        //}

        //public List<User> GetUsers(int topUsers)
        //{
        //    var users = new List<User>();

        //    return users.ToList();
        //}

        //public List<User> GetUsers(List<string> userIds)
        //{
        //    var users = new List<User>();

        //    return users.ToList();
        //}

        private List<Story> TransformItemToStory(List<ItemRecord> records, int commentLevels)
        {
            return records.Select(r => new Story()
            {
                Id = r.Id,
                By = r.By,
                Kids = GetChildComments(r.Kids, commentLevels, 0)?.ToArray(),
                Score = r.Score,
                Time = r.Time,
                Url = r.Url
            }).ToList();
        }
        
        private List<Comment> TransformItemToComment(List<ItemRecord> records, int commentLevels, int currentCommentLevel)
        {
            return records.Select(r => new Comment()
            {
                Id = r.Id,
                By = r.By,
                Kids = GetChildComments(r.Kids, commentLevels, currentCommentLevel)?.ToArray(),
                ParentComment = GetComment(r.Parent),
                ParentStory = GetStory(r.Parent),
                Text = r.Text,
                Time = r.Time
            }).ToList();
        }

        private List<Poll> TransformItemToPoll(List<ItemRecord> records, int commentLevels)
        {
            return records.Select(r => new Poll()
            {
                Id = r.Id,
                By = r.By,
                Kids = GetChildComments(r.Kids, commentLevels, 0)?.ToArray(),
                Descendant = r.Kids.Count(),
                Score = r.Score,
                Text = r.Text,
                Time = r.Time,
                Title = r.Title,
                Parts = GetPollOptions(r.Parts).ToArray()
            }).ToList();
        }

        private List<Job> TransformItemToJob(List<ItemRecord> records)
        {
            return records.Select(r => new Job()
            {
                Id = r.Id,
                By = r.By,
                Score = r.Score,
                Text = r.Text,
                Time = r.Time,
                Title = r.Title,
                Url = r.Url
            }).ToList();
        }

        private List<PollOption> TransformItemToPollOption(List<ItemRecord> records)
        {
            return records.Select(r => new PollOption()
            {
                Id = r.Id,
                By = r.By,
                Score = r.Score,
                Text = r.Text,
                Time = r.Time,
                Poll = GetPoll(r.Poll)
            }).ToList();
        }

        private List<Comment> GetChildComments(string kidsText, int commentLevels, int currentCommentLevel)
        {
            var childComments = new List<Comment>();

            if (currentCommentLevel + 1 > commentLevels)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(kidsText))
            {
                var kidTextIds = kidsText.Split(new[] { ',' }, StringSplitOptions.None);
                var kidIds = kidTextIds.Select(kti => Convert.ToInt32(kti)).ToList();

                var task = _repo.GetRangeItemRecords(kidIds);
                task.Wait();

                var itemRecords = task.Result;

                return TransformItemToComment(itemRecords, commentLevels, currentCommentLevel + 1).ToList();
            }

            return null;
        }

        private List<PollOption> GetPollOptions(string partsText)
        {
            var pollOptions = new List<PollOption>();

            if (!string.IsNullOrWhiteSpace(partsText))
            {
                var partsTextIds = partsText.Split(new[] { ',' }, StringSplitOptions.None);
                var partsIds = partsTextIds.Select(pti => Convert.ToInt32(pti)).ToList();

                var task = _repo.GetRangeItemRecords(partsIds);
                task.Wait();

                var itemRecords = task.Result;

                return TransformItemToPollOption(itemRecords).ToList();
            }

            return null;
        }

        private Story GetStory(int itemId)
        {
            var itemRecord = _repo.GetItem("story", itemId);

            if (itemRecord != null)
            {
                return new Story()
                {
                    Id = itemRecord.Id,
                    By = itemRecord.By,
                    Score = itemRecord.Score,
                    Time = itemRecord.Time,
                    Url = itemRecord.Url
                };
            }

            return null;
        }

        private Comment GetComment(int itemId)
        {
            var itemRecord = _repo.GetItem("comment", itemId);

            if (itemRecord != null)
            {
                return new Comment()
                {
                    Id = itemRecord.Id,
                    By = itemRecord.By,
                    ParentComment = GetComment(itemRecord.Id),
                    ParentStory = GetStory(itemRecord.Id),
                    Text = itemRecord.Text,
                    Time = itemRecord.Time
                };
            }

            return null;
        }

        private Poll GetPoll(int itemId)
        {
            var itemRecord = _repo.GetItem("poll", itemId);

            if (itemRecord != null)
            {
                return new Poll()
                {
                    Id = itemRecord.Id,
                    By = itemRecord.By,
                    Descendant = itemRecord.Kids.Count(),
                    Text = itemRecord.Text,
                    Time = itemRecord.Time,
                    Parts = GetPollOptions(itemRecord.Parts).ToArray()
                };
            }

            return null;
        }
        
        #endregion
    }
}
