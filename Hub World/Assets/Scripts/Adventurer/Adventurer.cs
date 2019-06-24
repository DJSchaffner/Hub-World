using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer
{
    private Food food;
    private Drink drink;
    private int foodPercent;
    private int drinkPercent;

    
    public Adventurer() {
        food = Bread;
        drink = Beer;
        foodPercent = 50;
        drinkPercent = 50;
    }
}
