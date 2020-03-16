using System.Collections.Generic;
using HrNewsPortal.Models;

namespace HrNewsPortal.Services
{
    public interface IHrNewsDataService
    {
        Story GetStory(int itemId);

        List<Story> GetStories(int topStories);

        List<Story> GetStories(List<int> storyIds);

        List<Story> SearchStories(string by, string time, string url, string score);

        Comment GetComment(int itemId);

        List<Comment> GetComments(int topComments);

        List<Comment> GetComments(List<int> commentIds);

        List<Comment> SearchComments(string by, string time, string text);

        List<Poll> GetPolls(int topPolls);

        Poll GetPoll(int itemId);

        List<Poll> GetPolls(List<int> pollIds);

        List<Poll> SearchPolls(string by, string time, string text, string score, string title);

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
