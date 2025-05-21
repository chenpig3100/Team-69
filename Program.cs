namespace Team_69
{
    internal class Program
    {
        static MovieCollection movieCollection = new MovieCollection();
        static MemberCollection memberCollection = new MemberCollection();
        static Member currentMember = null;

        static void Main(string[] args)
        {
            MainMenu();
        }

        //-----------------------------Main menu below, included staff login, member login, and Pause()-------------------------------
        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("COMMUNITY LIBRARY MOVIE DVD MANAGEMENT SYSTEM");
                Console.WriteLine("==================================================\n");
                Console.WriteLine("Main Menu");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Select from the following:");
                Console.WriteLine("1. Staff");
                Console.WriteLine("2. Member");
                Console.WriteLine("0. End the program");
                Console.Write("\nEnter your choice ==> ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        if (StaffLogin()) StaffMenu();
                        break;

                    case "2":
                        currentMember = MemberLogin();
                        if (currentMember != null) MemberMenu();
                        break;

                    case "0":
                        Console.WriteLine("Exiting...Goodbye!!");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Press any key.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static bool StaffLogin()
        {
            Console.WriteLine("Enter username:");
            string user = Console.ReadLine();
            Console.Write("Enter password:");
            string pass = Console.ReadLine();

            if(user == "staff" && pass == "today123")
            {
                Console.WriteLine("Staff login successful.");
                Pause();
                return true;
            }

            Console.WriteLine("Incorrect staff credentials.");
            Pause();
            return false;
        }

        static Member MemberLogin()
        {
            Console.Write("First Name: ");
            string fname = Console.ReadLine();
            Console.Write("Last Name: ");
            string lname = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            for (int i = 0; i <memberCollection.Count; i++)
            {
                var field = typeof(MemberCollection)
                    .GetField("members", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Member[] members = field.GetValue(memberCollection) as Member[];

                Member m = members[i];
                if (m != null && m.FirstName == fname && m.LastName == lname && m.Password == password)
                {
                    Console.WriteLine("Member login successful.");
                    Pause();
                    return m;
                }
            }

            Console.WriteLine("Member not found or password incorrect.");
            Pause();
            return null;
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue.....");
            Console.ReadKey();
        }

        //-----------------------------Staff menu below-------------------------------
        static void StaffMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Staff Menu");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("1. Add DVDs to system");
                Console.WriteLine("2. Remove DVDs from system");
                Console.WriteLine("3. Register a new member to system");
                Console.WriteLine("4. Remove a registered member from system");
                Console.WriteLine("5. Find a member contact phone number, given the member's name");
                Console.WriteLine("6. Find members who are currently renting a particular movie");
                Console.WriteLine("0. Return to main menu");
                Console.Write("\nEnter your choice ==> ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case"1":
                        AddMovie();
                        break;
                    case"2":
                        RemoveMovie();
                        break;
                    case"3":
                        RegisterMember();
                        break;
                    case"4":
                        RemoveMember();
                        break;
                    case"5":
                        FindMemberPhone();
                        break;
                    case"6":
                        FindMembersByMovie();
                        break;
                    case"0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        //-----------------------------Member menu below-------------------------------
        static void MemberMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Member Menu");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("1. Browse all the movies");
                Console.WriteLine("2. Display all the information about a movie, given the title of the movie");
                Console.WriteLine("3. Borrow a movie DVD");
                Console.WriteLine("4. Return a movie DVD");
                Console.WriteLine("5. List current borrowing movies");
                Console.WriteLine("6. Display the top 3 movies rented by the members");
                Console.WriteLine("0. Return to main menu");
                Console.Write("\nEnter your choice ==> ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        movieCollection.DisplayAllMovies();
                        Pause();
                        break;
                    case "2":
                        DisplayMovieInfo();
                        break;
                    case "3":
                        BorrowMovie();
                        break;
                    case "4":
                        ReturnMovie();
                        break;
                    case "5":
                        currentMember.DisplayBorrowedMovies();
                        Pause();
                        break;
                    case "6":
                        if (movieCollection.Count == 0)
                        {
                            Console.WriteLine("There is no movie in the collection.");
                            Pause();
                            break;
                        }
                        movieCollection.DisplayTop3();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again.");
                        Console.ReadKey();
                        break;

                }
            }
        }
        
        //Below are all the methods used in StaffMenu():
        //Case 1, Add DVDs to system
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
            Pause();
        }

        //Case 2, Remove DVDs from system
        // Got issue: 如果可租借的movie == 0, movie會被delete, (需要加以被租借的movie的判斷)
        static void RemoveMovie()
        {
            Console.Write("Enter the movie title to remove: ");
            string title = Console.ReadLine();
            Movie movie = movieCollection.Find(title);

            if (movie == null)
            {
                Console.WriteLine("Movie not found in the collection.");
                Pause();
                return;
            }

            Console.WriteLine($"There are {movie.AvailableCopies} available copies. How many to remove?");
            if (int.TryParse(Console.ReadLine(), out int countToRemove))
            {
                if (countToRemove <= 0 || countToRemove > movie.AvailableCopies)
                {
                    Console.WriteLine("Invalid number of copies to remove.");
                }
                else
                {
                    //Count how many members are currently rening this movie
                    int borrowedCount = 0;
                    var field = typeof(MemberCollection).GetField("members", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    Member[] members = field.GetValue(memberCollection) as Member[];

                    for (int i = 0; i < memberCollection.Count; i++)
                    {
                        Member m = members[i];
                        if (m != null)
                        {
                            foreach (var t in m.BorrowedMovies)
                            {
                                if (t == title)
                                {
                                    borrowedCount++;
                                    break;
                                }
                            }
                        }
                    }

                    if (countToRemove == movie.AvailableCopies)
                    {
                        if (borrowedCount > 0)
                        {
                            //Only remove available copies, keep the movie in system
                            movie.RemoveCopies(countToRemove);
                            Console.WriteLine($"Removed all available copies of '{title}', but kept the movie record since it is still borrowed.");
                        }
                        else
                        {
                            //No borrowed copies and staff removed all available => remove the movie completely
                            movieCollection.RemoveMovie(title);
                            Console.WriteLine($"All copies of '{title}' removed and movie deleted from system");
                        }
                    }
                    else
                    {
                        movie.RemoveCopies(countToRemove);
                        Console.WriteLine($"Removed {countToRemove} copies of '{title}'");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
            Pause();
        }

        //Case 3, Register a new member to system
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
            Pause();
        }

        //Case 4. Remove a registered member from system
        static void RemoveMember()
        {
            Console.Write("Enter first name: ");
            string fname = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lname = Console.ReadLine();

            for (int i=0; i < memberCollection.Count; i++)
            {
                var field = typeof(MemberCollection).GetField("members", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Member[] members = field.GetValue(memberCollection) as Member[];
                Member m = members[i];

                if (m != null && m.FirstName == fname && m.LastName == lname)
                {
                    if (m.BorrowedCount > 0)
                    {
                        Console.WriteLine("Member must return all DVDs before removal.");
                    }
                    else
                    {
                        memberCollection.Remove(fname, lname);
                        Console.WriteLine("Member removed successfully.");
                    }
                    Pause();
                    return;
                }
            }

            Console.WriteLine("Member not found.");
            Pause();
        }

        //Case 5, Find a member contact phone number, given the member's name
        static void FindMemberPhone()
        {
            Console.Write("Enter first name: ");
            string fname = Console.ReadLine();
            Console.Write("Enter last name: ");
            string lname = Console.ReadLine();

            for (int i = 0; i < memberCollection.Count; i++)
            {
                var field = typeof(MemberCollection).GetField("members", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Member[] members = field.GetValue(memberCollection) as Member[];
                Member m = members[i];

                if (m != null && m.FirstName == fname && m.LastName == lname)
                {
                    Console.WriteLine($"Phone number: {m.PhoneNumber}");
                    Pause();
                    return;
                }
            }

            Console.WriteLine("Member not found.");
            Pause();
        }

        //Case 6, Find members who are currently renting a particular movie
        static void FindMembersByMovie()
        {
            Console.Write("Enter the movie title: ");
            string title = Console.ReadLine();
            bool found = false;

            var field = typeof(MemberCollection).GetField("members", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Member[] members = field.GetValue(memberCollection) as Member[];

            Console.WriteLine($"\nMembers currently renting '{title}': ");
            for (int i=0; i < memberCollection.Count; i++)
            {
                Member m = members[i];
                if(m != null)
                {
                    foreach (var t in m.BorrowedMovies)
                    {
                        if (t == title)
                        {
                            Console.WriteLine($"- {m.FirstName} {m.LastName}");
                            found = true;
                            break;
                        }
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("No members currently renting this movie.");
            }

            Pause();
        }

        //Below are all the methods used in MemberMenu():
        //Case 1, Browse all the movies, doing that in the MovieCollection class


        //Case 2, Display all the info about a movie, given the title of the movie
        static void DisplayMovieInfo()
        {
            if (movieCollection.Count == 0)
            {
                Console.WriteLine("There is no movie in the collection.");
                Pause();
                return;
            }
            Console.WriteLine("Enter the title of the movie: ");
            string title = Console.ReadLine();
            Movie movie = movieCollection.Find(title);

            if (movie != null)
            {
                Console.WriteLine("\nMovie Information:");
                movie.Display();
            }
            else
            {
                Console.WriteLine("Movie not found.");
            }
            Pause();

        }

        //Case 3, Borrow a movie DVD
        static void BorrowMovie()
        {
            if (movieCollection.Count == 0)
            {
                Console.WriteLine("There is no movie in the collection.");
                Pause();
                return;
            }

            Console.Write("Enter movie title to borrow: ");
            string title = Console.ReadLine()?.Trim();

            //Input validation
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Invalid title input.");
                Pause();
                return;
            }

            if (currentMember.BorrowedCount >= 5)
            {
                Console.WriteLine("You can't borrow more than 5 copies in total.");
                Pause();
                return;
            }

            Movie? movie = movieCollection.Find(title);

            if (movie == null)
            {
                Console.WriteLine("Movie not found in the collection.");
                return;
            }

            //Trying to borrow the movie out, using Func<string, Movie?>
            currentMember.Borrow(title, movieCollection.Find);
           
            //error message has already been printed out in Member.Borrow(), no need to repeat it here
            Pause();
        }

        //Case 4, Return a movie DVD
        static void ReturnMovie()
        {
            if (currentMember.BorrowedCount == 0)
            {
                Console.WriteLine("You don't have any borrowed movie.");
                Pause();
                return;
            }

            Console.Write("Enter movie title to return: ");
            string title = Console.ReadLine();
            Movie movie = movieCollection.Find(title);

            if (currentMember.Return(title))
            {
                if (movie != null)
                {
                    movie.Return();
                }
                
                Console.WriteLine("Movie returned successfully.");
            }
            else
            {
                Console.WriteLine("Return failed. You may not have borrowed this movie.");
            }
            Pause();
        }

        //Case 5, List current borrowing movies


        //Case 6, we're doing that in the MovieCollection class

        static void CheckMovieCount()
        {
            
        }

        static void CheckMemberMovieCount()
        {
           
        }
    }
  
}
