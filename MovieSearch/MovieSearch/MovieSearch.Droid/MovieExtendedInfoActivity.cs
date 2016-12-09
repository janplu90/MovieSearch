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
using Java.IO;
using Android.Graphics;
using Newtonsoft.Json;
using Square.Picasso;

namespace MovieSearch.Droid
{
    [Activity(Theme = "@style/MyTheme", Label = "MovieSearch.Droid", MainLauncher = false, Icon = "@drawable/icon")]
    public class MovieExtendedInfoActivity : Activity
    {
        public MoviesModel movieModel;
        private const string ImageUrl = "http://image.tmdb.org/t/p/original";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MovieExtendedInfo);

            var toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.SetActionBar(toolbar);
            this.ActionBar.Title = "Extended Movie Info";

            var jsonStr = this.Intent.GetStringExtra("movie");

            movieModel = JsonConvert.DeserializeObject<MoviesModel>(jsonStr);     

            this.FindViewById<TextView>(Resource.Id.name).Text = movieModel.Name;

            this.FindViewById<TextView>(Resource.Id.timeAndGenres).Text = movieModel.PlayTime;

            this.FindViewById<TextView>(Resource.Id.description).Text = movieModel.Description;

            var file = new File(movieModel.ImageName);

            var imageview = this.FindViewById<ImageView>(Resource.Id.picture);
            //if (file.Exists())
            //{
            //    var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
            //    this.FindViewById<ImageView>(Resource.Id.picture).SetImageBitmap(bmimg);
            //}

            if (movieModel.ImageName == "Empty")
            {
                if (file.Exists())
                {
                    var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
                    this.FindViewById<ImageView>(Resource.Id.picture).SetImageBitmap(bmimg);
                }
            }
            else
            {
                var im = ImageUrl + movieModel.ImageName;
                Picasso.With(this).Load(im).Into(imageview);

            }




        }
    }
}