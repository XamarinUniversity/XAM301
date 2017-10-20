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
    [Activity(Label = "Fixster.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        MovieAdapter movieAdapter;
        IMoviePresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create our movie adapter
            movieAdapter = new MovieAdapter();

            // Create our presenter
            presenter = new MoviePresenter(movieAdapter.SetData);

            // Create the layout
            SetContentView(GenerateLayout(movieAdapter));
        }

        private void OnSearch(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewText))
            {
                presenter.FilterMovies(e.NewText);
            }

            e.Handled = true;
        }

        private View GenerateLayout(MovieAdapter movieAdapter)
        {
            LinearLayout layout = new LinearLayout(this) { Orientation = Orientation.Vertical };

            var searchView = new SearchView(this)
            {
                Iconified = false,
                SubmitButtonEnabled = false,
            };

            searchView.QueryTextChange += OnSearch;

            var listView = new ListView(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
                Adapter = movieAdapter
            };

            layout.AddView(searchView);
            layout.AddView(listView);

            return layout;
        }
    }
}

