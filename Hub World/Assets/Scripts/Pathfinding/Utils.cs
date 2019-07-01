using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public static class Utils
    {        
        public static void PrintList<T>(List<T> list) {
            foreach (T element in list) {
                Debug.Log(element);
            }
        }

        public static bool HasDuplicates(List<Node> list) {
            List<Vector3Int> positions = new List<Vector3Int>();

            foreach (Node n in list) {
                if (positions.Contains(n.Position))
                    return true;

                positions.Add(n.Position);
            }

            return false;
        }
    }   
}