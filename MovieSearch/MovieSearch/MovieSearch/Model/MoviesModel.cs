using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.MovieApi.MovieDb.Movies;

namespace MovieSearch.Model
{
    public class MoviesModel
    {
        public string Name { get; set; }

        public string MovieYear { get; set; }

        public string CastMembers { get; set; }

        public string ImageName { get; set; }

        public string PlayTime { get; set; }

        public string Description { get; set; }

        public MoviesModel(string Name, string Movieyear,string Cast, string Image, string PlayTime, string Description)
        {
            this.Name = Name;

            this.MovieYear = Movieyear;

            this.CastMembers = Cast;

            this.ImageName = Image;

            this.PlayTime = PlayTime;

            this.Description = Description;



        }

 
    }
}
