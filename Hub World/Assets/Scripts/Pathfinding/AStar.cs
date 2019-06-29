using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
  public class AStar
    {
        private Vector3Int[] NEIGHBORS = new [] {
            new Vector3Int(0, 1, 0),   // North
            new Vector3Int(1, 0, 0),    // East
            new Vector3Int(0, -1, 0),    // South
            new Vector3Int(-1, 0, 0),   // West
        };

        public List<Vector3Int> FindPath(Tilemap map, Vector3Int start, Vector3Int end) {
            Graph graph = new Graph(map, start, end);
            List<Node> library = new List<Node>();
            List<Node> done = new List<Node>();
            List<Vector3Int> result = new List<Vector3Int>();
            Node current;
            Debug.Log(start);
            // Init library
            library.Add(new Node(start, null, 0, graph.GetCell(start).Heuristic));
        
            while (!IsFinished(library, start, end)) {
                // Get current best candidate and move it to done
                current = library.First();
                done.Add(current);
                library.RemoveAt(0);
                graph.GetCell(current.Position).IsCompleted = true;

                // Get new candidates, insert them and sort the library
                library.AddRange(GetNeighbors(graph, end, library, current));
                // Sort library (lowest total first, highest last)
                library.Sort();
            }

            // Calculation finished
            foreach (Node n in done) {
                result.Add(n.Position);
            }

            Utils.PrintList(result);

            return result;
        }

        public bool IsFinished(List<Node> library, Vector3Int start, Vector3Int end) {
            return library.Count == 0 || library.First().Position == end; 
        }

        private IEnumerable<Node> GetNeighbors(Graph graph, Vector3Int end, List<Node> list, Node current) {
            foreach (Vector3Int neighbor in NEIGHBORS) {
                if (graph.IsInbounds(current.Position + neighbor) && !graph.GetCell(current.Position + neighbor).IsBlocked) {
                    Node temp;
                    try
                    {
                        temp = new Node(current.Position + neighbor, current, current.Traveled + 1, graph.GetCell(current.Position + neighbor).Heuristic);
                    }
                    catch (System.IndexOutOfRangeException)
                    {

                        throw;
                    }

                    if (!list.HasNode(temp))
                        yield return temp;
                }                
            }
        }
    }   
}
