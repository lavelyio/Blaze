using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JASBlazor.Models
{
    public enum IssueSeverity
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum IssueType
    {
        Bug = 0,
        Feature = 1,
        Enhancement = 2
    }

    public class Issue
    {
        public Issue()
        {
            Done = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
        public bool IsOpen { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string Description { get; set; }
        public IssueType Type { get; set; }
        public IssueSeverity? Severity { get; set; }

        [NotMapped]
        public bool Done { get; set; }

        [NotMapped]
        public List<string> Labels { get; set; }
    }
}