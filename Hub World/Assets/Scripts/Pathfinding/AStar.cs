using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
  public static class AStar
    {
        private static Vector3Int[] NEIGHBORS = new [] {
            new Vector3Int(-1, 0, 0),   // North
            new Vector3Int(0, 1, 0),    // East
            new Vector3Int(1, 0, 0),    // South
            new Vector3Int(0, -1, 0),   // West
        };

        public static List<Vector3Int> FindPath(Tilemap map, Vector3Int start, Vector3Int end) {
            Graph graph = new Graph(map, start, end);
            List<Node> library = new List<Node>();
            List<Node> done = new List<Node>();
            List<Vector3Int> result = new List<Vector3Int>();
            Node current;

            // Init library
            library.Add(new Node(start, null, 0, graph.cells[start.y, start.x].heuristic));
        
            while (!IsFinished(library, start, end)) {
                // Get current best candidate and move it to done
                current = library.Last();
                done.Add(current);
                library.RemoveAt(library.Count - 1);
                graph.cells[current.position.y, current.position.x].completed = true;

                // Get new candidates, insert them and sort the library
                library.AddRange(GetNeighbors(graph, end, library, current));
                library.Sort();
            }

            // Calculation finished
            foreach (Node n in done) {
                result.Add(n.position);
            }

            return result;
        }

        public static bool IsFinished(List<Node> library, Vector3Int start, Vector3Int end) {
            return library.Count == 0 || library.Last().position == end; 
        }

        public static IEnumerable<Node> GetNeighbors(Graph graph, Vector3Int end, List<Node> list, Node current) {
            foreach (Vector3Int neighbor in NEIGHBORS) {
                Node temp = new Node(current.position + neighbor, current, current.traveled + 1, graph.cells[current.position.y, current.position.x].heuristic);

                if (Utils.IsInbounds(graph, temp) && !list.HasNode(temp))
                    yield return temp;
            }
        }
    }   
}
