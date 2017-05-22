using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackProblem
{
    public class Item
    {
        float value;
        public float weight;
        public int number;
        public float ratio;

        public Item (float weight = -1, float value = -1, Boolean random = false)
        {
            if (random)
            {
                Console.Write("Podaj wage: ");
                weight = float.Parse(Console.ReadLine());
                Console.Write("Podaj wartosc: ");
                value = float.Parse(Console.ReadLine());

                return;
            }

            if (weight == -1 && value == -1)
            {
                Random rnd = new Random();
                this.weight = rnd.Next(1, 10);
                this.value = rnd.Next(1, 10);
                calcRatio();

                return;
            }

            this.weight = weight;
            this.value = value;

            calcRatio();
        }

        public float calcRatio()
        {
            ratio = value / weight;
            return ratio;
        }

        public float GetValue ()
        {
            return this.value;
        }

        public float GetWeight ()
        {
            return this.weight;
        }
    }

    class DynamicSolver
    {
        Item[] items;
        int[] knapsack;

        public DynamicSolver (int itemsAmount, int knapsackSize)
        {
            FillKnapsack(knapsackSize);
            FillItems(itemsAmount);
        }

        public DynamicSolver (Item[] items, int[] knapsack)
        {
            this.items = items;
            this.knapsack = knapsack;
        }

        static int[] FillArray (int len, int val = 0)
        {
            int[] arr = new int[len];

            for (int i = 0; i < len; i++)
            {
                arr[i] = val;
            }

            return arr;
        }

        void FillKnapsack (int len)
        {
            this.knapsack = FillArray(len);
        }

        void FillItems (int length)
        {
            this.items = new Item[length];

            for (int i = 0; i < length; i++)
            {
                this.items[i] = new Item(-1, -1, true);
            }
        }

        int Max (float a, float b)
        {
            return (int)(a > b ? a : b);
        }

        public int[] Solve ()
        {
            
            for (int i = 0; i <  this.knapsack.Length; i++)
            {
                for (int j = 1; j < this.items.Length; j++)
                {
                    if (this.items[j].GetWeight() <= i)
                    {
                        float weight = this.items[j].GetWeight();
                        float secondValue = (float)this.knapsack[i - (int)weight] + this.items[j].GetValue();
                        this.knapsack[i] = Max((float)this.knapsack[i], secondValue);
                    }
                }
            }

            return this.knapsack;
        }
    }

    class Program
    {

        static public List<Item> Heurestic(int capacity, int numberOfItems, List<Item> listOfItems)
        {
            List<Item> result = new List<Item>();
            float fill = 0;
            for (int i = 0; i < numberOfItems; i++)
            {
                if (listOfItems[i].weight + fill <= capacity)
                {
                    result.Add(listOfItems[i]);
                    fill += listOfItems[i].weight;
                }
            }
            return result;
        }

        static void PrintOut(int[] data)
        {
             foreach (int i in data)
            {
                Console.Out.WriteLine(i);
            }

            Console.ReadKey();
        }

        static void SolveHeuristic()
        {
            int capacity;
            int numberOfItems;

            Console.Write("Podaj ladownosc statku: ");
            capacity = int.Parse(Console.ReadLine());
            Console.Write("Podaj ilosc przedmiotow: ");
            numberOfItems = int.Parse(Console.ReadLine());


            List<Item> lista = new List<Item>();
            for (int i = 0; i < numberOfItems; i++)
            {
                lista.Add(new Item());
                lista[i].number = i;
            }

            Console.WriteLine("\n");
            /*wypisanie stosunków wartości do wagi listy wejściowej
            for (int i = 0; i < numberOfItems; i++)
                Console.WriteLine(lista[i].ratio);*/


            //sortowanie malejące listy względem stosunku wartości do wagi
            List<Item> result = lista.OrderByDescending(x => x.ratio).ToList();

            Console.WriteLine("\n");
            for (int i = 0; i < numberOfItems; i++)
                Console.WriteLine(result[i].ratio);


            Console.WriteLine("\n");
            //lista wynikowa
            List<Item> test = new List<Item>();
            test = Heurestic(capacity, numberOfItems, result);  //wywołanie algorytmu aproksymacyjnego
            for (int i = 0; i < test.Count; i++)
                Console.WriteLine(test[i].number);
            Console.ReadKey();

        }
        

        static void SolveDynamic()
        {
            int knapsackSize = 30;
            int itemsAmount = 4;

            DynamicSolver solver = new DynamicSolver(itemsAmount, knapsackSize);
            int[] result = solver.Solve();

            PrintOut(result);
        }

        public static Stopwatch StartCounting()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            return stopWatch;
        }

        public static string StopCounting(Stopwatch stopWatch)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format(
                                    "{0:00}:{1:00}:{2:00}.{3:00}",
                                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10
                                 );

            return elapsedTime;
        }




        static void Main (string[] args)
        {
 
            SolveDynamic();
            SolveHeuristic();
        }
    }
}