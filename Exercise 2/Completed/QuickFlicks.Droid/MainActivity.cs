using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace QuickFlicks.Droid
{
    [Activity(Label = "QuickFlicks.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        MoviePresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var movieList = FindViewById<ListView>(Resource.Id.movieList);
            var adapter = new MovieAdapter();
            movieList.Adapter = adapter;

            presenter = new MoviePresenter();
            presenter.FilterApplied += adapter.SetData;

            //await presenter.FilterMoviesAsync("Star Wars");

            var searchView = FindViewById<SearchView>(Resource.Id.searchView);
            searchView.QueryTextChange += OnSearch;
        }

        private async void OnSearch(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            await presenter.FilterMoviesAsync(e.NewText);
        }
    }
}

