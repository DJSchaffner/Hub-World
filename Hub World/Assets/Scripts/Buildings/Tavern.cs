using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : BuildingController
{
    public GameObject TavernMenu;

    /*
    private List<Worker> hiredWorkers;
    private List<Worker> availableWorkers;
    private List<Item> foodAndDrinks;
    */


    public override void InteractWith()
    {
        Debug.Log("Opening my Tavern-Menues!");
    }

    private void showMenu()
    {
        TavernMenu.SetActive(true);
    }
}
