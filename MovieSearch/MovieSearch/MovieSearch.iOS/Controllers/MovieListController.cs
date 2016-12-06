using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.iOS
{
    using Model;
    using UIKit;
    public class MovieListController : UITableViewController
    {
        private List<MoviesModel> _movieList;

        public MovieListController(List<MoviesModel> movieList)
        {
            this._movieList = movieList;
        }

        public override void ViewDidLoad()
        {
            
            this.View.BackgroundColor = UIColor.Black;
            this.Title = "Movie list";
            this.TableView.Source = new MovieListSource(this._movieList, OnSelectedMoviesModel); //sth missing
            //this.TableView.RowHeight = 100;

        }

        private void OnSelectedMoviesModel(int row)
        {
            //var okAlertController = UIAlertController.Create("Movies selected", this._movieList[row].Name, UIAlertControllerStyle.Alert);

            //okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            //this.PresentViewController(okAlertController, true, null);

            this.NavigationController.PushViewController(new MovieInfoController(this._movieList[row].Name, this._movieList[row].MovieYear, this._movieList[row].PlayTime, this._movieList[row].Description, this._movieList[row].ImageName), true);

        }
    }
}