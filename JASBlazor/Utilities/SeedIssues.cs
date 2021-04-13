using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using JASBlazor.Data;
using JASBlazor.Models;

namespace JASBlazor.Utilities
{


    /// <summary>
    /// Generates our test issues
    /// </summary>
    public class SeedIssues
    {

        /// <summary>
        /// Issue Title
        /// </summary>
        private static string _dummyTitle = "Issue lorem ipsum dolor sit amet, consectetur adipiscing elit.";


        /// <summary>
        /// Types of Issue
        /// </summary>
        private static string[] _issueTypes = { "bug", "feature", "enhancement" };

        /// <summary>
        /// Issue Severities
        /// </summary>
        private static string[] _severities = { "low", "medium", "high" };

        /// <summary>
        /// Components list
        /// </summary>
        private static string[] _componentList = { "grid", "button", "window", "chart", "textbox", "numeric textbox", "dropdownlist", "calendar" };

        /// <summary>
        /// Get some randominzation in play.
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Picks a random item from a list.
        /// </summary>
        /// <param name="list">A list of <c>string</c> to parse.</param>
        /// <returns>A single item from the list.</returns>
        private string RandomOne(string[] list)
        {
            var idx = _random.Next(list.Length - 1);
            return list[idx];
        }
      

        public async Task SeedIssuesAsync(JASDBContext context)
        {

            DateTime currTime = DateTime.Now;
            int daysToGenerateIssueFor = 30;
            List<Issue> issueList = new List<Issue>();


            for (int i = daysToGenerateIssueFor; i >= 0; i--)
            {
                for (int j = 0; j < _random.Next(1, 4); j++)
                {
                    Issue currIssue = new Issue();

                    currIssue.Title = _dummyTitle.Substring(_random.Next(5, _dummyTitle.Length)) + currIssue.Id;
                    currIssue.CreatedOn = currTime.AddDays(-i);
                    if (_random.Next(0, 10) % _random.Next(1, 4) == 0)
                    {
                        currIssue.ClosedOn = currTime.AddDays(-_random.Next(0, _random.Next(0, i)));
                    }

                    int type = _random.Next(0, 3);
                    currIssue.Type = (IssueType)type;
                    if (currIssue.Type == IssueType.Bug)
                    {
                        int sev = _random.Next(0, 3);
                        currIssue.Severity = (IssueSeverity)sev;
                    }

                    currIssue.Description = "<p style=\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif;\"><strong>Lorem ipsum </strong>dolor sit amet, consectetur adipiscing elit. Nam eget diam et ipsum vulputate porta. Duis non venenatis odio, ut sagittis mi. Nam et pellentesque dolor. Pellentesque ornare neque ac feugiat convallis:</p><ul>	<li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif;\"> Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">In ac eros eget elit laoreet congue vitae vel quam. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Suspendisse potenti. </li><li style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Fusce vitae magna maximus, ornare turpis quis, porttitor velit. Nam ac condimentum massa, vitae tristique nulla.</li></ul><h5 style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \">Vestibulum vitae ante egestas, sollicitudin justo a, pulvinar turpis.</h5><p style =\"margin: 0px 0px 15px; padding: 0px; text-align: justify; font-family: 'Open Sans', Arial, sans-serif; \"> Sed at condimentum turpis. Mauris fermentum, felis non euismod sagittis, nisl dui bibendum urna, vel iaculis mi nunc dictum turpis. In sodales at sapien eget pellentesque.</p>";

                    issueList.Add(currIssue);
                }
            }

            if (issueList.Count > 0)
            {
                context.Issues.AddRange(issueList);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DBConcurrencyException err) { } // dump it

            }
        }
    }

}
