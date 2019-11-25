using System;
using System.Linq;
using System.Collections.Generic;

namespace grafos_sharp.Model
{
    public static class TreeAlgorithms<T>
    {
        /*
        Arvore Geradora Minimal de Kruskal
        Retorna um grafo que representa a arvore de menor
        caminho entre todos os vertices
        */
        public static AdjacencyList<T> Kruskal(Graph<T> graph)
        {
            //Obtem a lista de chaves do dicionario
            //Referencia direta aos vertices do grafo
            List<Vertex<T>> keys = graph.Vertices();
            //Lista de aresta do grafo base
            List<Edge<T>> E = graph.Edges();
            //Grafo temporario para busca em profundidade
            AdjacencyList<T> temp;
            //Iniciamos a nova lista de aresta da arvore a ser gerada
            List<Edge<T>> A = new List<Edge<T>>();
            //Para cada aresta de E, ordenada crescentemente
            foreach (Edge<T> uv in E)
            {
                //Os vertices se encontram na mesma componenete?
                //E possivel apartir do vertice de origem chegar
                //no vertice de destino
                bool isSameComponent;

                //Geramos um grafo com as arestas seguras
                temp = new AdjacencyList<T>(keys, A, graph.CompareTo());
                //Partimos da origem a procura do vertice de chegada
                temp.BFS(uv.from.value);
                //Caso não seja possivel alcancar o vertice
                //Dizemos que não estao no mesmo componente
                isSameComponent = !float.IsPositiveInfinity(uv.to.distance);
                //Caso não estejam no mesmo componente
                if(!isSameComponent)
                {
                    //Esta aresta e segura
                    A.Add(uv);
                }
            }
            //Retornamos o grafo gerado apartir da lista de arestas seguras
            return new AdjacencyList<T>(keys, A, graph.CompareTo(), graph.isDirected);
        }

        /*
        Arvore Geradora Minimal de Prim
        Retorna um grafo que representa a arvore de menor
        caminho entre todos os vertices apartir de uma origem r
        */
        public static AdjacencyList<T> Prim(Graph<T> graph, T r)
        {
            //Obtem a lista de chaves do dicionario
            //Referencia direta aos vertices do grafo
            List<Vertex<T>> keys = graph.Vertices();
            //Para cada vertice do grafo
            foreach (Vertex<T> U in keys)
            {
                //Colocamos as variaveis internas do vertice em seus valores padrão
                U.key = float.PositiveInfinity;
                U.pi = null;
            }
            //Encontramos o vertice R
            Vertex<T> R = keys.Find(x => x.value.Equals(r));
            //A chave de R vale 0
            R.key = 0;
            //Adicionamos a fila Q todos os vertices ordenados pela sua chave
            Queue<Vertex<T>> Q = new Queue<Vertex<T>>(keys.OrderBy(x => x.key));
            //Enquanto houver vertice em Q
            while (Q.Count != 0)
            {
                //Desenpilhamos U
                Vertex<T> U = Q.Dequeue();
                //Para cada aresta adjacente a U
                foreach (Edge<T> edge in graph.Adjacent(U))
                {
                    //Obtemos o vertice V de destino da aresta
                    Vertex<T> V = edge.to;
                    //Se V estiver em Q e seu peso de sua aresta for
                    //menor que sua chave
                    if (Q.Contains(V) && edge.weight < V.key)
                    {
                        //'pai' de V é U
                        V.pi = U;
                        //A chave de v e o peso da aresta
                        V.key = edge.weight;
                    }
                }
            }
            //Retornamos o grafo gerado apartir da lista de arestas seguras
            return new AdjacencyList<T>(keys, graph.Edges().FindAll(x => (x.from.Equals(x.to.pi) || x.to.Equals(x.from.pi))), graph.CompareTo(), graph.isDirected);
        }

        private static void InitializeSingleSource(Graph<T> graph, Vertex<T> S)
        {
            foreach (Vertex<T> V in graph.Vertices())
            {
                V.distance = float.PositiveInfinity;
                V.f = float.PositiveInfinity;
                V.pi = null;
            }
            S.distance = 0;
        }

        private static void Relax(Edge<T> UtoV)
        {
            //Se a distância de S até V é maior do que a distância de S até U + peso da aresta, então U é o novo pai de V.
            if (UtoV.to.distance > UtoV.from.distance + UtoV.weight)
            {
                UtoV.to.distance = UtoV.from.distance + UtoV.weight;
                UtoV.to.pi = UtoV.from;
            }
        }

        public static bool BellmanFord(Graph<T> graph, T s, out Graph<T> output)
        {
            //Encontra no grafo o vértice correspondente a s.
            Vertex<T> S = graph.Vertices().Find(x => x.value.Equals(s));
            InitializeSingleSource(graph, S);
            //Para cada vértice do grafo
            for (int i = 1; i < graph.Vertices().Count-1; i++)
            {
                //Para cada aresta do grafo, indepedente do vértice
                foreach (Edge<T> edge in graph.Edges())
                {
                    //define novos pais a partir da função Relax
                    Relax(edge);
                }
            }
            //Para cada aresta
            foreach (Edge<T> edge in graph.Edges())
            {
                //Se a distância "voltando" for maior que a distancia "indo"+ peso da aresta
                if (edge.to.distance > edge.from.distance + edge.weight)
                {
                    output = null;
                    return false;
                }
            }

            //Retornamos o grafo gerado a partir da lista de arestas seguras
            output = 
             new AdjacencyMatrix<T>(graph.Vertices(), graph.Edges().FindAll(x => (x.from.Equals(x.to.pi) || x.to.Equals(x.from.pi))), graph.CompareTo(), graph.isDirected);
            return true;
        }

        public static AdjacencyMatrix<T> Dijkstra (Graph<T> graph, T s)
        {
            //Encontro o vértice correspondente a s
            Vertex<T> S = graph.Vertices().Find(x => x.value.Equals(s));
            InitializeSingleSource(graph, S);
            // Q é uma lista de vertices do grafo
            List<Vertex<T>> Q = graph.Vertices();
            //R é uma lista de vertices vazia 
            List<Vertex<T>> R = new List<Vertex<T>>();
            //Enquanto a lista nao estiver vazia
            while (Q.Count != 0)
            {
                //ordena a lista pela distancia
                Q = Q.OrderBy(x => x.distance).ToList();
                // Vértice U recebe o primeiro vértice da lista 
                Vertex<T> U = Q[0];
                //Remove o U da lista Q
                Q.Remove(U);
                //Adiciona o U na lista R
                R.Add(U);
                foreach (Edge<T> edge in graph.Adjacent(U))
                {
                    //define novos pais a partir da função Relax
                    Relax(edge);
                }

            }

            return new AdjacencyMatrix<T>(graph.Vertices(), graph.Edges().FindAll(x => (x.from.Equals(x.to.pi) || x.to.Equals(x.from.pi))), graph.CompareTo(), graph.isDirected);
        }
    }
}
