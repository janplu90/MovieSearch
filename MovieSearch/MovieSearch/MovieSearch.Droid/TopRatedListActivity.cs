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
using Newtonsoft.Json;

namespace MovieSearch.Droid
{
    [Activity(Theme = "@style/MyTheme", Label = "TopRatedListActivity")]
    public class TopRatedListActivity : Activity
    {
        public List<MoviesModel> movieList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.TopRatedList);

            var jsonStr = this.Intent.GetStringExtra("movieList");

            movieList = JsonConvert.DeserializeObject<List<MoviesModel>>(jsonStr);
            //     this.ListAdapter = new MovieListAdapter(this, movieList);
            var listview = this.FindViewById<ListView>(Resource.Id.namelistview);
            listview.Adapter = new MovieListAdapter(this, movieList);
            listview.ItemClick += MovieList_ItemClick;

            // Create your application here
        }

        void MovieList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(MovieExtendedInfoActivity));
            intent.PutExtra("movie", JsonConvert.SerializeObject(movieList[e.Position]));
            StartActivity(intent);
        }
    }
}