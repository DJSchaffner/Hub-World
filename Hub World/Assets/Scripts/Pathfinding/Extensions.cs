using System.Collections.Generic;

namespace Pathfinding
{
  public static class Extensions
    {
        public static bool HasNode(this List<Node> list, Node node) {
            return list.Find(n => n.position == node.position) != null;
        }
    }
}