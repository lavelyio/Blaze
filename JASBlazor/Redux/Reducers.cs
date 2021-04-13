using Blazor.Realm;
using JASBlazor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace JASBlazor.Redux
{
    public static class Reducers
    {
        public static AppState RootReducer(AppState appState, IRealmAction action)
        {
            if (appState == null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            return new AppState
            {
                IsLoading = IsLoadingReducer(appState.IsLoading, action),
                Count = CounterReducer(appState.Count, action),
                Issues = IssuesReducer(appState.Issues, action)
            };
        }

        public static int CounterReducer(int count, IRealmAction action)
        {
            switch (action)
            {
                case Actions.Counter.Increment:
                    return count + 1;
                case Actions.Counter.Decrement:
                    return count - 1;
                case Actions.Counter.Dispose _:
                case Actions.Counter.Reset _:
                    return 0;
                default:
                    return count;            }
        }

        public static ObservableCollection<Issue> IssuesReducer(ObservableCollection<Issue> issues, IRealmAction action)
        {
            switch (action)
            {
                case Actions.Issues.Set a:
                    return a.Data;
                case Actions.Issues.Clear:
                    return new ObservableCollection<Issue>();
                default:
                    return issues;
            }
        }

        public static bool IsLoadingReducer(bool isLoading, IRealmAction action)
        {
            switch(action)
            {
                case Actions.IsLoading.Set a:
                    return a.Value;
                default:
                    return isLoading;
            }
        }
    }
}
