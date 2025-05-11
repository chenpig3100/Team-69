using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_69
{
    public class MovieCollection
    {
        private const int Capacity = 1000;
        private Movie[] table = new Movie[Capacity];
        private string[] keys = new string[Capacity];
        private int count = 0;

        public int Count => count;

        private int Hash(string key)
        {
            int hash = 0;
            foreach (char c in key)
                hash = (hash * 31 + c) % Capacity;

            return hash;
        }

        public bool AddMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Title)) return false;

            int index = Hash(movie.Title);

            for (int i = 0; i < Capacity; i++)
            {
                int probe = (index + i) % Capacity;

                if (keys[probe] == null)
                {
                    // add new movie
                    keys[probe] = movie.Title;
                    table[probe] = movie;
                    count++;
                    return true;
                }
                else if (keys[probe] == movie.Title)
                {
                    // add copy when the movie already exist
                    return false;
                }
            }
            // Table already full
            return false;
        }

        public Movie Find(string title)
        {
            int index = Hash(title);

            for (int i = 0; i < Capacity; i++)
            {
                int probe = (index + i) % Capacity;

                if (keys[probe] == null) break;
                if (keys[probe] == title) return table[probe];
            }
            return null;
        }

        public bool RemoveMovie(string title)
        {
            int index = Hash(title);

            for (int i = 0; i < Capacity;i++)
            {
                int probe = (index + i) % Capacity;

                if (keys[probe] == null) break;
                if (keys[probe] == title)
                {
                    keys[probe] = null;
                    table[probe] = null;
                    count--;
                    return true;
                }
            }
            return false;
        }

        public void DisplayAllMovies()
        {
            Movie[] allMovies = new Movie[count];
            int j = 0;

            for (int i = 0; i < Capacity; i++)
            {
                if (table[i] != null) allMovies[j++] = table[i];
            }

            // sort by title
            Array.Sort(allMovies, (a, b) => string.Compare(a.Title, b.Title));

            foreach (var movie in allMovies)
            {
                Console.WriteLine("==========================================================");
                movie.Display();
            }
        }

        public void DisplayTop3()
        {
            Movie[] top = new Movie[3];

            for (int i = 0;i < Capacity;i++)
            {
                if (table[i] == null) continue;
                for (int j = 0; j < 3; j++)
                {
                    if (top[j] == null || table[i].BorrowedCount > top[j].BorrowedCount)
                    {
                        for (int k = 2; k > j; k--) top[k] = top[k - 1];
                        top[j] = table[i];
                        break;
                    }
                }
            }

            Console.WriteLine("Top 3 Most Borrowed Movies:");
            foreach (var movie in top)
            {
                if (movie != null) Console.WriteLine($"{movie.Title} - Borrowed {movie.BorrowedCount} times");
            }
        }
    }
}
