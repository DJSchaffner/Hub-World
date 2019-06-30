using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tavernen-Controller Komponente
 * @author cgt102461: Nicolas Begic
 */
public class Tavern : BuildingController
{
    //Objekt des Tavernen-Menü-UIs
    public GameObject TavernMenu;

    /* TODO: Relevante Informationen für das Tavernen-Menü
    private List<Worker> hiredWorkers;
    private List<Worker> availableWorkers;
    private List<Item> foodAndDrinks;
    */

    /**
     * Überschriebene Methode aus dem BuildingController,
     * welche bei Klick mit der linken Maustaste auf die Taverne aufgerufen wird.
     * 
     * Zeigt das Menü der Taverne an.
     */
    public override void InteractWith()
    {
        Debug.Log("Opening my Tavern-Menues!");
    }

    /**
     * Füllt das Tavernen-Menü mit allen relevanten Informationen
     */
    private void ShowMenu()
    {
        TavernMenu.SetActive(true);
    }
}
