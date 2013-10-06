using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SearchSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<String> fruits;
        private ObservableCollection<String> _searchResults;
        public MainPage()
        {
            this.InitializeComponent();

            fruits = new List<string>();
            fruits.Add("Apple");
            fruits.Add("Banana");
            fruits.Add("Cherry");
            fruits.Add("Clementine");
            fruits.Add("Dragon Fruit");
            fruits.Add("Orange");
            fruits.Add("Plum");

            _searchResults = new ObservableCollection<string>();
            this.DataContext = this;

            
        }

        public ObservableCollection<string> searchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
            }
        }

        private void SearchBoxEventsSuggestionsRequested(SearchBox sender, 
                                                         SearchBoxSuggestionsRequestedEventArgs e)
        {
            string queryText = e.QueryText;
            Windows.ApplicationModel.Search.SearchSuggestionCollection suggestionCollection = e.Request.SearchSuggestionCollection;

            //Fix this up
            //suggestionCollection.AppendResultSuggestion("Apple", "http://www.apple.com", "", null, null);
            //suggestionCollection.AppendSearchSeparator("Suggestions");
            
            foreach (string suggestion in fruits)
            {
                if (suggestion.StartsWith(queryText, StringComparison.CurrentCultureIgnoreCase))
                {
                    suggestionCollection.AppendQuerySuggestion(suggestion);
                }
            }
        }

        private void SearchBoxEventsQuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs e)
        {
            _searchResults.Add(e.QueryText);

            SearchBoxSuggestions.QueryText = "";
        }

        private void ClearHistory(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Search.Core.SearchSuggestionManager searchman = new Windows.ApplicationModel.Search.Core.SearchSuggestionManager();
            searchman.ClearHistory();
        }
    }
}
