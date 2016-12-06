using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using System.Threading;
using DM.MovieApi;
using DM.MovieApi.MovieDb.People;
using Foundation;
using MovieSearch.Model;
using MovieDownload;


namespace MovieSearch.iOS
{
    using System;
    using CoreGraphics;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using Model;
    using UIKit;
    public class MovieInfoController : UIViewController
    {

        private List<MoviesModel> _movieList;
        public StorageClient storageClient;
        public ImageDownloader imageDownloader;
        private UIImageView _imageView;


        private string _title;

        private string _playTime;

        private string _description;

        private string _picture;

        private string _year;


        public MovieInfoController(string title, string year, string playTime, string description, string picture)
        {
            this._title = title;

            this._year = year;

            this._playTime = playTime;

            this._description = description;

            this._picture = picture;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.Black;
            this.Title = "Movie info";



            var titleLabel = this.CreateTitleLabel();
            var playTimeLabel = this.CreatePlayTimeLabel();
            var descriptionLabel = this.CreateDescriptionLabel();
            // var picture = this.CreatePicture();
            this._imageView = new UIImageView();
            this._imageView.Frame = new CGRect(5, 130, 80, 55);
            if (_picture.Equals(string.Empty))
            {

            }
            else
            {
                this._imageView.Image = UIImage.FromFile(_picture);
            }





            this.View.AddSubview(playTimeLabel);
            this.View.AddSubview(titleLabel);
            this.View.AddSubview(descriptionLabel);
            this.View.AddSubview(_imageView);

        }

        private UILabel CreatePicture()
        {
            var picture = new UILabel
            {
                Frame = new CGRect(5, 130, 100, 80),
                TextColor = UIColor.White,
            };

            if (_picture.Equals(string.Empty))
            {

            }
            else
            {
                picture = new UILabel
                {
                    Frame = new CGRect(5, 130, 100, 80),
                    TextColor = UIColor.FromRGB(0, 0, 0),
                    BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(_picture)) // TO DO -- jesli naciskamy pusty to sie wywala bo wpisuje pusty do background
                                                                                           //   BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(_picture)),
                };
            }
            return picture;
        }

        private UILabel CreateDescriptionLabel()
        {

            var labelSize = new NSString(_description).
                GetBoundingRect(
                        new CGSize(this.View.Frame.Width, float.MaxValue),
                        NSStringDrawingOptions.UsesLineFragmentOrigin,
                        new UIStringAttributes() { Font = UIFont.SystemFontOfSize(5) },
                        null);

            var descriptionLabel = new UILabel()
            {
                Frame = new CGRect(100, 130, this.View.Bounds.Width - 105, labelSize.Height),
                Text = _description,
                TextColor = UIColor.White,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap

            };

            return descriptionLabel;
        }

        private UILabel CreatePlayTimeLabel()
        {
            var playTimeLabel = new UILabel()
            {
                Frame = new CGRect(5, 80, this.View.Bounds.Width - 60, 50),
                Text = _playTime,
                TextColor = UIColor.White
            };

            return playTimeLabel;
        }

        private UILabel CreateTitleLabel()
        {
            var titleLabel = new UILabel()
            {
                Frame = new CGRect(5, 50, this.View.Bounds.Width - 60, 50),
                TextColor = UIColor.White,
                Text = _title + _year
            };

            return titleLabel;
        }





    }
}