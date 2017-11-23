using QuickFlicks.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace QuickFlicks.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IReadOnlyList<Movie> _movies;
        public IReadOnlyList<Movie> Movies
        {
            get => _movies;
            private set
            {
                _movies = value;
                RaisePropertyChanged(nameof(Movies));
            }
        }

        private string searchTerm;
        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                if (searchTerm != value)
                {
                    searchTerm = value;
                    RaisePropertyChanged(); // use the [CallerMemberName] support
                    OnSearchTermChangedAsync(searchTerm)
                        .ContinueWith(tr => throw new Exception("Search Failed.", tr.Exception),
                        TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        private CancellationTokenSource cts;
        private async Task OnSearchTermChangedAsync(string searchTerm)
        {
            cts?.Cancel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var innerToken = cts = new CancellationTokenSource();

                var movieService = new MovieService();
                var movies = await movieService.GetMoviesForSearchAsync(searchTerm);
                if (!innerToken.IsCancellationRequested)
                {
                    Movies = movies;
                }
            }
            else
            {
                Movies = null;
            }
        }

        public MainViewModel()
        {
            //OnSearchTermChangedAsync("Star Wars");
        }
    }
}
