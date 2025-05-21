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

        // primary part for algorithm
        public void DisplayTop3()
        {
            // using Min Heap
            Movie[] heap = new Movie[3];
            int heapSize = 0;

            for (int i = 0; i < Capacity; i++)
            {
                if (table[i] == null || table[i].BorrowedCount == 0) continue;

                Movie current = table[i];
                if (heapSize < 3)
                {
                    // confuse
                    heap[heapSize++] = current;
                    HeapifyUp(heap, heapSize - 1);
                }
                else if (current.BorrowedCount > heap[0].BorrowedCount)
                {
                    heap[0] = current;
                    HeapifyDown(heap, 0, heapSize);
                }
            }

            if (heapSize == 0)
            {
                Console.WriteLine("No movies have been borrowed yet.");
                return;
            }

            // Sort
            Array.Sort(heap, (a, b) =>
            {
                if (a == null && b == null) return 0;
                if (a == null) return 1;
                if (b == null) return -1;
                return b.BorrowedCount.CompareTo(a.BorrowedCount);
            });


            Console.WriteLine("\nTop 3 Most Borrowed Movies:");
            for (int i = 0; i < 3; i++)
            {
                if (heap[i] != null)
                    Console.WriteLine($"{i + 1}. {heap[i].Title} - Borrowed {heap[i].BorrowedCount} time(s)");
                else
                    Console.WriteLine($"{i + 1}. null");
            }
            
        }

        private void HeapifyUp(Movie[] heap, int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (heap[index].BorrowedCount >= heap[parent].BorrowedCount) break;
                Swap(heap, index, parent);
                index = parent;
            }
        }

        private void HeapifyDown(Movie[] heap, int index, int size)
        {
            while (index < size)
            {
                int smallest = index;
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                if (left < size && heap[left].BorrowedCount < heap[smallest].BorrowedCount) smallest = left;
                if (right < size && heap[right].BorrowedCount < heap[smallest].BorrowedCount) smallest = right;
                if (smallest == index) break;
                Swap(heap, index, smallest);
                index = smallest;
            }
        }

        private void Swap(Movie[] heap, int i, int j)
        {
            Movie temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}
