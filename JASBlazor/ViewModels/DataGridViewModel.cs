using JASBlazor.Data;
using JASBlazor.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

using JASBlazor.Data;

using Microsoft.EntityFrameworkCore;

namespace JASBlazor.ViewModels
{
    public class DataGridViewModel : BaseViewModel, IDataGridViewModel
    {
        public IDbContextFactory<JASDBContext> Factory { get; set; }

        public void Initialize(IDbContextFactory<JASDBContext> factory)
        {
            Factory = factory;
            FetchIssues();
        }

        private ObservableCollection<Issue> _data = new ObservableCollection<Issue>();

        public ObservableCollection<Issue> Data
        {
            get => _data;
            private set
            {
                SetValue(ref _data, value);
            }
        }

        private List<Issue> _issueList = new List<Issue>();

        public List<Issue> IsssueList
        {
            get => _issueList;
            private set
            {
                SetValue(ref _issueList, value);
            }
        }

        private Issue _issue = new Issue();

        public Issue issue
        {
            get => _issue;
            set
            {
                SetValue(ref _issue, value);
            }
        }

        public int DataItems
        {
            get
            {
                return IsssueList.Where(i => i.Done.Equals(false)).Count();
            }
        }

        public async void FetchIssues()
        {
            IsBusy = true;
            using (var _context = Factory.CreateDbContext())
            {
                IsssueList = await _context.Issues.ToListAsync();
                Debug.WriteLine(" Fetching Issues: " + IsssueList.Count());
                Data = new ObservableCollection<Issue>(IsssueList);
                OnPropertyChanged(nameof(Data));
                IsBusy = false;
            }
        }

        public async void DeleteRandomIssue()
        {
        }


        /// <summary>
        /// Creates a new user from a seeder if not supplied
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async void CreateIssue(GridCommandEventArgs args)
        {
            Issue issue = args.Item as Issue;

            if (issue == null) { return; }

            IsBusy = true;

            try
            {
                using var Context = Factory.CreateDbContext();
                await Context.Issues.AddAsync(issue);
                await Context.SaveChangesAsync();
                Data.Add(issue);
                OnPropertyChanged(nameof(Data));
                IsBusy = false;
            }
            catch (DBConcurrencyException err) { }
            finally { IsBusy = false; }
        }

        /// <summary>
        /// Deletes the suppplied param if present, from the database
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async void DeleteIssue(GridCommandEventArgs args)
        {
            IsBusy = true;
            Issue issue = args.Item as Issue;
            if (issue == null)
            {
                return;
            }
            try
            {
                using var Context = Factory.CreateDbContext();
                var foundIssue = await Context.Issues.FindAsync(issue.Id);
                if (foundIssue != null)
                {
                    Context.Issues.Remove(foundIssue);
                    await Context.SaveChangesAsync();
                    Data.Remove(issue);
                    OnPropertyChanged(nameof(Data));
                }
            }
            catch (DBConcurrencyException err)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Update the supplied param
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async void UpdateIssue(GridCommandEventArgs args)
        {
            IsBusy = true;
            Issue issue = args.Item as Issue;
            if (issue == null)
            {
                return;
            }
            try
            {
                using var Context = Factory.CreateDbContext();
                Context.Issues.Update(issue);
                await Context.SaveChangesAsync();

                OnPropertyChanged(nameof(Data));
                IsBusy = false;
            }
            catch (DBConcurrencyException err) { }
            finally { IsBusy = false; }
        }
    }
}