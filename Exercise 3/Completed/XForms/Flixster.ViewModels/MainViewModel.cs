using Flixster.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Flixster.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string searchTerm;
        private CancellationTokenSource cts;
        private IReadOnlyList<Movie> _movies;

        public event PropertyChangedEventHandler PropertyChanged;
        public IReadOnlyList<Movie> Movies
        {
            get => _movies;
            private set
            {
                _movies = value;
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                if (searchTerm != value)
                {
                    searchTerm = value;
                    RaisePropertyChanged();
                    OnSearchTermChanged();
                }
            }
        }

        private void OnSearchTermChanged()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var myCts = cts = new CancellationTokenSource();

                var movieService = new MovieService();
                var movies = movieService.GetMoviesForSearchAsync(searchTerm)
                    .ContinueWith(tr => {

                        if (!myCts.IsCancellationRequested)
                        {
                            Movies = tr.Result;
                        }

                    }, myCts.Token,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public MainViewModel()
        {
            Movies = new ObservableCollection<Movie>();
        }
    }
}
