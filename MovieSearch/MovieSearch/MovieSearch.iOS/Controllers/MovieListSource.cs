using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.iOS
{
    using Foundation;
    using Model;
    using UIKit;
    using Views;

    public class MovieListSource : UITableViewSource
    {
        public readonly NSString MovieListCellId = new NSString("movieListCell");

        private List<MoviesModel> _movieList;
        private Action<int> _onSelectedMoviesModel;

        public MovieListSource(List<MoviesModel> movieList, Action<int> onSelectedMoviesModel)
        {
            this._movieList = movieList;
            this._onSelectedMoviesModel = onSelectedMoviesModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (CustomCell)tableView.DequeueReusableCell(this.MovieListCellId);

            if (cell == null)
            {
                cell = new CustomCell((NSString)this.MovieListCellId);
            }

            int row = indexPath.Row;
            cell.UpdateCell(this._movieList[row].Name, this._movieList[row].MovieYear.ToString(),
                this._movieList[row].CastMembers.ToString(), this._movieList[row].ImageName);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this._movieList.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            this._onSelectedMoviesModel(indexPath.Row);
        }


    }
}
