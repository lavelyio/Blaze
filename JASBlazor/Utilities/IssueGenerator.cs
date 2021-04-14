using JASBlazor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JASBlazor.Utilities
{
    public class IssuesGenerator
    {
        private static Random _rand = new Random();
        private static string _dummyTitle = "Issue lorem ipsum dolor sit amet, consectetur adipiscing elit.";
        private static string[] _issueTypes = { "bug", "feature", "enhancement" };
        private static string[] _severities = { "low", "medium", "high" };
        private static string[] _componentList = { "grid", "button", "window", "chart", "textbox", "numeric textbox", "dropdownlist", "calendar" };

        /// <summary>
        /// Make a new city.
        /// </summary>
        /// <returns>A random <see cref="Cities" /> instance.</returns>
        public Issue MakeIssue()
        {
            var currIssue = new Issue();
            currIssue.Id = _rand.Next(120000);
            currIssue.Title = _dummyTitle.Substring(_rand.Next(5, _dummyTitle.Length)) + _rand.Next(100);
            currIssue.CreatedOn = DateTime.Now;

            int type = _rand.Next(0, 3);
            currIssue.Type = (IssueType)type;

            if (currIssue.Type == IssueType.Bug)
            {
                int sev = _rand.Next(0, 3);
                currIssue.Severity = (IssueSeverity)sev;
            }

            currIssue.Description = "<p style=\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif;\"><strong>Lorem ipsum </strong>dolor sit amet, consectetur adipiscing elit. Nam eget diam et ipsum vulputate porta. Duis non venenatis odio, ut sagittis mi. Nam et pellentesque dolor. Pellentesque ornare neque ac feugiat convallis:</p><ul>	<li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif;\"> Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">In ac eros eget elit laoreet congue vitae vel quam. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Suspendisse potenti. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Fusce vitae magna maximus, ornare turpis quis, porttitor velit. Nam ac condimentum massa, vitae tristique nulla.</li></ul><h5 style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Vestibulum vitae ante egestas, sollicitudin justo a, pulvinar turpis.</h5><p style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \"> Sed at condimentum turpis. Mauris fermentum, felis non euismod sagittis, nisl dui bibendum urna, vel iaculis mi nunc dictum turpis. In sodales at sapien eget pellentesque.</p>";

            return currIssue;
        }

        /// <summary>
        /// Our Stub Issue API which allows for easy testing
        /// </summary>
        /// <param name="timeRange"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Issue>> GetIssuesStub(DateTime timeRange)
        {
            //in a real case this can be asynchronous and would be calling an actual data endpoint. Here, we generate data "randomly"

            DateTime currTime = DateTime.Now;
            int daysToGenerateIssueFor = ((TimeSpan)(currTime - timeRange)).Days;
            List<Issue> issueList = new List<Issue>();
            int issueId = 0;

            for (int i = daysToGenerateIssueFor; i >= 0; i--)
            {
                for (int j = 0; j < _rand.Next(1, 4); j++)
                {
                    Issue currIssue = new Issue();

                    currIssue.Id = ++issueId;
                    currIssue.Title = _dummyTitle.Substring(_rand.Next(5, _dummyTitle.Length)) + currIssue.Id;
                    currIssue.CreatedOn = currTime.AddDays(-i);
                    if (_rand.Next(0, 10) % _rand.Next(1, 4) == 0)
                    {
                        currIssue.ClosedOn = currTime.AddDays(-_rand.Next(0, _rand.Next(0, i)));
                    }

                    int type = _rand.Next(0, 3);
                    currIssue.Type = (IssueType)type;
                    if (currIssue.Type == IssueType.Bug)
                    {
                        int sev = _rand.Next(0, 3);
                        currIssue.Severity = (IssueSeverity)sev;
                    }
                    currIssue.Description = "<p style=\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif;\"><strong>Lorem ipsum </strong>dolor sit amet, consectetur adipiscing elit. Nam eget diam et ipsum vulputate porta. Duis non venenatis odio, ut sagittis mi. Nam et pellentesque dolor. Pellentesque ornare neque ac feugiat convallis:</p><ul>	<li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif;\"> Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">In ac eros eget elit laoreet congue vitae vel quam. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Suspendisse potenti. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Fusce vitae magna maximus, ornare turpis quis, porttitor velit. Nam ac condimentum massa, vitae tristique nulla.</li></ul><h5 style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Vestibulum vitae ante egestas, sollicitudin justo a, pulvinar turpis.</h5><p style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \"> Sed at condimentum turpis. Mauris fermentum, felis non euismod sagittis, nisl dui bibendum urna, vel iaculis mi nunc dictum turpis. In sodales at sapien eget pellentesque.</p>";

                    issueList.Add(currIssue);
                }
            }

            return issueList;
        }
    }
}