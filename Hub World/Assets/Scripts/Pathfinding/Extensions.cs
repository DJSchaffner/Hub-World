using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public static class Extensions
    {
        public static bool HasNode(this List<Node> list, Node node) {
            return list.Find(n => n.Position == node.Position) != null;
        }

        public static void AddOrUpdateSorted(this List<Node> list, Node value) {
            // Node already exists, try to update
            if (list.HasNode(value)) {
                int index = list.FindIndex(n => n.Position == value.Position);

                // Only update if new node has lower total value
                if (value.GetTotal() < list[index].GetTotal())
                    list[index] = value;
            }
            else {
                int pos = list.BinarySearch(value);

                list.Insert((pos >= 0) ? pos : ~pos, value);
            }      
        }

        public static void AddOrUpdateRangeSorted(this List<Node> list, IEnumerable<Node> values) {
            if (values == null)
                return;

            foreach(Node e in values) {
                list.AddOrUpdateSorted(e);
            }
        }
    }
}