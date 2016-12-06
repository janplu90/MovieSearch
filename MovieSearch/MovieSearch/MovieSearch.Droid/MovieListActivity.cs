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

namespace MovieSearch.Droid
{
    [Activity(Theme ="@style/MyTheme", Label = "MovieList")]
    public class MovieListActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var movieList = this.Intent.Extras.GetStringArrayList("movieList") ?? new string[0];
            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, movieList);
        }
    }
}