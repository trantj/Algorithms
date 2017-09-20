using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            var vertices = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var edges = new[] {Tuple.Create(1,2), Tuple.Create(1,3),
            Tuple.Create(2,4),Tuple.Create(3,5), Tuple.Create(3,6),
            Tuple.Create(4,7), Tuple.Create(5,7), Tuple.Create(5,8),
            Tuple.Create(5,6), Tuple.Create(8,9), Tuple.Create(9,10)};

            var graph = new Graph<int>(vertices, edges);
            var algorithms = new Algorithms();

            var path = new List<int>();

            var startVertex = 3;
            var shortestPath = algorithms.ShortestPathFunction(graph, startVertex);
            foreach (var vertex in vertices)
                Console.WriteLine("Shortest path to {0,2}: {1}", vertex, string.Join(", ", shortestPath(vertex)));

            Console.WriteLine(string.Join(", ", algorithms.DFS(graph, 1, v => path.Add(v))));
            //Console.WriteLine(string.Join(", ", path));

            Console.WriteLine(string.Join(", ", algorithms.BFS(graph, 1, v => path.Add(v))));
            //Console.WriteLine(string.Join(", ", path));

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }

    public class Graph<T>
    {
        public Graph() { }
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T, T> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }
    }

    public class Algorithms
    {
        public HashSet<T> DFS<T>(Graph<T> graph, T start, Action<T> preVisit = null)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var visited = new HashSet<T>();

            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            var stack = new Stack<T>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();

                if (visited.Contains(vertex))
                    continue;

                if (preVisit != null)
                    preVisit(vertex);

                visited.Add(vertex);

                foreach (var neighbor in graph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            return visited;
        }

        public HashSet<T> BFS<T>(Graph<T> graph, T start, Action<T> preVisit = null)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var visited = new HashSet<T>();

            if (!graph.AdjacencyList.ContainsKey(start))
                return visited;

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                    continue;

                if (preVisit != null)
                    preVisit(vertex);

                visited.Add(vertex);

                foreach (var neighbor in graph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            return visited;
        }

        public Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start)
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v => {
                var path = new List<T> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }

}
