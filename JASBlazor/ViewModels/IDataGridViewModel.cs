using JASBlazor.Data;
using JASBlazor.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace JASBlazor.ViewModels
{
    public interface IDataGridViewModel
    {
        public JASDBContext Context { get; set; }
        bool IsBusy { get; set; }
        int DataItems { get; }
        Issue issue { get; set; }
        List<Issue> IsssueList { get; }

        ObservableCollection<Issue> Data { get; }
        IEnumerable<Issue> DataIssues { get; }

        event PropertyChangedEventHandler PropertyChanged;

        Task InitializeAsync(JASDBContext context);

        Task FetchIssues();

        void CreateIssue(Issue issue);

        void UpdateIssue(Issue issue);

        void DeleteIssue(Issue issue);

        void ReleaseChanges(Issue issue);
    }
}