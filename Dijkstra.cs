using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    class Dijkstra
    {
        int V = 9;
        int minDistance(int[] dist, bool[] sptSet)
        {
            // Initialize min value
            int min = 100000, min_index = 0;

            for (int v = 0; v < V; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v]; min_index = v;
                }


            return min_index;
        }

        // Function to print shortest path from source to j
        // using parent array
        void printPath(int[] parent, int j)
        {
            // Base Case : If j is source
            if (parent[j] == -1)
                return;

            printPath(parent, parent[j]);

            Console.Write("{0}", j);
        }

        // A utility function to print the constructed distance
        // array
        void printSolution(int[] dist, int n, int[] parent)
        {
            int src = 0;
            Console.Write("Vertex\t Distance\tPath"); //printf("Vertex\t Distance\tPath");
            for (int i = 1; i < V; i++)
            {
                Console.Write("\n {0} -> {1}\t\t{2} \t\t{3}", src, i, dist[i], src); //printf("\n%d -> %d \t\t %d\t\t%d ", src, i, dist[i], src);
                printPath(parent, i);
            }
        }

        // Funtion that implements Dijkstra's single source shortest path
        // algorithm for a graph represented using adjacency matrix
        // representation
        void dijkstra(int[,] graph, int src)
        {
            int[] dist = new int[V]; // The output array. dist[i] will hold
            // the shortest distance from src to i

            // sptSet[i] will true if vertex i is included / in shortest
            // path tree or shortest distance from src to i is finalized
            bool[] sptSet = new bool[V];

            // Parent array to store shortest path tree
            int[] parent = new int[V];

            // Initialize all distances as INFINITE and stpSet[] as false
            for (int i = 0; i < V; i++)
            {
                parent[0] = -1;
                dist[i] = 100000;
                sptSet[i] = false;
            }

            // Distance of source vertex from itself is always 0
            dist[src] = 0;

            // Find shortest path for all vertices
            for (int count = 0; count < V - 1; count++)
            {
                // Pick the minimum distance vertex from the set of
                // vertices not yet processed. u is always equal to src
                // in first iteration.
                int u = minDistance(dist, sptSet);

                // Mark the picked vertex as processed
                sptSet[u] = true;

                // Update dist value of the adjacent vertices of the
                // picked vertex.
                for (int v = 0; v < V; v++)

                    // Update dist[v] only if is not in sptSet, there is
                    // an edge from u to v, and total weight of path from
                    // src to v through u is smaller than current value of
                    // dist[v]
                    if (!sptSet[v] && graph[u, v] != 0 &&
                        dist[u] + graph[u, v] < dist[v])
                    {
                        parent[v] = u;
                        dist[v] = dist[u] + graph[u, v];
                    }
            }

            // print the constructed distance array
            printSolution(dist, V, parent);
        }

        // driver program to test above function
        public int[,] graph { get; set; }
        public int DoIt()
        {
            /* Let us create the example graph discussed above */
            int[,] graph = {{0, 4, 0, 0, 0, 0, 0, 8, 0},
					{4, 0, 8, 0, 0, 0, 0, 11, 0},
					{0, 8, 0, 7, 0, 4, 0, 0, 2},
					{0, 0, 7, 0, 9, 14, 0, 0, 0},
					{0, 0, 0, 9, 0, 10, 0, 0, 0},
					{0, 0, 4, 0, 10, 0, 2, 0, 0},
					{0, 0, 0, 14, 0, 2, 0, 1, 6},
					{8, 11, 0, 0, 0, 0, 1, 0, 7},
					{0, 0, 2, 0, 0, 0, 6, 7, 0}
					};

            dijkstra(graph, 0);

            return 0;
        }

    }
}
