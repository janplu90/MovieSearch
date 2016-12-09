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
using Java.IO;
using Android.Graphics;
using Newtonsoft.Json;
using MovieSearch.Model;

namespace MovieSearch.Droid
{
    [Activity(Label = "TopRatedExtendedInfoActivity", Theme = "@style/MyTheme",  MainLauncher = false, Icon = "@drawable/icon")]
    public class TopRatedExtendedInfoActivity : Activity
    {
        
        public MoviesModel movieModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MovieExtendedInfo);

            var jsonStr = this.Intent.GetStringExtra("movie");

            movieModel = JsonConvert.DeserializeObject<MoviesModel>(jsonStr);

            this.FindViewById<TextView>(Resource.Id.name).Text = movieModel.Name;

            this.FindViewById<TextView>(Resource.Id.timeAndGenres).Text = movieModel.PlayTime;

            this.FindViewById<TextView>(Resource.Id.description).Text = movieModel.Description;

            var file = new File(movieModel.ImageName);
            if (file.Exists())
            {
                var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
                this.FindViewById<ImageView>(Resource.Id.picture).SetImageBitmap(bmimg);
            }

        }


        
    }
    }
