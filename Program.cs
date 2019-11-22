using System;
using System.Collections.Generic;

using grafos_sharp.Model;

namespace grafos_sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            AdjacencyMatrix<char> t = new AdjacencyMatrix<char>((u,v) => u.CompareTo(v), true);

            t.AddVertex('s');
            t.AddVertex('t');
            t.AddVertex('x');
            t.AddVertex('y');
            t.AddVertex('z');

            t.AddEdge('s', 't', 10);
            t.AddEdge('s', 'y', 5);

            t.AddEdge('t', 'x', 1);
            t.AddEdge('t', 'y', 2);

            t.AddEdge('x', 'z', 4);
            
            t.AddEdge('y', 't', 3);
            t.AddEdge('y', 'z', 2);
            t.AddEdge('y', 'x', 9);

            t.AddEdge('z', 'x', 6);

            AdjacencyList<char> l = (AdjacencyList<char>)t;

            Console.WriteLine(t);
            Console.WriteLine(l);

            Console.WriteLine(TreeAlgorithms<char>.Dijkstra(t, 's'));
            Console.WriteLine((AdjacencyList<char>) TreeAlgorithms<char>.Dijkstra(l, 's'));

            TreeAlgorithms<char>.BellmanFord(t, 's', out Graph<char> to);
            TreeAlgorithms<char>.BellmanFord(t, 's', out Graph<char> lo);

            AdjacencyList<char> ttt = (AdjacencyMatrix<char>)lo;
            Console.WriteLine(to);
            Console.WriteLine(ttt);
        } 
    }
}
