using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public static class Extensions
    {
        /// <summary>
        /// Extension method for Node list to see if it contains a node (Check via position)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool HasNode(this List<Node> list, Node node) {
            return list.Find(n => n.Position == node.Position) != null;
        }

        /// <summary>
        /// Extension method for Node list to add or update a value in that list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// Extension method for Node list to add or update a range of values in that list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="values"></param>
        public static void AddOrUpdateRangeSorted(this List<Node> list, IEnumerable<Node> values) {
            if (values == null)
                return;

            foreach(Node e in values) {
                list.AddOrUpdateSorted(e);
            }
        }
    }
}