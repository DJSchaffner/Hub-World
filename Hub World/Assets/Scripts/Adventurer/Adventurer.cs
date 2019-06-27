using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Abenteurer
 */
public class Adventurer
{
    //Zufriedenheit
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


    //Sonstige Gegenstände (z.B. Heiltränke) 
    private Dictionary<Gear, int> otherGear = new Dictionary<Gear, int>();

    
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
            case AdventurerClass.None:
               wantedGear = Gear.Torch;
               break;
        }
    }

    /**
     * Berechnet und setzt den aktuellen Zufriedenheitswert
     */
    private void calculateWellBeing() {
        int wellBeing = 0;

        if (foodPercent > 95) {
            wellBeing += 20;
        } else if (foodPercent > 75) {
            wellBeing += 12;
        } else if (foodPercent > 50) {
            wellBeing += 8;
        } else if (foodPercent > 25) {
            wellBeing += 4;
        }

        if (drinkPercent > 95) {
            wellBeing += 20;
        } else if (drinkPercent > 75) {
            wellBeing += 12;
        } else if (drinkPercent > 50) {
            wellBeing += 8;
        } else if (drinkPercent > 25) {
            wellBeing += 4;
        }

        if (armor != Armor.None) {
            wellBeing += 20;
        }

        if (weapon != Weapon.None) {
            wellBeing += 20;
        }

        if (amountOfGear > 100) {
            wellBeing += 20;
        } else if (amountOfGear > 75) {
            wellBeing += 12;
        } else if (amountOfGear > 50) {
            wellBeing += 8;
        } else if (amountOfGear > 25) {
            wellBeing += 4;
        }
        this.wellBeing = wellBeing;
    }

    /**
     * Prüft, ob eine Zahl zwischen 0 und 100 ist
     * @param value zu prüfende Zahl
     * @return Zahl < 0 -> 0; Zahl > 100 -> 100; sonst -> Zahl
     */
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
     * Verändert den Nahrungswert, achtet auf einen Bereich zwischen 0 und 100
     * @param percent Zahl, um die der Nahrungswert verändert wird
     */
    private void addWellBeing(int percent) {
        wellBeing = checkRange(wellBeing + percent);
    }

    private void addFoodPercent(int percent) {
        foodPercent = checkRange(foodPercent + percent);
    }

    /**
     * Verändert den durstwert, achtet auf einen Bereich zwischen 0 und 100
     * @param percent Zahl, um die der Durstwert verändert wird
     */
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
            //coins -= Luxusgutspreis
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
            if(otherGear.ContainsKey(gear)) {
                this.otherGear[gear]++;
            } else {
                otherGear.Add(gear, 1);
            }
            //coins -= gearpreis
         //}
         //Random herumlaufen
     }

    /**
     * Nahrungsaufnahme
     * @param food Nahrung
     */
     public void eatSomething(Food food) {
         //Nahrungshändler finden + hingehen
         //if (coins >= Nahrungspreis) {
             addFoodPercent(20);
             //coins -= Nahrungspreis
         //}
         //Random herumlaufen
     }

    /**
     * Trinken
     * @param drink Getränk
     */
     public void drinkSomething(Drink drink) {
         //Getränkehändler finden + hingehen
         //if (coins >= Getränkepreis) {
             addDrinkPercent(20);
             //coins -= Getränkepreis
         //}
         //Random herumlaufen
     }

    /**
     * Abenteurer bekommt Münzen (z.B. durch Dungeonbesuch)
     * @param coins Münzen, die der Abenteurer bekommt
     */
     public void earnCoins(int coins) {
         this.coins += coins;
     }

}
