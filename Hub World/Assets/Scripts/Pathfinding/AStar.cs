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

        // @TODO Sometimes last tile is off by 1
        /// <summary>
        /// Finds the shortest path from start to end ond a given tilemap
        /// </summary>
        /// <param name="map"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Vector3Int> FindPath(Tilemap map, Vector3Int start, Vector3Int end) {
            Graph graph = new Graph(map, start, end);
            List<Node> library = new List<Node>();
            List<Node> done = new List<Node>();
            Node current;
            
            // Init library
            library.Add(new Node(start, default(Vector3Int), 0, graph.GetCell(start).Heuristic));
        
            while (!IsFinished(library, end)) {
                // Get current best candidate and move it to done
                current = library.First();
                library.RemoveAt(0);
                done.AddOrUpdateSorted(current);
                graph.GetCell(current.Position).IsCompleted = true;

                // Get new candidates, insert them
                library.AddOrUpdateRangeSorted(GetNeighbors(graph, end, library, current));
            }

            // Library is empty? No way found
            if (library.Count == 0) {
                return null;
            }

            // Add dest to done
            done.AddOrUpdateSorted(library.First());

            // Convert done library to graph path
            return GetFinalPath(graph, done);;
        }

        /// <summary>
        /// Checks if Node library and an end Vector are finished
        /// </summary>
        /// <param name="library"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool IsFinished(List<Node> library, Vector3Int end) {
            return library.Count == 0 || library.First().Position == end; 
        }

        private IEnumerable<Node> GetNeighbors(Graph graph, Vector3Int end, List<Node> list, Node current) {
            foreach (Vector3Int neighbor in NEIGHBORS) {
                // Neighbor should be in bounds, not be blocked and not be completed yet
                Vector3Int position = current.Position + neighbor;
                if (graph.IsInbounds(position) && !graph.GetCell(position).IsBlocked && !graph.GetCell(position).IsCompleted ) {
                    Node temp = new Node(position, current.Position, current.Traveled + 1, graph.GetCell(position).Heuristic);

                    if (!list.HasNode(temp)){
                        yield return temp;
                    }
                }                
            }
        }

        /// <summary>
        /// Generates the Vector list from a given graph and a completely calculated Node list
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        private List<Vector3Int> GetFinalPath(Graph graph, List<Node> done) {
            Node temp = done.Find(n => n.Position == graph.End);
            List<Vector3Int> result = new List<Vector3Int>();

            // Element exists?
            if (temp != null) {
                while (temp.Position != graph.Start) {
                    result.Add(temp.Position);
                    temp = done.Find(n => n.Position == temp.Previous);
                }
            }

            // Reverse result so list is from start to end
            result.Reverse();

            return result;
        }
    }   
}
