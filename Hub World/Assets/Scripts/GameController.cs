using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

//Verschiedene Gebäude-Typen
public enum BuildingTypes { Tavern = 0, Smith }

/**
 * Game-Controller Komponente
 * @author cgt102461: Nicolas Begic
 */
public class GameController : MonoBehaviour
{
    //Größe eines Feldes auf der TileMap in WorldSpace Einheit
    public static float TILE_SIZE;
    //Anzahl an Adventurern, die maximal gleichzeitig auf der Karte existieren sollen
    private const int ADV_POOL_SIZE = 20;

    //GameObject des Grids
    public GameObject MapObject;
    //GameObject des Spielers und seiner Cam
    public GameObject PlayerObject;
    //Sammlung aller Prefabs sämtlicher Gebäudetypen
    public GameObject[] BuildingObjects;
    //Prefab eines Abenteurers
    public GameObject Adventurer;
    //Leeres GameObject innerhalb des GameManagers in dem der ObjectPool gelagert wird
    public GameObject ObjectPool;

    //Liste aller Gebäude-Controller der Gebäude, die sich im ObjectPool befinden
    public List<BuildingController> Buildings { get; set; }
    //Liste aller Gebäude-Typen, die der Spieler bereits gebaut hat
    //TODO: Eventuell besser ne BuildingController Liste zu machen und hin und her zu adden/removen
    public List<BuildingTypes> completedBuildings { get; set; }

    //Controller-Komponente des Grids
    private MapController map;
    //Controller-Komponente des Spielers
    private PlayerController player;

    //Liste aller Controller der Adventurer im ObjectPool
    private List<AdventurerController> adventurerPool;
    //Zeit in Sekunden, in denen ein neuer Abenteurer erscheinen soll
    private int advCoolDown;
    //Timer fürs runterzählen der Gametime
    private float timer;

    //Wird vor Start bei Entstehung des GameManager-Objekts aufgerufen
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

    //Wird jeden Frame aufgerufen
    void Update()
    {
        //Herunterzählen des Timers
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //Wenn die advCoolDown-Zeit abgelaufen ist, wird ein neuer Abenteurer gespawned
            SpawnAdventurer();
            //Und der timer zurück gesetzt
            timer = advCoolDown;
        }
    }

    /**
     * Initiiert die Buildings-Liste
     * Erstellt alle Gebäude und berechnet ihren Umfang auf dem Grid
     */
    private void InitBuildings()
    {
        Buildings = new List<BuildingController>();
        completedBuildings = new List<BuildingTypes>();
        foreach (GameObject building in BuildingObjects)
        {
            BuildingController bCont = Instantiate(building).GetComponent<BuildingController>();
            bCont.transform.parent = ObjectPool.transform;
            bCont.SetBuildingType(bCont.GetComponent<SpriteRenderer>().sprite);
            bCont.gameObject.SetActive(false);
            Buildings.Add(bCont);
        }
    }

    /**
     * Initiiert den Adventurer-Pool
     * Erstellt alle Adventurer und speichert ihre Controller in einer Liste
     */
    private void InitAdventurers()
    {
        adventurerPool = new List<AdventurerController>();
        for (int i = 0; i < ADV_POOL_SIZE; i++)
        {
            GameObject newAdv = Instantiate(Adventurer);
            newAdv.transform.parent = ObjectPool.transform;
            adventurerPool.Add(newAdv.GetComponent<AdventurerController>());
        }
    }

    /**
     * Lässt einen neuen Adventurer erscheinen
     * 
     * Nimmt sich den ersten Adventurer-Controller aus ihrem Pool.
     * Lässt diesen auf einer zufälligen Position unter den definierten SpawnPoints
     * erscheinen und gibt ihm ein Ziel, abhängig davon welche Gebäude der Spieler
     * bereits errichtet hat.
     */
    private void SpawnAdventurer()
    {
        AdventurerController newAdv = adventurerPool[0];
        adventurerPool.Remove(newAdv);

        newAdv.gameObject.SetActive(true);
        int spawnPointInd = Random.Range(0, map.SpawnPoints.GetLength(1));
        int spawnSide = Random.Range(0, 2);
        newAdv.transform.position = (Vector3Int)map.SpawnPoints[spawnSide, spawnPointInd];

        //TODO: Immer als erstes die Taverne ansteuern?
        if (completedBuildings.Contains(BuildingTypes.Tavern))
        {
            Vector3 target = Buildings[(int)BuildingTypes.Tavern].Entrance.position;
            Vector3Int targetCell = new Vector3Int((int)target.x, (int)target.y, 0);
            newAdv.StartPath(map.GetMap()[0], targetCell);
        }
    }
    
}
