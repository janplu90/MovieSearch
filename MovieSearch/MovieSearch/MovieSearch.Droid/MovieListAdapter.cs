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
using Square.Picasso;

namespace MovieSearch.Droid
{
    public class MovieListAdapter : BaseAdapter<MoviesModel>
    {
        private Activity _context;

        private List<MoviesModel> _movieList;

        private const string ImageUrl = "http://image.tmdb.org/t/p/original";

        public MovieListAdapter(Activity context, List<MoviesModel> movieList)
            {
            this._context = context;
            this._movieList = movieList;
            }

        public override int Count
        {
            get
            {
                return this._movieList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if(view == null)
            {
                view = this._context.LayoutInflater.Inflate(Resource.Layout.MovieListItem, null );
            }

            var movie = this._movieList[position];
           view.FindViewById<TextView>(Resource.Id.name).Text = movie.Name + movie.MovieYear;
            view.FindViewById<TextView>(Resource.Id.cast).Text = movie.CastMembers;
            var imageview = view.FindViewById<ImageView>(Resource.Id.picture);


            if (movie.ImageName == "Empty")
            {
                var resourceId = _context.Resources.GetIdentifier(
                    movie.ImageName,
                    "drawable",
                    _context.PackageName);

                view.FindViewById<ImageView>(Resource.Id.picture).SetBackgroundResource(resourceId);
            }
            else
            {
                var im = ImageUrl + movie.ImageName;
                Picasso.With(_context).Load(im).Into(imageview);

            }

            //File file;
            //List<string> fileList = new List<string>();
            //    file = new File(movie.ImageName);
            //if (!fileList.Contains(file.AbsolutePath))
            //{
            //    if (file.Exists())
            //    {
            //        var bmimg = BitmapFactory.DecodeFile(file.AbsolutePath);
            //        view.FindViewById<ImageView>(Resource.Id.picture).SetImageBitmap(bmimg);
            //        fileList.Add(file.AbsolutePath);
            //    }
            //}


            return view;
        }

        public override MoviesModel this[int position]
        {
            get
            {
                return this._movieList[position];
            }
        }
    }
}