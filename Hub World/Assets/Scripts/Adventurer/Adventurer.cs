using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer
{
    private Food food;
    private Drink drink;
    private int foodPercent;
    private int drinkPercent;
    private Race race;

    
    public Adventurer() {
        race = Human;
        food = Bread;
        drink = Beer;
        foodPercent = 50;
        drinkPercent = 50;
    }

    public Adventurer(Food food, Drink drink, Race race) {
        this.race = race;
        this.drink = drink;
        this.food = food;
        foodPercent = 50;
        drinkPercent = 50;
    }
}
