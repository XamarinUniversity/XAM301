using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Flixster.Data;

namespace Flixster
{
    public class MovieTableViewController : UITableViewController, IUISearchResultsUpdating
    {
        private UISearchController searchController;
        private NSString CellId = new NSString("MovieCell");
        private IReadOnlyList<Movie> movies;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            searchController = new UISearchController(searchResultsController: null)
            {
                SearchResultsUpdater = this,
                HidesNavigationBarDuringPresentation = false,
                ObscuresBackgroundDuringPresentation = false
            };

            searchController.SearchBar.Placeholder = "Search Movies";
            TableView.TableHeaderView = searchController.SearchBar;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return movies?.Count ?? 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellId);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellId);
            }

            var movie = movies[indexPath.Row];
            cell.TextLabel.Text = movie.Title;
            cell.DetailTextLabel.Text = movie.Description;

            return cell;
        }

        public async void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            var searchTerm = searchController.SearchBar.Text;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var movieService = new MovieService();
                movies = await movieService.GetMoviesForSearchAsync(searchTerm);
            }
            else
            {
                movies = null;
            }

            TableView.ReloadData();
        }
    }
}