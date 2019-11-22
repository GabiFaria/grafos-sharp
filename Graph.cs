using System;
using System.Linq;
using System.Collections.Generic;

namespace grafos_sharp
{
    public static class Print
    {
        public static void print<T>(List<T> l)
        {
            Console.Write("[");
            int length = l.Count;
            for (int i = 0; i < length; i++)
            {
                T item = l[i];

                Console.Write(item);
                if (i != length -1)
                    Console.Write(", ");
            }
            Console.Write("]\n");
        }
        public static void print<T>(Queue<T> q)
        {
            print(q.ToList());
        }
    }
}