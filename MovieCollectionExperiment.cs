using System;

namespace Team_69
{
    public static class MovieCollectionExperiment
    {
        public static void Run()
        {
            Console.WriteLine("InputSize, AverageComparisons");

            for (int size = 1000; size <= 20000; size+= 1000)
            {
                long totalComparisons = 0;

                for (int trial = 0; trial < 20; trial++)
                {
                    Movie[] movies = GenerateRandomMovies(size);
                    int comparisons = RunTopThreeExperiment(movies);
                    totalComparisons += comparisons;
                }

                double average = totalComparisons / 20.0;
                Console.WriteLine($"{size}, {average}");
            }
        }

        public static int RunTopThreeExperiment(Movie[] movies)
        {
            int comparisonCount = 0;
            Movie[] heap = new Movie[3];
            int heapSize = 0;

            for(int i = 0; i < movies.Length; i++)
            {
                if (i < 3)
                {
                    heap[heapSize++] = movies[i];
                    HeapifyUp(heap, heapSize - 1);
                } else
                {
                    comparisonCount++;
                    if (movies[i].BorrowedCount > heap[0].BorrowedCount)
                    {
                        heap[0] = movies[i];
                        HeapifyDown(heap, 0, heapSize);
                    }
                }
            }

            return comparisonCount;
        }

        private static Movie[] GenerateRandomMovies(int size)
        {
            Random rand = new Random();
            Movie[] movies = new Movie[size];

            for (int i = 0; i < size; i++)
            {
                movies[i] = new Movie(
                        title: "Movie" + i,
                        genre: Genre.Other,
                        classification: Classification.G,
                        duration: 100,
                        copies: 5
                    );

                for (int j = 0; j < rand.Next(50); j++)
                    movies[i].Borrow();
            }

            return movies;
        }

        private static void HeapifyUp(Movie[] heap, int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (heap[index].BorrowedCount >= heap[parent].BorrowedCount)
                    break;

                Swap(heap, index, parent);
                index = parent;
            }
        }

        private static void HeapifyDown(Movie[] heap, int index, int heapSize)
        {
            while (index < heapSize)
            {
                int smallest = index;
                int left = 2 * index + 1;
                int right = 2 * index + 2;

                if (left < heapSize && heap[left].BorrowedCount < heap[smallest].BorrowedCount)
                    smallest = left;

                if (right < heapSize && heap[right].BorrowedCount < heap[smallest].BorrowedCount)
                    smallest = right;

                if (smallest == index)
                    break;

                Swap(heap, index, smallest);
                index = smallest;
            }
        }

        private static void Swap(Movie[] heap, int i, int j)
        {
            Movie temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}