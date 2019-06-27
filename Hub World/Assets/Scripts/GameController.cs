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
    public GameObject[] BuildingObjects;
    public GameObject Adventurer;

    public List<BuildingController> Buildings { get; set; }
    //TODO: Eventuell besser ne BuildingController Liste zu machen und hin und her zu adden/removen
    public List<BuildingTypes> completedBuildings { get; set; }

    private MapController map;
    private PlayerController player;

    private List<AdventurerController> adventurerPool;
    private int advCoolDown;
    private float timer;

    void Awake()
    {
        Grid grid = MapObject.GetComponent<Grid>();
        map = MapObject.GetComponent<MapController>();
        player = PlayerObject.GetComponent<PlayerController>();

        TILE_SIZE = (int)grid.cellSize.x;

        InitAdventurers();
        advCoolDown = 5;
        timer = advCoolDown;

        player.InitPlayerCt(this, map);
        InitBuildings();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnAdventurer();
            timer = advCoolDown;
        }
    }

    private void InitBuildings()
    {
        Buildings = new List<BuildingController>();
        completedBuildings = new List<BuildingTypes>();
        foreach (GameObject building in BuildingObjects)
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
        int spawnPointInd = Random.Range(0, map.SpawnPoints.GetLength(1));
        int spawnSide = Random.Range(0, 2);
        newAdv.transform.position = (Vector3Int)map.SpawnPoints[spawnSide, spawnPointInd];

        if (completedBuildings.Contains(BuildingTypes.Tavern))
        {
            Vector3 target = Buildings[(int)BuildingTypes.Tavern].Entrance.position;
            Vector3Int targetCell = new Vector3Int((int)target.x, (int)target.y, 0);
            newAdv.GetPath(map.GetMap()[0], targetCell);
        }
    }
    
}
