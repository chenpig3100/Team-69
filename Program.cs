namespace Team_69
{
    internal class Program
    {
        static MovieCollection movieCollection = new MovieCollection();
        static MemberCollection memberCollection = new MemberCollection();
        static Member currentMember = null;

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== DVD Management System ===");
                Console.WriteLine("1. Register as new member");
                Console.WriteLine("2. Member login");
                Console.WriteLine("3. Add a new movie (staff only)");
                Console.WriteLine("4. Display all movies");
                Console.WriteLine("5. Display top 3 most borrowed movies");
                Console.WriteLine("6. Borrow a movie (logged-in member)");
                Console.WriteLine("7. Return a movie (logged-in member)");
                Console.WriteLine("8. Show my borrowed movies");
                Console.WriteLine("9. Logout");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        RegisterMember();
                        break;
                    case "2":
                        LoginMember();
                        break;
                    case "3":
                        AddMovie();
                        break;
                    case "4":
                        movieCollection.DisplayAllMovies();
                        break;
                    case "5":
                        movieCollection.DisplayTop3();
                        break;
                    case "6":
                        BorrowMovie();
                        break;
                    case "7":
                        ReturnMovie();
                        break;
                    case "8":
                        ShowBorrowedMovies();
                        break;
                    case "9":
                        Logout();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;
                }
            }

            Console.WriteLine("Thank you for using the DVD system!");
        }

        static void RegisterMember()
        {
            Console.Write("First name: ");
            string fname = Console.ReadLine();
            Console.Write("Last name: ");
            string lname = Console.ReadLine();
            Console.Write("Password: ");
            string pw = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            memberCollection.Add(fname, lname, pw, phone);
        }

        static void LoginMember()
        {
            Console.Write("First name: ");
            string fname = Console.ReadLine();
            Console.Write("Last name: ");
            string lname = Console.ReadLine();
            Console.Write("Password: ");
            string pw = Console.ReadLine();

            Member? member = memberCollection.FindByNameAndPassword(fname, lname, pw);
            if (member != null)
            {
                currentMember = member;
                Console.WriteLine($"Welcome {fname} {lname}!");
            }
            else
            {
                Console.WriteLine("Invalid login.");
            }
        }

        static void AddMovie()
        {
            Console.Write("Movie title: ");
            string title = Console.ReadLine();

            Console.WriteLine("Choose genre: 0-Drama, 1-Adventure, 2-Family, 3-Action, 4-SciFi, 5-Comedy, 6-Animated, 7-Thriller, 8-Other");
            Genre genre = (Genre)int.Parse(Console.ReadLine());

            Console.WriteLine("Choose classification: 0-G, 1-PG, 2-M15Plus, 3-MA15Plus");
            Classification classification = (Classification)int.Parse(Console.ReadLine());

            Console.Write("Duration (minutes): ");
            int duration = int.Parse(Console.ReadLine());

            Console.Write("Number of copies: ");
            int copies = int.Parse(Console.ReadLine());

            Movie newMovie = new Movie(title, genre, classification, duration, copies);
            bool added = movieCollection.AddMovie(newMovie);
            Console.WriteLine(added ? "Movie added." : "Movie already exists.");
        }

        static void BorrowMovie()
        {
            if (currentMember == null)
            {
                Console.WriteLine("Please login first.");
                return;
            }

            Console.Write("Enter movie title to borrow: ");
            string title = Console.ReadLine()?.Trim();

            //Input validation
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Invalid title input.");
                return;
            }


            Movie? movie = movieCollection.Find(title);

            if (movie == null)
            {
                Console.WriteLine("Movie not found in the collection.");
                return;
            }

            //Trying to borrow the movie out, using Func<string, Movie?>
            bool success = currentMember.Borrow(title, movieCollection.Find);

            if (success)
            {
                Console.WriteLine($"You have successfully borrowed: {title}");
            }
            else
            {
                //error message has already been printed out in Member.Borrow(), no need to repeat it here
            }

            
        }

        static void ReturnMovie()
        {
            if (currentMember == null)
            {
                Console.WriteLine("Please login first.");
                return;
            }

            Console.Write("Enter movie title to return: ");
            string title = Console.ReadLine();
            currentMember.Return(title);
        }

        static void ShowBorrowedMovies()
        {
            if (currentMember == null)
            {
                Console.WriteLine("Please login first.");
                return;
            }

            currentMember.DisplayBorrowedMovies();
        }

        static void Logout()
        {
            currentMember = null;
            Console.WriteLine("Logged out.");
        }

    }
  
}
