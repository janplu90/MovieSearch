//using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;

namespace MovieSearch.Droid
{
    using Android.OS;
    using Android.Views;
    using MovieSearch.Model;
    using Android.Views.InputMethods;

    using Newtonsoft.Json;
    using DM.MovieApi;
    using DM.MovieApi.ApiRequest;
    using DM.MovieApi.ApiResponse;
    using DM.MovieApi.MovieDb.Movies;
    using System.Collections.Generic;
    using System.Threading;
    using Android.InputMethodServices;
    using MovieDownload;
    using Android.Widget;
    using Android.Content;
    using System.Threading.Tasks;

    public class TopRatedFragment : Fragment
    {
        public ApiSearchResponse<MovieInfo> responseTopRated;
        public List<MoviesModel> _moviesmodel;
        public StorageClient storageClient;
        public ImageDownloader imageDownloader;
        public ProgressBar spinner;
        public int counter = 0;

        public TopRatedFragment()
        {
            this._moviesmodel = new List<MoviesModel>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = inflater.Inflate(Resource.Layout.TopRatedList, container, false);

            spinner = rootView.FindViewById<ProgressBar>(Resource.Id.progressBar);
            spinner.Visibility = ViewStates.Visible;

            return rootView;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }



        public async Task FetchTopRatedMovies()
        {
            counter++;
            if (counter == 1)
            {
                MovieDbFactory.RegisterSettings("7d9a7734361d93c55e7b4691d91e1197", "http://api.themoviedb.org/3/");
                IApiMovieRequest movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

                this.storageClient = new StorageClient();
                this.imageDownloader = new ImageDownloader(this.storageClient);



                this._moviesmodel.Clear();


                //if (movieNameEditText.Text == "")
                //{

                //}
                // else
                // {
                responseTopRated = await movieApi.GetTopRatedAsync();
                var topRated = responseTopRated.Results;


                for (int i = 0; i < topRated.Count; i++)
                {

                    ApiQueryResponse<MovieCredit> responseTopRatedCast = await movieApi.GetCreditsAsync(topRated[i].Id);
                    ApiQueryResponse<Movie> responseTopRatedGenre = await movieApi.FindByIdAsync(topRated[i].Id);

                    var topRatedGenresList = responseTopRatedGenre.Item.Genres;
                    var topRatedCastList = responseTopRatedCast.Item.CastMembers;
                    var topRatedTime = responseTopRatedGenre.Item.Runtime;

                    string path = "Not check";

                    if (topRated[i].PosterPath == null)
                    {
                        //  await
                        //imageDownloader.DownloadImage(topRated[i].PosterPath, imageDownloader.LocalPathForFilename(topRated[i].PosterPath),
                        //    CancellationToken.None);
                        path = "Empty";

                    }
                    else
                    {
                        path = topRated[i].PosterPath;
                    }

                    string genreList = topRatedTime + " | ";

                    if (topRatedGenresList.Count == 0)
                    {
                        genreList += "";
                    }
                    else
                    {
                        genreList += topRatedGenresList[0].Name;
                    }

                    for (var j = 1; j < topRatedGenresList.Count; j++)
                    {
                        if (!topRatedGenresList[j].Equals(null))
                        {
                            genreList += ", " + topRatedGenresList[j].Name;
                        }
                    }

                    switch (topRatedCastList.Count)
                    {
                        case 0:
                            this._moviesmodel.Add(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                              string.Empty, path,
                             genreList, topRated[i].Overview));
                            break;
                        case 1:
                            this._moviesmodel.Add(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                             topRatedCastList[0].Name, path,
                            genreList, topRated[i].Overview));
                            break;
                        case 2:
                            this._moviesmodel.Add(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                             topRatedCastList[0].Name + ", " + topRatedCastList[1].Name, path,
                            genreList, topRated[i].Overview));
                            break;
                        default:
                            this._moviesmodel.Add(new MoviesModel(topRated[i].Title, "(" + topRated[i].ReleaseDate.Year.ToString() + ")",
                          topRatedCastList[0].Name + ", " + topRatedCastList[1].Name + ", " + topRatedCastList[2].Name, path,
                         genreList, topRated[i].Overview));
                            break;
                    }
                }

                var intent = new Intent(this.Context, typeof(MovieListActivity));
                //    intent.PutStringArrayListExtra("movieList", this.movies);
                intent.PutExtra("movieList", JsonConvert.SerializeObject(this._moviesmodel));
                StartActivity(intent);
            }
            else
            {
                var intent = new Intent(this.Context, typeof(MovieListActivity));
                //    intent.PutStringArrayListExtra("movieList", this.movies);
                intent.PutExtra("movieList", JsonConvert.SerializeObject(this._moviesmodel));
                StartActivity(intent);
            }

            spinner.Visibility = ViewStates.Gone;
        }
    }
}