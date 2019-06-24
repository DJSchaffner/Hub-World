using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Tank
{
    public Paladin() {
        super(Food.Meat, Drink.Beer, Race.Dwarf);
    }
}
