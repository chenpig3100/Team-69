using System;
using System.Collections.Generic;


namespace Team_69
{
	public class Member
	{
        //Don't have to declare fields, just declare getters and setters inside Properties shown below-------------------------------------
        public string FirstName {  get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }
		
		//a bit complicated so writing it as fields and properties pair for clarity
		private string[] borrowedMovies = new string[5];//field
		public string[] BorrowedMovies//property
		{
			get { return borrowedMovies; }//reading value from the lowercase variable
			private set { borrowedMovies = value;}//setting value for the lowercase variable, then pass to the uppercase property variable
		}

		private int borrowedCount = 0;
		public int BorrowedCount
		{
			get { return borrowedCount; }
			private set { borrowedCount = value;}
		}

		//constructor-------------------------------------
		public Member(string firstName, string lastName, string password, string phoneNumber)
		{
			FirstName = firstName;
			LastName = lastName;
			Password = password;
			PhoneNumber = phoneNumber;
		}

        //methods-------------------------------------
		//"when a member borrows a movie" method
        public bool Borrow(string title, Func<string, Movie?> findMovie)
		{
			if (BorrowedCount >= 5)
			{
				Console.WriteLine("Failed to borrow: you already reached 5 different movies");
				return false;
			}

			//using loop to loop through the array for checking any movie duplicates
			for (int i = 0;  i < BorrowedCount; i++) 
			{
				if (BorrowedMovies[i] == title)
				{
					Console.WriteLine("Failed to borrow: already borrowed the same movie");
					return false;
				}
			}

			//The following two condition checks are to update the borrowedcount and available copies in the Movie.cs
			Movie? movie = findMovie(title);
			if (movie == null)
			{
				Console.WriteLine("Movie not found.");
				return false;
			}

			if (!movie.Borrow())
			{
				Console.WriteLine("No available copies of this movie.");
				return false;
			}
			//--------------------------------------------------------------


			//The movie (represented by the variable title), will be stored in the BorrowedMovies array (position in the array determined by the BorrowedCount number)
			BorrowedMovies[BorrowedCount] = title;
			Console.WriteLine($"You have successfully borrowed: {title}");
			BorrowedCount++;
			return true;

		}

        //"when a member returns a movie" method
        public bool Return(string title)
		{
			//if there are things in the BorrowedMovies array then we first iterate thru and locate, then 
			for ( int i = 0; i < BorrowedCount; i++)
			{
				if (BorrowedMovies[i] == title)
				{
					BorrowedMovies[i] = BorrowedMovies[BorrowedCount - 1];
					BorrowedMovies[BorrowedCount - 1] = null;
					BorrowedCount--;
					Console.WriteLine($"You have successfully returned: {title}");
					return true;
				}
			}

            //if there's nothing inside the BorrowedMovies then return a false
            Console.WriteLine("Failed to return: there are no movies to return");
            return false;
        }

        //"when a member displays all borrowed movies" method
        public bool DisplayBorrowedMovies()
		{
            //if the movie array doesn't have any movies, return an error message
            if (BorrowedCount == 0)
			{
                Console.WriteLine("You didn't borrow any movie so there are no movies no display!");
                return false;
            }

            //loop thru the movie array and print out the title of every movie
            Console.WriteLine("Movies you have borrowed:");
			for (int i = 0; i < BorrowedCount; i++)
			{
				Console.WriteLine($"Movie {i + 1}: {BorrowedMovies[i]}");
			}
            return true;

		}

	}

}
