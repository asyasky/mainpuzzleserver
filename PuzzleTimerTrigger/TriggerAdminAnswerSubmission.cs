using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PuzzleTimerTrigger
{
    public class TriggerAdminAnswerSubmission
    {
        private Dictionary<int, SubmissionInfo> _submissions = new Dictionary<int, SubmissionInfo>
        {
            // Assign submissions to players
            { 7309, new SubmissionInfo{ AnswerText = "MIDWAY", PlayerId=13838, PuzzleId=7309, TeamId = 5423} },
            { 7313, new SubmissionInfo{ AnswerText = "MONTAUK", PlayerId=13838, PuzzleId=7313, TeamId = 5423} },
            { 7248, new SubmissionInfo{ AnswerText = "SEPHORA", PlayerId=13838, PuzzleId=7248, TeamId = 5423} },
            { 7246, new SubmissionInfo{ AnswerText = "NARCISSUS", PlayerId=14247, PuzzleId=7246, TeamId = 5423} },
            { 7312, new SubmissionInfo{ AnswerText = "PARADOX", PlayerId=14247, PuzzleId=7312, TeamId = 5423} },
            { 7267, new SubmissionInfo{ AnswerText = "VIRGINIA", PlayerId=14247, PuzzleId=7267, TeamId = 5423} },
            { 7268, new SubmissionInfo{ AnswerText = "EUROPA", PlayerId=17826, PuzzleId=7268, TeamId = 5423} },
            { 7276, new SubmissionInfo{ AnswerText = "MANTICORE", PlayerId=17826, PuzzleId=7276, TeamId = 5423} },
            { 7299, new SubmissionInfo{ AnswerText = "NOVEMBER", PlayerId=17826, PuzzleId=7299, TeamId = 5423} },
            { 7306, new SubmissionInfo{ AnswerText = "OUTREACH", PlayerId=17827, PuzzleId=7306, TeamId = 5423} },
            { 7278, new SubmissionInfo{ AnswerText = "POWELL", PlayerId=17827, PuzzleId=7278, TeamId = 5423} },
            { 7298, new SubmissionInfo{ AnswerText = "THESEUS", PlayerId=17827, PuzzleId=7298, TeamId = 5423} },
            { 7305, new SubmissionInfo{ AnswerText = "LEVIATHAN", PlayerId=13856, PuzzleId=7305, TeamId = 5423} },
            { 7303, new SubmissionInfo{ AnswerText = "MONTEVIDEO", PlayerId=13856, PuzzleId=7303, TeamId = 5423} },
            { 7325, new SubmissionInfo{ AnswerText = "RESOLUTE", PlayerId=13856, PuzzleId=7325, TeamId = 5423} },
            { 7249, new SubmissionInfo{ AnswerText = "ALLEGIANCE", PlayerId=9210, PuzzleId=7249, TeamId = 5423} },
            { 7250, new SubmissionInfo{ AnswerText = "DELILAH", PlayerId=9210, PuzzleId=7250, TeamId = 5423} },
            { 7269, new SubmissionInfo{ AnswerText = "VICTORY", PlayerId=9210, PuzzleId=7269, TeamId = 5423} },
            { 7245, new SubmissionInfo{ AnswerText = "BOREAS", PlayerId=1741, PuzzleId=7245, TeamId = 5423} },
            { 7311, new SubmissionInfo{ AnswerText = "ELVIK", PlayerId=1741, PuzzleId=7311, TeamId = 5423} },
            { 7301, new SubmissionInfo{ AnswerText = "EREBUS", PlayerId=1741, PuzzleId=7301, TeamId = 5423} },
            { 7275, new SubmissionInfo{ AnswerText = "AMERICAN", PlayerId=17841, PuzzleId=7275, TeamId = 5423} },
            { 7297, new SubmissionInfo{ AnswerText = "AVARICE", PlayerId=17841, PuzzleId=7297, TeamId = 5423} },
            { 7308, new SubmissionInfo{ AnswerText = "MELVILLE", PlayerId=17841, PuzzleId=7308, TeamId = 5423} },
            { 7270, new SubmissionInfo{ AnswerText = "ESMERELDA", PlayerId=17803, PuzzleId=7270, TeamId = 5423} },
            { 7300, new SubmissionInfo{ AnswerText = "GATEWAY", PlayerId=17803, PuzzleId=7300, TeamId = 5423} },
            { 7322, new SubmissionInfo{ AnswerText = "TYPHOON", PlayerId=17803, PuzzleId=7322, TeamId = 5423} },
            { 7247, new SubmissionInfo{ AnswerText = "DAHLIA", PlayerId=9210, PuzzleId=7247, TeamId = 5423} },
            { 7271, new SubmissionInfo{ AnswerText = "MIRANDA", PlayerId=13838, PuzzleId=7271, TeamId = 5423} },
            { 7272, new SubmissionInfo{ AnswerText = "SHERIDAN", PlayerId=14247, PuzzleId=7272, TeamId = 5423} },
            { 7323, new SubmissionInfo{ AnswerText = "EMERSON", PlayerId=17826, PuzzleId=7323, TeamId = 5423} },
            { 7277, new SubmissionInfo{ AnswerText = "GARDENIA", PlayerId=17827, PuzzleId=7277, TeamId = 5423} },
            { 7321, new SubmissionInfo{ AnswerText = "SNARK", PlayerId=13856, PuzzleId=7321, TeamId = 5423} },
            { 7238, new SubmissionInfo{ AnswerText = "ARCHIMEDES", PlayerId=9210, PuzzleId=7238, TeamId = 5423} },
            { 7273, new SubmissionInfo{ AnswerText = "JUNKET", PlayerId=17841, PuzzleId=7273, TeamId = 5423} },
            { 7324, new SubmissionInfo{ AnswerText = "SAMSON", PlayerId=17803, PuzzleId=7324, TeamId = 5423} },
            { 476, new SubmissionInfo{ AnswerText = "MIDWAY", PlayerId=476, PuzzleId=7309, TeamId=5422} },
            { 476, new SubmissionInfo{ AnswerText = "MONTAUK", PlayerId=476, PuzzleId=7313, TeamId=5422} },
            { 476, new SubmissionInfo{ AnswerText = "SEPHORA", PlayerId=476, PuzzleId=7248, TeamId=5422} },
            { 476, new SubmissionInfo{ AnswerText = "NARCISSUS", PlayerId=476, PuzzleId=7246, TeamId=5422} },
            { 476, new SubmissionInfo{ AnswerText = "PARADOX", PlayerId=476, PuzzleId=7312, TeamId=5422} },
            { 17836, new SubmissionInfo{ AnswerText = "VIRGINIA", PlayerId=17836, PuzzleId=7267, TeamId=5422} },
            { 17836, new SubmissionInfo{ AnswerText = "EUROPA", PlayerId=17836, PuzzleId=7268, TeamId=5422} },
            { 17836, new SubmissionInfo{ AnswerText = "MANTICORE", PlayerId=17836, PuzzleId=7276, TeamId=5422} },
            { 17836, new SubmissionInfo{ AnswerText = "NOVEMBER", PlayerId=17836, PuzzleId=7299, TeamId=5422} },
            { 17837, new SubmissionInfo{ AnswerText = "OUTREACH", PlayerId=17837, PuzzleId=7306, TeamId=5422} },
            { 17837, new SubmissionInfo{ AnswerText = "POWELL", PlayerId=17837, PuzzleId=7278, TeamId=5422} },
            { 17837, new SubmissionInfo{ AnswerText = "THESEUS", PlayerId=17837, PuzzleId=7298, TeamId=5422} },
            { 17837, new SubmissionInfo{ AnswerText = "LEVIATHAN", PlayerId=17837, PuzzleId=7305, TeamId=5422} },
            { 17838, new SubmissionInfo{ AnswerText = "MONTEVIDEO", PlayerId=17838, PuzzleId=7303, TeamId=5422} },
            { 17838, new SubmissionInfo{ AnswerText = "RESOLUTE", PlayerId=17838, PuzzleId=7325, TeamId=5422} },
            { 17838, new SubmissionInfo{ AnswerText = "ALLEGIANCE", PlayerId=17838, PuzzleId=7249, TeamId=5422} },
            { 17838, new SubmissionInfo{ AnswerText = "DELILAH", PlayerId=17838, PuzzleId=7250, TeamId=5422} },
            { 17526, new SubmissionInfo{ AnswerText = "VICTORY", PlayerId=17526, PuzzleId=7269, TeamId=5422} },
            { 7, new SubmissionInfo{ AnswerText = "BOREAS", PlayerId=7, PuzzleId=7245, TeamId=5422} },
            { 7, new SubmissionInfo{ AnswerText = "ELVIK", PlayerId=7, PuzzleId=7311, TeamId=5422} },
            { 7, new SubmissionInfo{ AnswerText = "EREBUS", PlayerId=7, PuzzleId=7301, TeamId=5422} },
            { 17526, new SubmissionInfo{ AnswerText = "AMERICAN", PlayerId=17526, PuzzleId=7275, TeamId=5422} },
            { 17526, new SubmissionInfo{ AnswerText = "AVARICE", PlayerId=17526, PuzzleId=7297, TeamId=5422} },
            { 17526, new SubmissionInfo{ AnswerText = "MELVILLE", PlayerId=17526, PuzzleId=7308, TeamId=5422} },
            { 17840, new SubmissionInfo{ AnswerText = "ESMERELDA", PlayerId=17840, PuzzleId=7270, TeamId=5422} },
            { 17840, new SubmissionInfo{ AnswerText = "GATEWAY", PlayerId=17840, PuzzleId=7300, TeamId=5422} },
            { 17840, new SubmissionInfo{ AnswerText = "TYPHOON", PlayerId=17840, PuzzleId=7322, TeamId=5422} },
            { 17840, new SubmissionInfo{ AnswerText = "DAHLIA", PlayerId=17840, PuzzleId=7247, TeamId=5422} },
            { 17842, new SubmissionInfo{ AnswerText = "MIRANDA", PlayerId=17842, PuzzleId=7271, TeamId=5422} },
            { 17842, new SubmissionInfo{ AnswerText = "SHERIDAN", PlayerId=17842, PuzzleId=7272, TeamId=5422} },
            { 17842, new SubmissionInfo{ AnswerText = "EMERSON", PlayerId=17842, PuzzleId=7323, TeamId=5422} },
            { 17842, new SubmissionInfo{ AnswerText = "GARDENIA", PlayerId=17842, PuzzleId=7277, TeamId=5422} },
            { 17843, new SubmissionInfo{ AnswerText = "SNARK", PlayerId=17843, PuzzleId=7321, TeamId=5422} },
            { 17843, new SubmissionInfo{ AnswerText = "ARCHIMEDES", PlayerId=17843, PuzzleId=7238, TeamId=5422} },
            { 17843, new SubmissionInfo{ AnswerText = "JUNKET", PlayerId=17843, PuzzleId=7273, TeamId=5422} },
            { 17843, new SubmissionInfo{ AnswerText = "SAMSON", PlayerId=17843, PuzzleId=7324, TeamId=5422} },
        };
        static HttpClient HttpClient { get; } = new HttpClient();

        public TriggerAdminAnswerSubmission()
        {
            _submissions = new Dictionary<int, SubmissionInfo>();
        }

        [FunctionName("TriggerAdminAnswerSubmission")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            //https://puzzlehunt.azurewebsites.net
            const string eventId = "pd24beta";
            var response = HttpClient.GetAsync($"http://localhost:44319/api/puzzleapi/state/puzzleunlockstate/{eventId}?minutes=30&timerWindow=20").Result.Content;
            var unlocked = response.ReadFromJsonAsync<List<UnlockDetail>>().Result;

            if (unlocked != null)
            {
                foreach (UnlockDetail unlockedPuzzle in unlocked)
                {
                    //AdminAnswerSubmission submission = new()
                    //{
                    //    AllowFreeformSharing = true,
                    //    EventPassword = "1d5cb3a1-7a73-4c34-b680-6e90c072972c",
                    //    SubmissionText = "time check two",
                    //    SubmitterDisplayName = "DisplayingName",
                    //    Timestamp = DateTime.Now,
                    //};
                    //HttpClient.PostAsJsonAsync("http://localhost:44319/api/puzzleapi/submitanswer/2/19/1", submission).Wait();

                    // Find the matching entry and send the answer submission (using current time & hardcoded values in this version)
                    if(_submissions.ContainsKey(unlockedPuzzle.PuzzleId))
                    {
                        AdminAnswerSubmission submission = new()
                        {
                            AllowFreeformSharing = false,
                            EventPassword = "",
                            SubmissionText = _submissions[unlockedPuzzle.PuzzleId].AnswerText,
                        };

                        HttpClient.PostAsJsonAsync($"https://puzzlehunt.azurewebsites.net/api/puzzleapi/submitanswer/ph24beta/{unlockedPuzzle.PuzzleId}/{_submissions[unlockedPuzzle.PuzzleId]}", submission).Wait();
                    }

                }
            }

            //var unlocked = await client.GetFromJsonAsync<List<UnlockDetail>>("http://localhost:44319/api/puzzleapi/state/puzzleunlockstate/2?minutes=10000");
        }
    }

    public class SubmissionInfo
    {
        public int TeamId { get; set; }
        public int PuzzleId { get; set; }
        public int PlayerId { get; set; }
        public string AnswerText { get; set; }
    }

    public class UnlockDetail
    {
        public int TeamId { get; set; }
        public int PuzzleId { get; set; }
        public DateTime UnlockTime { get; set; }
    }

    public class AdminAnswerSubmission
    {
        public bool AllowFreeformSharing { get; set; }
        public string EventPassword { get; set; }
        public string SubmissionText { get; set; }
        public string SubmitterDisplayName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
