using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegateLambdaDemo
{
    class Program
    {
        // Delegate declaration
        public delegate int Calc(int a, int b);

        // Methods for calculator
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static int Sub(int a, int b)
        {
            return a - b;
        }

        public static int Mul(int a, int b)
        {
            return a * b;
        }

        public static int Div(int a, int b)
        {
            return a / b;
        }

        // Methods for multicast delegate
        public static void Message1()
        {
            Console.WriteLine("Hello from Method 1");
        }

        public static void Message2()
        {
            Console.WriteLine("Hello from Method 2");
        }

        static void Main(string[] args)
        {
            Console.WriteLine(" Calculator using Delegates ");

            Calc c1 = Add;
            Console.WriteLine("Addition: " + c1(10, 5));

            c1 = Sub;
            Console.WriteLine("Subtraction: " + c1(10, 5));

            c1 = Mul;
            Console.WriteLine("Multiplication: " + c1(10, 5));

            c1 = Div;
            Console.WriteLine("Division: " + c1(10, 5));


            Console.WriteLine("\n Multicast Delegate Example ");

            Action multi = Message1;
            multi += Message2;
            multi();


            Console.WriteLine("\n Replace Methods with Lambda Expressions ");

            Calc add = (a, b) => a + b;
            Calc sub = (a, b) => a - b;

            Console.WriteLine("Lambda Add: " + add(8, 2));
            Console.WriteLine("Lambda Sub: " + sub(8, 2));


            Console.WriteLine("\n Sort List using Lambda ");

            List<int> numbers = new List<int>() { 5, 2, 8, 1, 3 };

            numbers.Sort((x, y) => x.CompareTo(y));

            Console.WriteLine("Sorted List:");
            foreach (int n in numbers)
            {
                Console.WriteLine(n);
            }


            Console.WriteLine("\n LINQ Query Example ");

            List<int> data = new List<int>() { 10, 25, 30, 45, 50 };

            var result = from n in data
                         where n > 25
                         select n;

            Console.WriteLine("Numbers greater than 25:");

            foreach (var i in result)
            {
                Console.WriteLine(i);
            }


            
        }
    }
}