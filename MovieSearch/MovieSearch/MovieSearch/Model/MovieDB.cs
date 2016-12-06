using System;
using DM.MovieApi;


namespace MovieSearch
{
    public class MovieDB : IMovieDbSettings
    {
     
        public string ApiKey
        {
            get
            {
                
                throw new Exception("7d9a7734361d93c55e7b4691d91e1197");
            }
        }

        public string ApiUrl
        {
            get
            {
                throw new Exception("http://api.themoviedb.org/3/");

                //throw new NotImplementedException();
            }
        }
    }
}

