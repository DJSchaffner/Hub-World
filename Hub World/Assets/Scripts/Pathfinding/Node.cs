using UnityEngine;
using System;

namespace Pathfinding
{
  public class Node : IComparable
    {
        public Vector3Int Position { get; set; }
        public Node Previous       { get; set; }
        public float Traveled      { get; set; }
        public float Heuristic     { get; set; }

        public Node (Vector3Int position, Node previous, float traveled, float heuristic) {
            this.Position = position;
            this.Previous = previous;
            this.Traveled = traveled;
            this.Heuristic = heuristic;
        }

        public float GetTotal() {
            return this.Traveled + this.Heuristic;
        }

        public int CompareTo(object obj) {
            if (obj == null)
                return 1;

            Node other = obj as Node;

            return this.GetTotal().CompareTo(other.GetTotal());
        }
    }
}