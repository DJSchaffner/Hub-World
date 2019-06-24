using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : SwordFighter
{
    public Rogue() {
        super(Food.Bread, Drink.Beer, Race.Human);
    }
}
