using UnityEngine;
using System;

namespace Pathfinding
{
  public class Node : IComparable
    {
        public Vector3Int position { get; set; }
        public Node previous       { get; set; }
        public float traveled      { get; set; }
        public float heuristic     { get; set; }

        public Node (Vector3Int position, Node previous, float traveled, float heuristic) {
            this.position = position;
            this.previous = previous;
            this.traveled = traveled;
            this.heuristic = heuristic;
        }

        public float GetTotal() {
            return this.traveled + this.heuristic;
        }

        public int CompareTo(object obj) {
            if (obj == null)
                return 1;

            Node other = obj as Node;

            return this.GetTotal().CompareTo(other.GetTotal());
        }
    }
}