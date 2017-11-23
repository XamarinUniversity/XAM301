using QuickFlicks.Data;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace QuickFlicks
{
    public partial class MovieTableViewController : UITableViewController, IUISearchResultsUpdating
    {
        private UISearchController searchController;
        private NSString CellId = new NSString("MovieCell");
        private IReadOnlyList<Movie> movies;

        public MovieTableViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            searchController = new UISearchController(searchResultsController: null)
            {
                HidesNavigationBarDuringPresentation = false,
                ObscuresBackgroundDuringPresentation = false,
                SearchResultsUpdater = this,
            };

            searchController.SearchBar.Placeholder = "Search Movies";
            TableView.TableHeaderView = searchController.SearchBar;

            //await UpdateMovieListings("Star Wars");
        }

        private async Task UpdateMovieListings(string searchTerm)
        {
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
            await UpdateMovieListings(searchTerm);
        }
    }
}
