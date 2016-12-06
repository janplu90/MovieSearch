using MovieSearch.iOS.Views;
using MovieSearch.Model;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieSearch.iOS.Controllers
{

    using CoreGraphics;
    using DM.MovieApi;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using DM.MovieApi.MovieDb.People;
    using Model;
    using MovieDownload;
    using System.Threading;
    using UIKit;

    public class MovieCollectionController : UICollectionViewController
    {
        private List<MoviesModel> _moviesModelList;

        //     public MoviesObjects moviesObjects = new MoviesObjects();

        public ImageDownloader imageDownloader;
        public StorageClient storageClient;

        public MovieCollectionController(UICollectionViewFlowLayout layout, List<MoviesModel> moviesModelList) : base(layout)
        {
            this._moviesModelList = moviesModelList;
            this.TabBarItem = new UITabBarItem(UITabBarSystemItem.TopRated, 0);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = "Top Rated Movies";
            this.CollectionView.BackgroundColor = UIColor.Black;
            this.CollectionView.ContentSize = this.View.Frame.Size;
            this.CollectionView.ContentInset = new UIEdgeInsets(5, 5, 5, 5);
            this.CollectionView.RegisterClassForCell(typeof(CustomCollectionCell), MovieCollectionSource.MovieCollectionCellId);
            this.CollectionView.Source = new MovieCollectionSource(this._moviesModelList, OnSelectedMoviesModel);
        }

        private void OnSelectedMoviesModel(int row)
        {
            this.NavigationController.PushViewController(new MovieInfoController(this._moviesModelList[row].Name, this._moviesModelList[row].MovieYear, this._moviesModelList[row].PlayTime, this._moviesModelList[row].Description, this._moviesModelList[row].ImageName), true);

        }
    }
}



