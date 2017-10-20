using Android.App;
using Android.Widget;
using Android.OS;
using Flixster.Data;
using Android.Views;
using System.Collections.Generic;
using static Android.Views.ViewGroup;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Fixster.Droid
{
    public class MoviePresenter : IMoviePresenter
    {
        CancellationTokenSource cts;
        Action<IReadOnlyList<Movie>> filterMovies;

        public MoviePresenter(Action<IReadOnlyList<Movie>> setMovies)
        {
            filterMovies = setMovies;
        }

        public void FilterMovies(string search)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }

            if (!string.IsNullOrEmpty(search))
            {
                var myCts = cts = new CancellationTokenSource();

                var movieService = new MovieService();
                movieService.GetMoviesForSearchAsync(search)
                    .ContinueWith(tr => {

                        if (!myCts.IsCancellationRequested)
                        {
                            filterMovies?.Invoke(tr.Result);
                        }

                    }, myCts.Token,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                filterMovies?.Invoke(null);
            }
        }
    }
}

