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
using Newtonsoft.Json;
using MovieSearch.Model;

namespace MovieSearch.Droid
{

    [Activity(Theme ="@style/MyTheme", Label = "MovieList")]
    public class MovieListActivity : Activity
    {
        public List<MoviesModel> movieList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.MovieList);

            var jsonStr = this.Intent.GetStringExtra("movieList");

            movieList = JsonConvert.DeserializeObject<List<MoviesModel>>(jsonStr);
            var listview = this.FindViewById<ListView>(Resource.Id.namelistview);
            listview.Adapter = new MovieListAdapter(this, movieList);
            listview.ItemClick += MovieList_ItemClick;

            var toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.SetActionBar(toolbar);
            // this.ActionBar.Title = this.GetString(Resource.String.ToolbarTitle);
            this.ActionBar.Title = "Movie List";
            
        }

        void MovieList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(MovieExtendedInfoActivity));
            intent.PutExtra("movie", JsonConvert.SerializeObject(movieList[e.Position]));
            StartActivity(intent);
        }
 
    }
}