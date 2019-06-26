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
    }   
}