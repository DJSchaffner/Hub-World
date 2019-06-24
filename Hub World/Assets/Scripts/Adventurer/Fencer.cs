using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fencer : SwordFighter
{
    public Fencer() {
      super(Food.Bread, Drink.Beer, Race.Human);
    }
}
