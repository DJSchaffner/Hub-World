using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

/**
 * Spieler-Controller Komponente
 * @author cgt102461: Nicolas Begic
 */
public class PlayerController : MonoBehaviour
{
    //Konstanten für die Eigenschaften der Spieler-Kamera
    private const int MAX_ZOOM_OUT = 35;
    private const int MAX_ZOOM_IN = 10;
    private const int CAM_SPEED = 20;
    private const int ZOOM_SPEED = 10;
    private const int DEFAULT_ZOOM = 15;

    private GameController gameControl;

    //Variablen wichtig für das Platzieren eines Gebäudes
    private Vector3 placingPos;
    private MapController map;
    private bool isPlacing = false;
    private BuildingTypes selectedBuilding;

    //Kamera-Componente der Spieler-Cam und ihre Eigenschaften
    private Camera cam;
    private float screenAspect;
    private float camHeight;
    private float camWidth;

    // 2 Testsounds die bisher nur bei dem Platzieren eines Gebäudes abgespielt werden
    public AudioClip testSound1;
    public AudioClip testSound2;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = DEFAULT_ZOOM;

        //Initialisierung der Größe der Spieler-Cam
        screenAspect = (float)Screen.width / (float)Screen.height;
        camHeight = cam.orthographicSize * 2;
        float camHalfWidth = screenAspect * cam.orthographicSize;
        camWidth = 2.0f * camHalfWidth;

        //Initialisierung der Platzierungs-Variablen
        placingPos = new Vector3(0, 0, 0);
        //TODO: wird sonst über das Menü des Baumeisterhauses gesetzt
        selectedBuilding = BuildingTypes.Tavern;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandling();
    }

    /**
     * Lässt sich eine Referenz auf den GameController 
     * und den MapController des Grids übergeben und speichert diese global.
     * 
     * Zudem wird die Position des Spielers auf die Mitte der Tilemap gesetzt.
     */
    public void InitPlayerCt(GameController gameControl, MapController map)
    {
        this.map = map;
        this.gameControl = gameControl;

        this.transform.position = new Vector3(MapController.MAP_SIZE/2, MapController.MAP_SIZE / 2, transform.position.z);
    }

    /**
     * Wird in der Update-Funktion jeden Frame aufgerufen.
     * Verarbeitet sämtlichen Spieler-Input.
     **/
    private void InputHandling()
    {
        //Wenn der Spieler sich bewegen möchte (W, A, S, D; Pfeiltasten)
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            float moveX = Input.GetAxis("Horizontal") * CAM_SPEED * Time.deltaTime;
            float moveY = Input.GetAxis("Vertical") * CAM_SPEED * Time.deltaTime;
            //Werden die neuen Positions-Daten auf die Bounds der TileMap gechecked
            if (map.InBounds(new Vector2(transform.position.x + moveX, transform.position.y + moveY), new Vector2(camWidth, camHeight)))
            {
                //Wenn innerhalb dieser, kann sich der Spieler in die gewünschte Richtung bewegen 
                transform.Translate(moveX, moveY, 0);
            }
        }

        //Möchte der Spieler die Kamera heran- oder heraus-zoomen
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float scrollVal = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SPEED;
            //Wenn die Cam-Size sich in seinen Bereichsgrenzen befindet, kann sie sich größer oder kleiner ziehen(zoomen)
            if (cam.orthographicSize - scrollVal < MAX_ZOOM_OUT && cam.orthographicSize - scrollVal > MAX_ZOOM_IN)
            {
                cam.orthographicSize -= scrollVal;
            }

            //Die Größen-Variablen der Cam werden an die neue orthographische Größe angepasst
            camHeight = cam.orthographicSize * 2;
            float camHalfWidth = screenAspect * cam.orthographicSize;
            camWidth = 2.0f * camHalfWidth;
            //Wenn sich die Cam nun in ihrer neuen Größe teils außerhalb der Tilemap befindet,
            //wird sie so verschoben, dass sie sich wieder innerhalb der Tilemap befindet.
            if (!map.InBounds(transform.position, new Vector2(camWidth, camHeight)))
                transform.position = map.GetBackInBounds(transform.position, new Vector2(camWidth, camHeight));
        }

        HandleBuildingInput();
    }

    /**
     * Händelt sämtlichen Input das Platzieren von Gebäude betreffend 
     */
    private void HandleBuildingInput()
    {
        //Wenn der Spieler ein Gebäude bauen möchte (B-Taste)
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Und nicht bereits dabei ist eines zu platzieren
            if (!isPlacing)
            {
                isPlacing = true;
                placingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                placingPos.z = 0;
                //Wird das Gebäude an der Position seiner Maus angezeigt
                gameControl.Buildings[(int)selectedBuilding].gameObject.SetActive(true);
                gameControl.Buildings[(int)selectedBuilding].transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                //Falls er bereits dabei ist, wird der Vorgang abgebrochen
                gameControl.Buildings[(int)selectedBuilding].gameObject.SetActive(false);
                isPlacing = false;
            }
        }
    
        //Falls der Spieler das Platzieren eines Gebäudes gestartet hat
        if (isPlacing)
        {
            //Und die linke Maustaste drückt, während er sich mit seiner Maus auf einer für das Gebäude platzierbaren Position befindet
            if (Input.GetMouseButtonDown(0) && map.IsPlacable((int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)selectedBuilding].BuildArea))
            {
                //Wird das Gebäude dort platziert und auf der Tilemap die wichtigen Tiles blockiert
                map.PlaceObject((int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)selectedBuilding].BuildArea);
                isPlacing = false;
                gameControl.CompletedBuildings.Add(selectedBuilding);

                foreach (AdventurerController adventurer in gameControl.AdventurerPool)
                {
                    if (adventurer.Target != Vector3Int.zero && map.pathBlocked(adventurer.NewPath, (int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)selectedBuilding].BuildArea))
                    { 
                        adventurer.StartPath(map.GetMap()[0], adventurer.Target);
                    }
                }

                // Abspielen eines Testsounds
                SoundManager.instance.RandomizeSfx(testSound1, testSound2);
            }
            else
            {
                //Ansonsten wird stetig, solange der Spieler die Gebäude-Platzierung nicht abbricht,
                //das Gebäude auf dem Grid an der Position der Maus angezeigt
                placingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                placingPos.z = 0;

                gameControl.Buildings[(int)selectedBuilding].transform.position = new Vector3((int)placingPos.x, (int)placingPos.y);

                //Wenn das Gebäude nicht an der derzeitigen Position platzierbar ist, wird es rot eingefärbt.
                //TODO: Ressourcen-fressend da jeden Frame!
                if (!map.IsPlacable((int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)selectedBuilding].BuildArea))
                    gameControl.Buildings[(int)selectedBuilding].setSpriteColor(Color.red);
                else
                    gameControl.Buildings[(int)selectedBuilding].setSpriteColor(Color.white);
            }
        }
    }
}
