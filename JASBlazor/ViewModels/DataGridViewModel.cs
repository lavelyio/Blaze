using JASBlazor.Data;
using JASBlazor.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace JASBlazor.ViewModels {

    public class DataGridViewModel : BaseViewModel, IDataGridViewModel {

        public JASDBContext Context { get; set; }

        public async Task InitializeAsync(JASDBContext context) {
            Context = context;
            await FetchIssues();
        }

        private ObservableCollection<Issue> _data = new ObservableCollection<Issue>();
        public ObservableCollection<Issue> Data {
            get => _data;
            private set {
                SetValue(ref _data, value);
            }
        }

        private IEnumerable<Issue> _issues;
        public IEnumerable<Issue> DataIssues {
            get => _issues;
            private set {
                SetValue(ref _issues, value);
            }
        }

        private List<Issue> _issueList = new List<Issue>();
        public List<Issue> IsssueList {
            get => _issueList;
            private set {
                SetValue(ref _issueList, value);
            }

        }

        private Issue _issue = new Issue();
        public Issue issue {
            get => _issue;
            set {
                SetValue(ref _issue, value);
            }
        }

        public int DataItems {
            get {
                return IsssueList.Where(i => i.Done.Equals(false)).Count();
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <returns></returns>
        public async Task FetchIssues()
        {
            IsBusy = true;

            try {
                IsssueList = await Context.Issues.ToListAsync();
                Debug.WriteLine(" Fetching Issues: " + IsssueList.Count());

                Data = new ObservableCollection<Issue>(IsssueList);
                OnPropertyChanged(nameof(Data));
            }
            catch (Exception ex) {
                throw;
            }
            finally {
                IsBusy = false;
            }
        }


        /// <summary>
        /// Creates a new user from a seeder if not supplied
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async void CreateIssue(Issue issue)
        {

            if (issue == null) { return; }

            IsBusy = true;

            try {
                Context.Issues.Add(issue);
                await Context.SaveChangesAsync();

                Data.Add(issue);
                OnPropertyChanged(nameof(Data));
            }
            catch (DbUpdateConcurrencyException ex) {

                // Catch Exception, reload the conflicting issue from
                //  the database; refreshing EFCore's cache
                ex.Entries.Single().Reload();

                // Save once and for all
                Context.SaveChanges();

            }
            finally { IsBusy = false; }
        }

        /// <summary>
        /// Deletes the suppplied param if present, from the database
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async void DeleteIssue(Issue issue)
        {
            
            if (issue == null) {
                return;
            }
            IsBusy = true;

            try {

                Context.Issues.Remove(issue);
                await Context.SaveChangesAsync();
                Data.Remove(issue);
                OnPropertyChanged(nameof(Data));
            }
            catch (DbUpdateConcurrencyException ex) {

                // Catch Exception, reload the conflicting issue from
                //  the database; refreshing EFCore's cache
                ex.Entries.Single().Reload();

                // Save once and for all
                Context.SaveChanges();

            }
            finally { IsBusy = false; }
        }

        /// <summary>
        /// Update the supplied param
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async void UpdateIssue(Issue issue)
        {
            if (issue == null) {
                return;
            }

            IsBusy = true;

            try {

                Context.Issues.Update(issue);
                await Context.SaveChangesAsync();

                OnPropertyChanged(nameof(Data));
            }
            catch (DbUpdateConcurrencyException ex) {

                // Catch Exception, reload the conflicting issue from
                //  the database; refreshing EFCore's cache
                ex.Entries.Single().Reload();

                // Save once and for all
                Context.SaveChanges();

            }
            finally { IsBusy = false; }
        }

        /// <summary>
        /// Update the tracked Entity, essentially "releasing" it from it's
        ///  changed state
        /// </summary>
        /// <param name="issue"></param>
        public void ReleaseChanges(Issue issue) {
            
            var issueEntry = Context.Entry(issue);
            if (issueEntry.State == EntityState.Modified) {
                issueEntry.CurrentValues.SetValues(issueEntry.OriginalValues);
                issueEntry.State = EntityState.Unchanged;
            }
        }
    }
}