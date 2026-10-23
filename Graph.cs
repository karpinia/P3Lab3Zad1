using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public abstract class Graph
    {
        public int VertexCount {  get; }
        
        protected Graph(int vertexCount)
        {
            VertexCount = vertexCount;
        }

        public abstract List<int> GetOutVertices(int vertex);
        public abstract void AddEdge(int from, int to);
        public void Display()
        {
            for (int i = 0; i < VertexCount; i++) {
                List<int> outVertices = GetOutVertices(i);
                if (outVertices == null) { 
                    outVertices = new List<int>();
                }
                Console.WriteLine($"{i} - > [{string.Join(",", outVertices)}]");
            }
        }
        public void DepthFirstSearch(int start, IVertexVisitor visitor)
        {
            // 1. Tworzymy tablicę do śledzenia odwiedzonych wierzchołków.
            // Rozmiar to liczba wszystkich wierzchołków.
            // W C# nowa tablica bool[] jest domyślnie wypełniona wartościami false.
            bool[] visited = new bool[VertexCount];

            // 2. Uruchamiamy rekurencyjne przeszukiwanie zaczynając od wierzchołka 'start'
            DepthFirstSearchRecursive(start, visited, visitor);
        }
        private void DepthFirstSearchRecursive(int vertex, bool[] visited, IVertexVisitor visitor)
        {
            // 1. Zaznaczamy obecny wierzchołek jako odwiedzony, 
            // żebyśmy nie chodzili w kółko, jeśli w grafie są cykle.
            visited[vertex] = true;

            // 2. Wchodzimy do wierzchołka - odpalamy PreVisit
            visitor.PreVisit(vertex);

            // 3. Pobieramy wszystkich sąsiadów tego wierzchołka
            List<int> neighbors = GetOutVertices(vertex);

            // 4. Przeszukujemy sąsiadów
            if (neighbors != null) // Upewniamy się, że wierzchołek ma jakichś sąsiadów
            {
                foreach (int neighbor in neighbors)
                {
                    // Jeśli sąsiad NIE był jeszcze odwiedzony, wchodzimy do niego (zejście rekurencyjne)
                    if (!visited[neighbor])
                    {
                        DepthFirstSearchRecursive(neighbor, visited, visitor);
                    }
                }
            }

            // 5. Sprawdziliśmy wszystkie drogi wychodzące z tego wierzchołka.
            // Wycofujemy się - odpalamy PostVisit.
            visitor.PostVisit(vertex);
        }
    }

    public class AdjacencyListGraph : Graph
    {
        private List<int>[] AdjacencyList;
        public AdjacencyListGraph(int vertexCount) : base(vertexCount) { 
            AdjacencyList = new List<int>[vertexCount];
            for (int i = 0; i < VertexCount; i++) { 
                AdjacencyList[i] = new List<int>();
            }
        }
        public override List<int> GetOutVertices(int vertex)
        {
            return AdjacencyList[vertex];
        }
        public override void AddEdge(int from, int to)
        {
            AdjacencyList[from].Add(to);
        }
    }

    public class AdjacencyMatrixGraph : Graph
    {
        private bool[,] AdjacencyMatrix;
        
        public AdjacencyMatrixGraph(int vertexCount) : base(vertexCount)
        {
            AdjacencyMatrix = new bool[vertexCount, vertexCount];
        }
        public override void AddEdge(int from, int to)
        {
            AdjacencyMatrix[from,to] = true; 
        }
        public override List<int> GetOutVertices(int vertex)
        {
            List<int> lista = new List<int>();
            for (int i = 0; i < VertexCount; i++) {
                if (AdjacencyMatrix[vertex,i])
                {
                    lista.Add(i);
                }    
            }
            return lista;
        }
    }
}
