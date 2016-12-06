
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.iOS.Controllers
{
    using Foundation;
    using Model;
    using UIKit;
    using Views;

    public class MovieCollectionSource : UICollectionViewSource
    {

        public static readonly NSString MovieCollectionCellId = new NSString("movieCollectionCell");

        private List<MoviesModel> _movieList;
        private Action<int> _onSelectedMoviesModel;

        public MovieCollectionSource(List<MoviesModel> movieList, Action<int> onSelectedMoviesModel)
        {
            this._movieList = movieList;
            this._onSelectedMoviesModel = onSelectedMoviesModel;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (CustomCollectionCell)collectionView.DequeueReusableCell(MovieCollectionCellId, indexPath);
          
            int row = indexPath.Row;

            cell.UpdateCell(this._movieList[row].Name, this._movieList[row].MovieYear, this._movieList[row].ImageName);

            return cell;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return this._movieList.Count;
        }

        public override void ItemSelected(UICollectionView tableView, NSIndexPath indexPath)
        {
            this._onSelectedMoviesModel(indexPath.Row);
        }

        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            
            return true;
        }

        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    tableView.DeselectRow(indexPath, true);

            
        //}


    }
}

