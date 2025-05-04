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
            { 5423, new SubmissionInfo{ AnswerText = "MIDWAY", PlayerId="13838", PuzzleId="7309"} },
            { 5423, new SubmissionInfo{ AnswerText = "MONTAUK", PlayerId="13838", PuzzleId="7313"} },
            { 5423, new SubmissionInfo{ AnswerText = "SEPHORA", PlayerId="13838", PuzzleId="7248"} },
            { 5423, new SubmissionInfo{ AnswerText = "NARCISSUS", PlayerId="14247", PuzzleId="7246"} },
            { 5423, new SubmissionInfo{ AnswerText = "PARADOX", PlayerId="14247", PuzzleId="7312"} },
            { 5423, new SubmissionInfo{ AnswerText = "VIRGINIA", PlayerId="14247", PuzzleId="7267"} },
            { 5423, new SubmissionInfo{ AnswerText = "EUROPA", PlayerId="17826", PuzzleId="7268"} },
            { 5423, new SubmissionInfo{ AnswerText = "MANTICORE", PlayerId="17826", PuzzleId="7276"} },
            { 5423, new SubmissionInfo{ AnswerText = "NOVEMBER", PlayerId="17826", PuzzleId="7299"} },
            { 5423, new SubmissionInfo{ AnswerText = "OUTREACH", PlayerId="17827", PuzzleId="7306"} },
            { 5423, new SubmissionInfo{ AnswerText = "POWELL", PlayerId="17827", PuzzleId="7278"} },
            { 5423, new SubmissionInfo{ AnswerText = "THESEUS", PlayerId="17827", PuzzleId="7298"} },
            { 5423, new SubmissionInfo{ AnswerText = "LEVIATHAN", PlayerId="13856", PuzzleId="7305"} },
            { 5423, new SubmissionInfo{ AnswerText = "MONTEVIDEO", PlayerId="13856", PuzzleId="7303"} },
            { 5423, new SubmissionInfo{ AnswerText = "RESOLUTE", PlayerId="13856", PuzzleId="7325"} },
            { 5423, new SubmissionInfo{ AnswerText = "ALLEGIANCE", PlayerId="9210", PuzzleId="7249"} },
            { 5423, new SubmissionInfo{ AnswerText = "DELILAH", PlayerId="9210", PuzzleId="7250"} },
            { 5423, new SubmissionInfo{ AnswerText = "VICTORY", PlayerId="9210", PuzzleId="7269"} },
            { 5423, new SubmissionInfo{ AnswerText = "BOREAS", PlayerId="1741", PuzzleId="7245"} },
            { 5423, new SubmissionInfo{ AnswerText = "ELVIK", PlayerId="1741", PuzzleId="7311"} },
            { 5423, new SubmissionInfo{ AnswerText = "EREBUS", PlayerId="1741", PuzzleId="7301"} },
            { 5423, new SubmissionInfo{ AnswerText = "AMERICAN", PlayerId="17841", PuzzleId="7275"} },
            { 5423, new SubmissionInfo{ AnswerText = "AVARICE", PlayerId="17841", PuzzleId="7297"} },
            { 5423, new SubmissionInfo{ AnswerText = "MELVILLE", PlayerId="17841", PuzzleId="7308"} },
            { 5423, new SubmissionInfo{ AnswerText = "ESMERELDA", PlayerId="17803", PuzzleId="7270"} },
            { 5423, new SubmissionInfo{ AnswerText = "GATEWAY", PlayerId="17803", PuzzleId="7300"} },
            { 5423, new SubmissionInfo{ AnswerText = "TYPHOON", PlayerId="17803", PuzzleId="7322"} },
            { 5423, new SubmissionInfo{ AnswerText = "DAHLIA", PlayerId="9210", PuzzleId="7247"} },
            { 5423, new SubmissionInfo{ AnswerText = "MIRANDA", PlayerId="13838", PuzzleId="7271"} },
            { 5423, new SubmissionInfo{ AnswerText = "SHERIDAN", PlayerId="14247", PuzzleId="7272"} },
            { 5423, new SubmissionInfo{ AnswerText = "EMERSON", PlayerId="17826", PuzzleId="7323"} },
            { 5423, new SubmissionInfo{ AnswerText = "GARDENIA", PlayerId="17827", PuzzleId="7277"} },
            { 5423, new SubmissionInfo{ AnswerText = "SNARK", PlayerId="13856", PuzzleId="7321"} },
            { 5423, new SubmissionInfo{ AnswerText = "ARCHIMEDES", PlayerId="9210", PuzzleId="7238"} },
            { 5423, new SubmissionInfo{ AnswerText = "JUNKET", PlayerId="17841", PuzzleId="7273"} },
            { 5423, new SubmissionInfo{ AnswerText = "SAMSON", PlayerId="17803", PuzzleId="7324"} },
            { 5422, new SubmissionInfo{ AnswerText = "MIDWAY", PlayerId="476", PuzzleId="7309"} },
            { 5422, new SubmissionInfo{ AnswerText = "MONTAUK", PlayerId="476", PuzzleId="7313"} },
            { 5422, new SubmissionInfo{ AnswerText = "SEPHORA", PlayerId="476", PuzzleId="7248"} },
            { 5422, new SubmissionInfo{ AnswerText = "NARCISSUS", PlayerId="476", PuzzleId="7246"} },
            { 5422, new SubmissionInfo{ AnswerText = "PARADOX", PlayerId="476", PuzzleId="7312"} },
            { 5422, new SubmissionInfo{ AnswerText = "VIRGINIA", PlayerId="17836", PuzzleId="7267"} },
            { 5422, new SubmissionInfo{ AnswerText = "EUROPA", PlayerId="17836", PuzzleId="7268"} },
            { 5422, new SubmissionInfo{ AnswerText = "MANTICORE", PlayerId="17836", PuzzleId="7276"} },
            { 5422, new SubmissionInfo{ AnswerText = "NOVEMBER", PlayerId="17836", PuzzleId="7299"} },
            { 5422, new SubmissionInfo{ AnswerText = "OUTREACH", PlayerId="17837", PuzzleId="7306"} },
            { 5422, new SubmissionInfo{ AnswerText = "POWELL", PlayerId="17837", PuzzleId="7278"} },
            { 5422, new SubmissionInfo{ AnswerText = "THESEUS", PlayerId="17837", PuzzleId="7298"} },
            { 5422, new SubmissionInfo{ AnswerText = "LEVIATHAN", PlayerId="17837", PuzzleId="7305"} },
            { 5422, new SubmissionInfo{ AnswerText = "MONTEVIDEO", PlayerId="17838", PuzzleId="7303"} },
            { 5422, new SubmissionInfo{ AnswerText = "RESOLUTE", PlayerId="17838", PuzzleId="7325"} },
            { 5422, new SubmissionInfo{ AnswerText = "ALLEGIANCE", PlayerId="17838", PuzzleId="7249"} },
            { 5422, new SubmissionInfo{ AnswerText = "DELILAH", PlayerId="17838", PuzzleId="7250"} },
            { 5422, new SubmissionInfo{ AnswerText = "VICTORY", PlayerId="17526", PuzzleId="7269"} },
            { 5422, new SubmissionInfo{ AnswerText = "BOREAS", PlayerId="7", PuzzleId="7245"} },
            { 5422, new SubmissionInfo{ AnswerText = "ELVIK", PlayerId="7", PuzzleId="7311"} },
            { 5422, new SubmissionInfo{ AnswerText = "EREBUS", PlayerId="7", PuzzleId="7301"} },
            { 5422, new SubmissionInfo{ AnswerText = "AMERICAN", PlayerId="17526", PuzzleId="7275"} },
            { 5422, new SubmissionInfo{ AnswerText = "AVARICE", PlayerId="17526", PuzzleId="7297"} },
            { 5422, new SubmissionInfo{ AnswerText = "MELVILLE", PlayerId="17526", PuzzleId="7308"} },
            { 5422, new SubmissionInfo{ AnswerText = "ESMERELDA", PlayerId="17840", PuzzleId="7270"} },
            { 5422, new SubmissionInfo{ AnswerText = "GATEWAY", PlayerId="17840", PuzzleId="7300"} },
            { 5422, new SubmissionInfo{ AnswerText = "TYPHOON", PlayerId="17840", PuzzleId="7322"} },
            { 5422, new SubmissionInfo{ AnswerText = "DAHLIA", PlayerId="17840", PuzzleId="7247"} },
            { 5422, new SubmissionInfo{ AnswerText = "MIRANDA", PlayerId="17842", PuzzleId="7271"} },
            { 5422, new SubmissionInfo{ AnswerText = "SHERIDAN", PlayerId="17842", PuzzleId="7272"} },
            { 5422, new SubmissionInfo{ AnswerText = "EMERSON", PlayerId="17842", PuzzleId="7323"} },
            { 5422, new SubmissionInfo{ AnswerText = "GARDENIA", PlayerId="17842", PuzzleId="7277"} },
            { 5422, new SubmissionInfo{ AnswerText = "SNARK", PlayerId="17843", PuzzleId="7321"} },
            { 5422, new SubmissionInfo{ AnswerText = "ARCHIMEDES", PlayerId="17843", PuzzleId="7238"} },
            { 5422, new SubmissionInfo{ AnswerText = "JUNKET", PlayerId="17843", PuzzleId="7273"} },
            { 5422, new SubmissionInfo{ AnswerText = "SAMSON", PlayerId="17843", PuzzleId="7324"} },
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
            const string eventId = "2";
            var response = HttpClient.GetAsync($"http://localhost:44319/api/puzzleapi/state/puzzleunlockstate/{eventId}?minutes=30&timerWindow=20").Result.Content;
            var unlocked = response.ReadFromJsonAsync<List<UnlockDetail>>().Result;

            if (unlocked != null)
            {
                foreach (UnlockDetail unlockedPuzzle in unlocked)
                {
                    AdminAnswerSubmission submission = new()
                    {
                        AllowFreeformSharing = true,
                        EventPassword = "1d5cb3a1-7a73-4c34-b680-6e90c072972c",
                        SubmissionText = "time check two",
                        SubmitterDisplayName = "DisplayingName",
                        Timestamp = DateTime.Now,
                    };
                    HttpClient.PostAsJsonAsync("http://localhost:44319/api/puzzleapi/submitanswer/2/19/1", submission).Wait();

                    // Find the matching entry and send the answer submission (using current time & hardcoded values in this version)
                    if(_submissions.ContainsKey(unlockedPuzzle.PuzzleId))
                    {

                    }

                }
            }

            //var unlocked = await client.GetFromJsonAsync<List<UnlockDetail>>("http://localhost:44319/api/puzzleapi/state/puzzleunlockstate/2?minutes=10000");
        }
    }

    public class SubmissionInfo
    {
        public string PuzzleId { get; set; }
        public string PlayerId { get; set; }
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
