using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer
{
    //Zufriedenheit (<25% debuff >75% buff)
    private int wellBeing = 50;
    //Bevorzugte Nahrung
    private Food food;
    //Bevorzugtes Getränk
    private Drink drink;
    //Hunger, wenn <20%
    private int foodPercent = 50;
    //Durst, wenn <20%
    private int drinkPercent = 50;
    //Rasse
    private Race race;
    //Klasse
    private AdventurerClass adClass;
    //Luxusgut
    private Gear wantedGear;
    //Wie oft wurde das Luxusgut gekauft
    private int amountOfGear = 0;
    //Rüstung
    private Armor armor = Armor.None;
    //Waffe
    private Weapon weapon = Weapon.None;
    //Währung
    private int coins = 50;
    //TODO: weitere Güter (z.B. Heiltränke), so nicht ideal -> Map<Gear, int>? 
    private List<Gear> obstacleGear = new List<Gear>();


    /**
     * Standardkonstruktor
     */
    public Adventurer() {
        adClass = AdventurerClass.None;
        race = Race.Human;
        food = Food.Bread;
        drink = Drink.Beer;
        wantedGear = Gear.Torch;
    }

    /**
     * Konstruktor mit speziellen Eigenschaften
     * @param food bevorzugte Nahrung
     * @param drink bevorzzugtes Getränk
     * @param race Rasse
     * @param adClass Klasse
     */
    public Adventurer(Food food, Drink drink, Race race, AdventurerClass adClass) {
        this.race = race;
        this.drink = drink;
        this.food = food;
        this.adClass = adClass;
        switch (adClass) {
            case AdventurerClass.Archer:
              wantedGear = Gear.Arrow;
              break;
            case AdventurerClass.Berserker:
            case AdventurerClass.Tank:
            case AdventurerClass.Paladin:
              wantedGear = Gear.Healpotion;
              break;
            case AdventurerClass.Crossbowman:
              wantedGear = Gear.Bolt;
              break;
            case AdventurerClass.Fencer:
            case AdventurerClass.SwordFighter:
              wantedGear = Gear.Strengthpotion;
              break;
            case AdventurerClass.Rogue:
              wantedGear = Gear.Whore;
              break;
            case AdventurerClass.Healer:
            case AdventurerClass.Mage:
              wantedGear = Gear.Manapotion;
              break;
            case AdventurerClass.Warlock:
              wantedGear = Gear.Incense;
              break;
            default:
              wantedGear = Gear.Torch;
              break;
        }
    }

    private int checkRange(int value) {
        if (value < 0) {
            return 0;
        } else if (value > 100) {
            return 100;
        } else {
            return value;
        }
    }

    /**
     * Verändert den Zufriedenheitswert, achtet auf einen Bereich zwischen 0 und 100
     * @param percent Zahl, um die der Zufriedenheitswert verändert wird
     */
    private void addWellBeing(int percent) {
        wellBeing = checkRange(wellBeing + percent);
    }

    private void addFoodPercent(int percent) {
        foodPercent = checkRange(foodPercent + percent);
    }

    private void addDrinkPercent(int percent) {
        drinkPercent = checkRange(drinkPercent + percent);
    }

    

    /**
     * Waffe kaufen
     * @param weapon zu kaufende Waffe
     */
    public void buyWeapon(Weapon weapon) {
        //Waffenhändler finden + hingehen
        //if (coins >= Waffenpreis) {
          this.weapon = weapon;
          //coins -= Waffenpreis
          addWellBeing(10);
        //} else {
          addWellBeing(-10);
        //}
        //Random herumlaufen
    }

    /**
     * Rüstung kaufen
     * @param armor zu kaufende Rüstung
     */
    public void buyArmor(Armor armor) {
        //Rüstungshändler finden + hingehen
        //if (coins >= Rüstungspreis) {
            this.armor = armor;
            //coins -= Rüstungspreis
            addWellBeing(10);
        //} else {
            addWellBeing(-10);
        //}
        //Random herumlaufen
    }

    /**
     * Kaufen des benötigten Luxusgutes
     */
    public void buyGear() {
        //Händler des benötigten Luxusgutes finden + hingehen
        //if (coins >= Luxusgutpreis) {
            amountOfGear++;
        //}
        //Random herumlaufen
    }

    /**
     * Kaufen weiterer Güter (z.B. Heiltrank)
     * @param gear zu kaufendes Gut
     */
     public void buyGear(Gear gear) {
         //Händler des Guts finden + hingehen
         //if (coins >= Gutspreis) {
             this.obstacleGear.Add(gear);
         //}
         //Random herumlaufen
     }

    /**
     * 
     */
     public void eatSomething(Food food) {
         //Nahrungshändler finden + hingehen
         //if (coins >= Nahrungspreis) {
             if (food == this.food) {
                 addWellBeing(10);
             } else {
                 addWellBeing(-2);
             }
             addFoodPercent(20);
         //} else {
             addWellBeing(-5);
         //}
         //Random herumlaufen
     }

     public void drinkSomething(Drink drink) {
         //Getränkehändler finden + hingehen
         //if (coins >= Getränkepreis) {
             if (drink == this.drink) {
                 addWellBeing(10);
             } else {
                 addWellBeing(-2);
             }
             addDrinkPercent(20);
         //} else {
             addWellBeing(-5);
         //}
         //Random herumlaufen
     }

}
