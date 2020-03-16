using System;
using System.Collections.Generic;
using HrNewsPortal.Models;

namespace HrNewsPortal.Services
{
    public interface IHrNewsDataService
    {
        List<Story> GetStories(int topStories, int commentLevels);

        List<Story> GetStories(List<int> storyIds, int commentLevels);

        List<Story> SearchStories(string by, string time, string url, string score, int commentLevels);

        List<Comment> GetComments(int topComments, int commentLevels);

        List<Comment> GetComments(List<int> commentIds, int commentLevels);

        List<Comment> SearchComments(string by, string time, string text, int commentLevels);

        List<Poll> GetPolls(int topPolls, int commentLevels);

        List<Poll> GetPolls(List<int> pollIds, int commentLevels);

        List<Poll> SearchPolls(string by, string time, string text, string score, string title, int commentLevels);

        List<PollOption> GetPollsOptions(List<int> pollOptions);

        List<Job> GetJobs(int topJobs);

        List<Job> GetJobs(List<int> jobIds);

        List<Job> SearchJobs(string by, string time, string text, string url, string score, string title);

        //List<Update> GetUpdates(int topUpdates);

        //List<Update> GetUpdates(List<int> updatesIds);

        //List<User> GetUsers(int topUsers);

        //List<User> GetUsers(List<string> userIds);
    }
}
