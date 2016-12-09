using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MovieSearch.Model;
using MovieDownload;

using Fragment = Android.Support.V4.App.Fragment;

namespace MovieSearch.Droid
{
    using Android.Views.InputMethods;

    using Newtonsoft.Json;
    using DM.MovieApi;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using System.Collections.Generic;
    using System.Threading;
    using Android.InputMethodServices;

    public class MovieInputFragment : Fragment
    {
        public ApiSearchResponse<MovieInfo> responseMovieInfo;
        public List<MoviesModel> _moviesmodel;
        public StorageClient storageClient;
        public ImageDownloader imageDownloader;

        public MovieInputFragment()
        {
            this._moviesmodel = new List<MoviesModel>();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var rootView = inflater.Inflate(Resource.Layout.MovieInput, container, false);

            MovieDbFactory.RegisterSettings("7d9a7734361d93c55e7b4691d91e1197", "http://api.themoviedb.org/3/");
            IApiMovieRequest movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

            this.storageClient = new StorageClient();
            this.imageDownloader = new ImageDownloader(this.storageClient);


            // Get our UI controls from the loaded layout
            EditText movieNameEditText = rootView.FindViewById<EditText>(Resource.Id.movieNameEditText);

            TextView movieNameTextView = rootView.FindViewById<TextView>(Resource.Id.movieNameTextView);

            TextView foundedMovieNameTextView = rootView.FindViewById<TextView>(Resource.Id.foundedMovieNameTextView);

            Button searchMovieButton = rootView.FindViewById<Button>(Resource.Id.searchMovieButton);

            ProgressBar progressBar = rootView.FindViewById<ProgressBar>(Resource.Id.progressBar);
            progressBar.Visibility = ViewStates.Invisible;

            searchMovieButton.Click += async (sender, args) =>
            {
                if (movieNameEditText.Text.Length < 1)
                {
                    searchMovieButton.Enabled = true;
                    movieNameEditText.Text = "Enter movie title";
                }
                else
                {
                    searchMovieButton.Enabled = false;
                    progressBar.Visibility = Android.Views.ViewStates.Visible;

                    InputMethodManager manager = (InputMethodManager)this.Context.GetSystemService(Context.InputMethodService);
                    manager.HideSoftInputFromWindow(movieNameEditText.WindowToken, 0);

                    this._moviesmodel.Clear();


                    //if (movieNameEditText.Text == "")
                    //{

                    //}
                    // else
                    // {
                    responseMovieInfo = await movieApi.SearchByTitleAsync(movieNameEditText.Text);
                    var movieInfo = responseMovieInfo.Results;

                    for (int i = 0; i < movieInfo.Count; i++)
                    {

                        ApiQueryResponse<MovieCredit> movieInfoCast = await movieApi.GetCreditsAsync(movieInfo[i].Id);
                        ApiQueryResponse<Movie> movieInfoGenre = await movieApi.FindByIdAsync(movieInfo[i].Id);

                        var movieInfoCastList = movieInfoCast.Item.CastMembers;
                        var movieInfoGenresList = movieInfoGenre.Item.Genres;
                        var movieInfoTime = movieInfoGenre.Item.Runtime;


                        string path = "Not check";
                        if (movieInfo[i].PosterPath == null)
                        {
                            //  await
                            //imageDownloader.DownloadImage(topRated[i].PosterPath, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                            //    CancellationToken.None);
                            path = "Empty";

                        }
                        else
                        {
                            path = movieInfo[i].PosterPath;
                        }

                        string genreList = movieInfoTime + " | ";

                        if (movieInfoGenresList.Count == 0)
                        {
                            genreList += "";
                        }
                        else
                        {
                            genreList += movieInfoGenresList[0].Name;
                        }

                        for (var j = 1; j < movieInfoGenresList.Count; j++)
                        {
                            if (!movieInfoGenresList[j].Equals(null))
                            {
                                genreList += ", " + movieInfoGenresList[j].Name;
                            }
                        }

                        switch (movieInfoCastList.Count)
                        {
                            case 0:
                                this._moviesmodel.Add(new MoviesModel(movieInfo[i].Title, "(" + movieInfo[i].ReleaseDate.Year.ToString() + ")",
                                  string.Empty, path,
                                 genreList, movieInfo[i].Overview));
                                break;
                            case 1:
                                this._moviesmodel.Add(new MoviesModel(movieInfo[i].Title, "(" + movieInfo[i].ReleaseDate.Year.ToString() + ")",
                                 movieInfoCastList[0].Name, path,
                                genreList, movieInfo[i].Overview));
                                break;
                            case 2:
                                this._moviesmodel.Add(new MoviesModel(movieInfo[i].Title, "(" + movieInfo[i].ReleaseDate.Year.ToString() + ")",
                                 movieInfoCastList[0].Name + ", " + movieInfoCastList[1].Name, path,
                                genreList, movieInfo[i].Overview));
                                break;
                            default:
                                this._moviesmodel.Add(new MoviesModel(movieInfo[i].Title, "(" + movieInfo[i].ReleaseDate.Year.ToString() + ")",
                              movieInfoCastList[0].Name + ", " + movieInfoCastList[1].Name + ", " + movieInfoCastList[2].Name, path,
                             genreList, movieInfo[i].Overview));
                                break;
                        }
                    }

                    var intent = new Intent(this.Context, typeof(MovieListActivity));
                    //    intent.PutStringArrayListExtra("movieList", this.movies);
                    intent.PutExtra("movieList", JsonConvert.SerializeObject(this._moviesmodel));
                    progressBar.Visibility = Android.Views.ViewStates.Gone;
                    StartActivity(intent);



                    searchMovieButton.Enabled = true;
                    // }
                }
                };
            
            

                return rootView;
            }
            
    }
}