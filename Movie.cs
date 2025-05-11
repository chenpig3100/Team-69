using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_69
{

    public enum Genre
    {
        Drama,
        Adventure,
        Family,
        Action,
        SciFi,
        Comedy,
        Animated,
        Thriller,
        Other
    }

    public enum Classification
    {
        G,
        PG,
        M15Plus,
        MA15Plus
    }

    public class Movie
    {

        public string Title { get; set; }
        public Genre Genre { get; set; }
        public Classification Classification { get; set; }
        public int Duration { get; set; }
        public int AvailableCopies { get; private set; }
        public int TotalCopies { get; private set; }
        public int BorrowedCount { get; private set; }

        public Movie(string title, Genre genre, Classification classification, int duration, int copies)
        {
            Title = title;
            Genre = genre;
            Classification = classification;
            Duration = duration;
            AvailableCopies = copies;
            TotalCopies = copies;
            BorrowedCount = 0;
        }

        public bool Borrow()
        {
            if (AvailableCopies > 0)
            {
                AvailableCopies--;
                BorrowedCount++;
                return true;
            }
            return false;
        }

        public bool Return()
        {
            if (AvailableCopies < TotalCopies)
            {
                AvailableCopies++;
                return true;
            }
            return false;
        }

        public void AddCopies(int count)
        {
            if (count > 0)
            {
                AvailableCopies += count;
                TotalCopies += count;
            }
        }

        public bool RemoveCopies(int count)
        {
            if (count > AvailableCopies)
                return false;

            AvailableCopies -= count;
            TotalCopies -= count;
            return true;
        }

        public void Display()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Classification: {Classification}");
            Console.WriteLine($"Duration: {Duration} mins");
            Console.WriteLine($"Available Copies: {AvailableCopies}");
            Console.WriteLine($"Total Borrowed: {BorrowedCount}");
        }
    }
}
