 using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using System.Threading;
using DM.MovieApi;
using DM.MovieApi.MovieDb.People;
using MovieSearch.Model;
using MovieDownload;

namespace MovieSearch.iOS
{
    using CoreGraphics;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using Model;
    using UIKit;
    public class MovieController : UIViewController
    {
        private const int HorizontalMargin = 20;
        private const int StartY = 80;
        private const int StepY = 50;
        private int _yCoord;

        public StorageClient storageClient;
        public ImageDownloader imageDownloader;
        public List<MoviesModel> _moviesmodel;
        public UIActivityIndicatorView spinner;
        public IApiMovieRequest movieApi;
        public IApiPeopleRequest peopleApi;
        public UIButton navigateButton;

        public  MoviesObjects moviesObjects = new MoviesObjects();

        public MovieController(List<MoviesModel> moviesModel)
        {
            this._moviesmodel = moviesModel;
            this.TabBarItem = new UITabBarItem(UITabBarSystemItem.Search, 0);
        }

        public MovieController()
        {
            this._moviesmodel = new List<MoviesModel>();
            this.TabBarItem = new UITabBarItem(UITabBarSystemItem.Search, 0);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            this.Title = "Movie Search";
            this.View.BackgroundColor = UIColor.Black;
            
            this._yCoord = StartY;
            this._moviesmodel = new List<MoviesModel>();
            this.storageClient = new StorageClient();
            this.imageDownloader = new ImageDownloader(this.storageClient);

            spinner = this.CreateSpinner();
            var prompt = this.CreatePromptl();
            var titleField = this.CreateTitleField();
            var searchingLabel = this.CreateSearchingLabel();
            navigateButton = this.CreateButton("See movie list");

            MovieDbFactory.RegisterSettings("7d9a7734361d93c55e7b4691d91e1197", "http://api.themoviedb.org/3/");

            movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
            peopleApi = MovieDbFactory.Create<IApiPeopleRequest>().Value;

            AddTopRated();

            navigateButton.TouchUpInside += (sender, args) =>   //when touched
            {
                titleField.ResignFirstResponder();      //hide keyboard
                navigateButton.Enabled = false;
                spinner.StartAnimating();

                if (titleField.HasText == false)
                {
                    navigateButton.Enabled = true;
                }
                else
                {
                    this._moviesmodel.Clear();
                    AddFilm(titleField);                              
                }
              
            };

            this.View.AddSubview(prompt);
            this.View.AddSubview(titleField);
            this.View.AddSubview(searchingLabel);
            this.View.AddSubview(navigateButton);
            this.View.AddSubview(spinner);
        }

