using JASBlazor.Data;
using JASBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace JASBlazor.ViewModels {

    public interface IDataGridViewModel {

        public IDbContextFactory<JASDBContext> Factory { get; set; }
        bool IsBusy { get; set; }
        int DataItems { get; }
        Issue issue { get; set; }
        List<Issue> IsssueList { get; }

        ObservableCollection<Issue> Data { get; }

        event PropertyChangedEventHandler PropertyChanged;
        void Initialize(IDbContextFactory<JASDBContext> factory);
        void FetchIssues();

        void CreateIssue(GridCommandEventArgs args);

        void UpdateIssue(GridCommandEventArgs args);

        void DeleteIssue(GridCommandEventArgs args);

        void DeleteRandomIssue();
    }
}
