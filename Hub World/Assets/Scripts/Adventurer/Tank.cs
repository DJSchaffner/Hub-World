using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Adventurer
{
    public Tank() {
        super(Food.Bread, Drink.Beer, Race.Dwarf);
    }
}