        private async void AddTopRated()
        {
            ApiSearchResponse<MovieInfo> responeTopRated = await movieApi.GetTopRatedAsync();

            var topRated = responeTopRated.Results;

            for (int i = 0; i < topRated.Count; i++)
            {

                ApiQueryResponse<MovieCredit> responeTopRatedCast = await movieApi.GetCreditsAsync(topRated[i].Id);
                ApiQueryResponse<Movie> responeTopRatedGenre = await movieApi.FindByIdAsync(topRated[i].Id);

                var topRatedGenres = responeTopRatedGenre.Item;
                var topRatedGenresList = topRatedGenres.Genres;

                var topRatedCast = responeTopRatedCast.Item;
                var topRatedCastList = topRatedCast.CastMembers;

                var localTop = imageDownloader.LocalPathForFilename(topRated[i].PosterPath);

                if (localTop.Length <= 0)
                {
                    //catch empty local path
                }
                else
                {
                    await
                  imageDownloader.DownloadImage(topRated[i].PosterPath, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                      CancellationToken.None);
                }


                if (topRatedCastList.Count < 3)
                {
                    this.moviesObjects.AddToMoviesModelList(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                                             string.Empty, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                                             topRatedGenres.Runtime + " min", topRated[i].Overview));
                }
                else
                {

 
                    if (topRatedGenresList.Count < 1)
                    {
                        this.moviesObjects.AddToMoviesModelList(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                                            topRatedCastList[0].Name + ", " + topRatedCastList[1].Name + ", " + topRatedCastList[2].Name, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                                            topRatedGenres.Runtime + " min", topRated[i].Overview));



                    }
                    else if (topRatedGenresList.Count == 1)
                    {

                        this.moviesObjects.AddToMoviesModelList(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                                            topRatedCastList[0].Name + ", " + topRatedCastList[1].Name + ", " + topRatedCastList[2].Name, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                                            topRatedGenres.Runtime + " min" + " I " + topRatedGenresList[0].Name, topRated[i].Overview));
                    }
                    else if (topRatedGenresList.Count == 2)
                    {

                        this.moviesObjects.AddToMoviesModelList(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                                              topRatedCastList[0].Name + ", " + topRatedCastList[1].Name + ", " + topRatedCastList[2].Name, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                                               topRatedGenres.Runtime + " min" + " I " + topRatedGenresList[0].Name + ", " + topRatedGenresList[1].Name, topRated[i].Overview));


                    }
                    else
                    {

                        this.moviesObjects.AddToMoviesModelList(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                                             topRatedCastList[0].Name + ", " + topRatedCastList[1].Name + ", " + topRatedCastList[2].Name, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                                              topRatedGenres.Runtime + " min" + " I " + topRatedGenresList[0].Name + ", " + topRatedGenresList[1].Name + ", " + topRatedGenresList[2].Name, topRated[i].Overview));


                    }
                }
            }
        }

        private UIActivityIndicatorView CreateSpinner()
        {
            spinner = new UIActivityIndicatorView(new Rectangle(60, 200, 200, 200))
            {
                ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
                HidesWhenStopped = true,             
            };

            spinner.Color = UIColor.Cyan;
            return spinner;
        }

        private UIButton CreateButton(string title)
        {
            var button = UIButton.FromType(UIButtonType.RoundedRect);
            button.Frame = new CGRect(HorizontalMargin, this._yCoord - 50, this.View.Bounds.Width - 2 * HorizontalMargin, 50);
            button.SetTitle(title, UIControlState.Normal);
            this._yCoord += StepY;
            return button;
        }

        private UILabel CreateSearchingLabel()
        {
            var searchingLabel = new UILabel() { Frame = new CGRect(HorizontalMargin, this._yCoord, this.View.Bounds.Width, 50) };
            this._yCoord += StepY;
            return searchingLabel;
        }

        private UITextField CreateTitleField()
        {
            var titleField = new UITextField()
            {
                Frame = new CGRect(HorizontalMargin, this._yCoord, this.View.Bounds.Width - 2 * HorizontalMargin, 50),
                BorderStyle = UITextBorderStyle.RoundedRect,
                Placeholder = "Search for...",
                TextColor = UIColor.White,
                BackgroundColor = UIColor.DarkGray,
                
            };
            this._yCoord += StepY;
            return titleField;
        }

        private UILabel CreatePromptl()
        {
            var prompt = new UILabel()
            {
                Frame = new CGRect(HorizontalMargin, this._yCoord, this.View.Bounds.Width, 50),
                Text = "Enter key word: ",
                TextColor = UIColor.White
                
            };
            this._yCoord += StepY;
            return prompt;
        }

        public MoviesObjects returnMoviesObjects()
        {
            return this.moviesObjects;
        }
        public async void AddFilm(UITextField titleField)
        {
           

            ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync(titleField.Text);

            foreach (MovieInfo info in response.Results)
            {
                ApiQueryResponse<MovieCredit> responeCast = await movieApi.GetCreditsAsync(info.Id);
                ApiQueryResponse<Movie> responeGenre = await movieApi.FindByIdAsync(info.Id);
              
                var movie = responeGenre.Item;

                var movieGenres = responeGenre.Item;
                var genresList = movieGenres.Genres;

                var movieCredit = responeCast.Item;
                var castList = movieCredit.CastMembers;

                var local = imageDownloader.LocalPathForFilename(info.PosterPath);
               

                if (local.Length <= 0)
                {
                    //catch empty local path
                }
                else
                {
                    await
                  imageDownloader.DownloadImage(info.PosterPath, imageDownloader.LocalPathForFilename(info.PosterPath),
                      CancellationToken.None);
                }


                if (castList.Count < 3)
                {
                    this._moviesmodel.Add(new MoviesModel(info.Title, "(" + info.ReleaseDate.Year.ToString() + ")",
                        string.Empty, imageDownloader.LocalPathForFilename(info.PosterPath), string.Empty, info.Overview));
                }
                else
                {
               
                    imageDownloader.LocalPathForFilename(info.PosterPath);


                    if (genresList.Count < 1)
                    {
                        this._moviesmodel.Add(new MoviesModel(info.Title, "(" + info.ReleaseDate.Year.ToString() + ")",
                            castList[0].Name + ", " + castList[1].Name + ", " + castList[2].Name, imageDownloader.LocalPathForFilename(info.PosterPath),
                            movie.Runtime.ToString() + " min", info.Overview));

                    

                    }
                    else if(genresList.Count >= 1 && genresList.Count < 2)
                    {

                        this._moviesmodel.Add(new MoviesModel(info.Title, "(" + info.ReleaseDate.Year.ToString() + ")",
                            castList[0].Name + ", " + castList[1].Name + ", " + castList[2].Name, imageDownloader.LocalPathForFilename(info.PosterPath),
                            movie.Runtime.ToString() + " min" + " I " + genresList[0].Name, info.Overview));
                    }
                    else if (genresList.Count >= 2 && genresList.Count < 3)
                    {

                        this._moviesmodel.Add(new MoviesModel(info.Title, "(" + info.ReleaseDate.Year.ToString() + ")",
                            castList[0].Name + ", " + castList[1].Name + ", " + castList[2].Name, imageDownloader.LocalPathForFilename(info.PosterPath),
                            movie.Runtime.ToString() + " min" + " I " + genresList[0].Name + ", " + genresList[1].Name, info.Overview));

                      
                    }
                    else
                    {
                 
                        this._moviesmodel.Add(new MoviesModel(info.Title, "(" + info.ReleaseDate.Year.ToString() + ")",
                            castList[0].Name + ", " + castList[1].Name + ", " + castList[2].Name, imageDownloader.LocalPathForFilename(info.PosterPath),
                            movie.Runtime.ToString() + " min" + " I " + genresList[0].Name + ", " + genresList[1].Name + ", " + genresList[2].Name, info.Overview));

                    
                    }

                }       
            }
            navigateButton.Enabled = true;
            spinner.StopAnimating();
            this.NavigationController.PushViewController(new MovieListController(this._moviesmodel), true);
           
        }
    }
}





