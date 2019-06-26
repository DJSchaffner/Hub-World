using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingTypes { Tavern = 0, Smith }

public class GameController : MonoBehaviour
{
    public static float TILE_SIZE;

    public GameObject GridObj;
    public GameObject[] BuildingTypes;

    public List<BuildingController> Buildings { get; set; }

    private Grid grid;

    void Awake()
    {
        grid = GridObj.GetComponent<Grid>();
        TILE_SIZE = (int)grid.cellSize.x;

        InitBuildings();
    }

    private void InitBuildings()
    {
        Buildings = new List<BuildingController>();
        foreach (GameObject building in BuildingTypes)
        {
            BuildingController bCont = Instantiate(building).GetComponent<BuildingController>();
            bCont.SetBuildingType(bCont.GetComponent<SpriteRenderer>().sprite);
            bCont.gameObject.SetActive(false);
            Buildings.Add(bCont);
        }
    }
    
}
