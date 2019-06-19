using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public static class Utils
    {
        public static bool IsInbounds(Graph graph, Node node) {
            return  node.position.x >= 0 && node.position.x < graph.width && 
                    node.position.y >= 0 && node.position.y < graph.height;
        }
        
        public static void PrintList<T>(List<T> list) {
            foreach (T element in list) {
                Debug.Log(element);
            }
        }
    }   
}