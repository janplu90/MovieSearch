using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using MovieSearch.Model;
using MovieDownload;

namespace MovieSearch.Droid
{
    using DM.MovieApi;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Threading;

    [Activity(Theme = "@style/MyTheme", Label = "MovieSearch.Droid", MainLauncher = false,  Icon = "@drawable/icon")]
    public class MainActivity : Activity
	{

        public MovieDB db;
        public ApiSearchResponse<MovieInfo> responseMovieInfo;
        public MovieInfo title;

        public MoviesObjects moviesObjects = new MoviesObjects();
        public List<MoviesModel> _moviesmodel;
        public List<string> movies;

        public StorageClient storageClient;
        public ImageDownloader imageDownloader;

        public MainActivity()
        {
            this._moviesmodel = new List<MoviesModel>();
            this.movies = new List<string>();
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            MovieDbFactory.RegisterSettings("7d9a7734361d93c55e7b4691d91e1197", "http://api.themoviedb.org/3/");
            IApiMovieRequest movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

            this.storageClient = new StorageClient();
            this.imageDownloader = new ImageDownloader(this.storageClient);


            // Get our UI controls from the loaded layout
            var movieNameEditText = this.FindViewById<EditText>(Resource.Id.movieNameEditText);

            var movieNameTextView = this.FindViewById<TextView>(Resource.Id.movieNameTextView);

            var foundedMovieNameTextView = this.FindViewById<TextView>(Resource.Id.foundedMovieNameTextView);

            var searchMovieButton = this.FindViewById<Button>(Resource.Id.searchMovieButton);

            ApiSearchResponse<MovieInfo> responeTopRated = await movieApi.GetTopRatedAsync();

            var topRated = responeTopRated.Results;

            for (int i = 0; i < topRated.Count; i++)
            {

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
            }

            searchMovieButton.Click += async (sender, args) =>
            {
                var manager = (InputMethodManager)this.GetSystemService(InputMethodService);
                manager.HideSoftInputFromWindow(movieNameEditText.WindowToken, 0);


                responseMovieInfo = await movieApi.SearchByTitleAsync(movieNameEditText.Text);
                foundedMovieNameTextView.Text = responseMovieInfo.Results[0].OriginalTitle;

                var intent = new Intent(this, typeof(MovieListActivity));

                foreach (MovieInfo info in responseMovieInfo.Results)
                {
                    movies.Add(info.OriginalTitle);
                    //this._moviesmodel.Add(new MoviesModel(info.OriginalTitle, "(" + info.ReleaseDate.Year.ToString() + ")",
                    //        string.Empty, imageDownloader.LocalPathForFilename(info.PosterPath),
                    //       string.Empty, info.Overview));

                    this._moviesmodel.Add(new MoviesModel(info.OriginalTitle, "(" + info.ReleaseDate.Year.ToString() + ")",
                               string.Empty, string.Empty,
                              string.Empty, info.Overview));
                }

                //  intent.PutStringArrayListExtra("movieList", this.movies);
                intent.PutExtra("movieList", JsonConvert.SerializeObject(this._moviesmodel));
                StartActivity(intent);
                //    AddFilm(movieNameEditText, foundedMovieNameTextView);
            };

        }

        //public async void AddFilm(EditText titleField, TextView foundedMovieNameTextView)
        //{

        //    //this.db.SetApiMovieInfo(titleField.Text);
        //    // responseMovieInfo = this.db.GetApiMovieInfo();
        //    setDB(titleField);
        //    MovieInfo title = responseMovieInfo.Results[0];
        //    foundedMovieNameTextView.Text = title.OriginalTitle;

        //}

        //public async void setDB(EditText titleField)
        //{
        //    MovieDbFactory.RegisterSettings("4f7d30611a795d7959637a51d1e8a009", "http://api.themoviedb.org/3/");
        //    IApiMovieRequest movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
        //    responseMovieInfo = await movieApi.SearchByTitleAsync(titleField.Text);
        //}
    }
}


