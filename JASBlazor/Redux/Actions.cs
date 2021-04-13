using Blazor.Realm;
using Blazor.Realm.Async;
using JASBlazor.Data;
using JASBlazor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace JASBlazor.Redux.Actions
{
    namespace Counter
    {
        public class Decrement : IRealmAction
        {
            public Decrement(int value)
            {
                Value = value;
            }

            public int Value { get; set; }
        }

        public class Dispose : Reset { }

        public class Increment : IRealmAction
        {
            public Increment(int value)
            {
                Value = value;
            }

            public int Value { get; set; }
        }

        public class Reset : IRealmAction { }
    }

    namespace IsLoading
    {
        public class Set : IRealmAction
        {
            public Set(bool value)
            {
                Value = value;
            }

            public bool Value { get; set; }
        }
    }

    namespace Issues
    {
        public class Clear : IRealmAction { }

        /// <summary>
        /// ///
        /// </summary>
        public class CreateIssueAsync : IAsyncRealmAction
        {
            /// <summary>
            /// Creates a new Issue Async
            /// </summary>
            /// <param name="store"></param>
            /// <param name="factory"></param>
            /// <param name="args"></param>
            public CreateIssueAsync(Store<AppState> store, IDbContextFactory<JASDBContext> factory, GridCommandEventArgs args)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Factory = factory ?? throw new ArgumentNullException(nameof(factory));
                issue = args.Item as Issue;
            }

            public IDbContextFactory<JASDBContext> Factory { get; set; }
            public Issue issue { get; set; }
            public Store<AppState> Store { get; set; }

            public async Task Invoke()
            {
                Debug.WriteLine("Creating new Issue: ");
                Store.Dispatch(new IsLoading.Set(true));
                Store.Dispatch(new Issues.Clear());

                await Task.Delay(500);
                using var _context = Factory.CreateDbContext();
                await _context.Issues.AddAsync(issue);
                await _context.SaveChangesAsync();
                var issues = await _context.Issues.ToListAsync();

                Store.Dispatch(new Set(new ObservableCollection<Issue>(issues)));
                Store.Dispatch(new IsLoading.Set(false));
            }
        }

        /// <summary>
        /// Delete and Issue from the database
        /// </summary>
        public class DeleteIssueAsync : IAsyncRealmAction
        {
            /// <summary>
            /// Deletes an Issue from the database Async
            /// </summary>
            /// <param name="store"></param>
            /// <param name="factory"></param>
            /// <param name="args"></param>
            public DeleteIssueAsync(Store<AppState> store, IDbContextFactory<JASDBContext> factory, GridCommandEventArgs args)
            {
                Debug.WriteLine("Init DeleteIssue Async");
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Factory = factory;
                Issue = args.Item as Issue;
            }

            public IDbContextFactory<JASDBContext> Factory { get; set; }
            public Issue Issue { get; set; }
            public Store<AppState> Store { get; set; }

            public async Task Invoke()
            {
                Debug.WriteLine("Invoking Delete Action");
                Store.Dispatch(new IsLoading.Set(true));

                using var _context = Factory.CreateDbContext();
                var foundIssue = await _context.Issues.FindAsync(Issue.Id);
                if (foundIssue != null)
                {
                    _context.Issues.Remove(foundIssue);
                    await _context.SaveChangesAsync();
                    var issues = await _context.Issues.ToListAsync();
                    Store.Dispatch(new Issues.Clear());
                    Store.Dispatch(new Issues.Set(new ObservableCollection<Issue>(issues)));
                }

                Store.Dispatch(new IsLoading.Set(false));
            }
        }

        /// <summary>
        /// ///
        /// </summary>
        public class FetchAsync : IAsyncRealmAction
        {
            public FetchAsync(Store<AppState> store, IDbContextFactory<JASDBContext> factory)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Factory = factory;
            }

            public IDbContextFactory<JASDBContext> Factory { get; set; }
            public Store<AppState> Store { get; set; }

            public async Task Invoke()
            {
                Store.Dispatch(new IsLoading.Set(true));
                Store.Dispatch(new Issues.Clear());

                using var _context = Factory.CreateDbContext();
                var _issues = await _context.Issues.ToListAsync();
                var issues = new ObservableCollection<Issue>(_issues);

                Store.Dispatch(new Issues.Set(issues));
                Store.Dispatch(new IsLoading.Set(false));
            }
        }

        public class Set : IRealmAction
        {
            public Set(ObservableCollection<Issue> issues)
            {
                Data = issues;
            }

            public ObservableCollection<Issue> Data { get; set; }
        }

        /// <summary>
        /// ///
        /// </summary>
        public class UpdateIssueAsync : IAsyncRealmAction
        {
            /// <summary>
            /// Updates an issue Async
            /// </summary>
            /// <param name="store"></param>
            /// <param name="factory"></param>
            /// <param name="args"></param>
            public UpdateIssueAsync(Store<AppState> store, IDbContextFactory<JASDBContext> factory, GridCommandEventArgs args)
            {
                Store = store ?? throw new ArgumentNullException(nameof(store));
                Factory = factory ?? throw new ArgumentNullException(nameof(factory));
                _issue = args.Item as Issue;
            }

            public IDbContextFactory<JASDBContext> Factory { get; set; }
            public Store<AppState> Store { get; set; }
            private Issue _issue { get; set; }

            public async Task Invoke()
            {
                Store.Dispatch(new IsLoading.Set(true));

                try
                {
                    using var _context = Factory.CreateDbContext();
                    var issueToUpdate = await _context.Issues.FindAsync(_issue.Id);

                    if (issueToUpdate != null)
                    {
                        /*
                            To Avoid Concurrency Exceptions, updating the tracked entity
                                directly saves us headacheand it updates the tracking for us.
                            Reference: https://docs.microsoft.com/en-us/ef/ef6/saving/change-tracking/property-values
                         */
                        _context.Entry(issueToUpdate).CurrentValues.SetValues(_issue);

                        await _context.SaveChangesAsync();
                        Store.Dispatch(new Issues.Clear());

                        var issues = await _context.Issues.ToListAsync();
                        Store.Dispatch(new Issues.Set(new ObservableCollection<Issue>(issues)));
                    }

                    Store.Dispatch(new IsLoading.Set(false));
                }
                catch (Exception ex)
                {
                    Debug.Write(ex);
                    throw;
                }
                finally
                {
                    Store.Dispatch(new IsLoading.Set(false));
                }
            }
        }
    }
}