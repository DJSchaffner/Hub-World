using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingTypes { Tavern = 0, Smith }

public class GameController : MonoBehaviour
{
    public static float TILE_SIZE;

    public GameObject GridObj;
    public GameObject BuildingObj;
    public Sprite[] BuildingTypes;

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
        foreach (Sprite buildingSp in BuildingTypes)
        {
            BuildingController bCont = Instantiate(BuildingObj).GetComponent<BuildingController>();
            bCont.SetBuildingType(buildingSp);
            bCont.gameObject.SetActive(false);
            Buildings.Add(bCont);
        }
    }
    
}
