using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public enum BuildingTypes { Tavern = 0, Smith }

public class GameController : MonoBehaviour
{
    public static float TILE_SIZE;
    private const int ADV_POOL_SIZE = 20;

    public GameObject MapObject;
    public GameObject PlayerObject;
    public GameObject[] BuildingTypes;
    public GameObject Adventurer;

    public List<BuildingController> Buildings { get; set; }

    private MapController map;
    private PlayerController player;

    private List<AdventurerController> adventurerPool;
    private int advCoolDown;
    private int timer;

    void Awake()
    {
        Grid grid = MapObject.GetComponent<Grid>();
        map = MapObject.GetComponent<MapController>();
        player = PlayerObject.GetComponent<PlayerController>();

        TILE_SIZE = (int)grid.cellSize.x;

        player.InitPlayerCt(this, map);
        InitBuildings();
        InitAdventurers();

        advCoolDown = 2000;
        timer = advCoolDown;
    }

    void Update()
    {
        timer--;
        if (timer <= 0)
        {
            SpawnAdventurer();
            timer = advCoolDown;
        }
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

    private void InitAdventurers()
    {
        adventurerPool = new List<AdventurerController>();
        for (int i = 0; i < ADV_POOL_SIZE; i++)
        {
            adventurerPool.Add(Instantiate(Adventurer).GetComponent<AdventurerController>());
        }
    }

    private void SpawnAdventurer()
    {
        AdventurerController newAdv = adventurerPool[0];
        adventurerPool.Remove(newAdv);

        newAdv.gameObject.SetActive(true);
        //newAdv.GetPath(map, );
    }
    
}
