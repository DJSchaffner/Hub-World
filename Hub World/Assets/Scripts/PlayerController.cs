using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class PlayerController : MonoBehaviour
{
    private const int MAX_ZOOM_OUT = 28;
    private const int MAX_ZOOM_IN = 10;
    private const int CAM_SPEED = 20;
    private const int ZOOM_SPEED = 10;
    private const int DEFAULT_ZOOM = 15;

    private GameController gameControl;

    private Vector3 placingPos;
    private MapController map;
    private bool isPlacing = false;

    private Camera cam;
    private float screenAspect;
    private float camHeight;
    private float camWidth;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = DEFAULT_ZOOM;

        screenAspect = (float)Screen.width / (float)Screen.height;
        camHeight = cam.orthographicSize * 2;
        float camHalfWidth = screenAspect * cam.orthographicSize;
        camWidth = 2.0f * camHalfWidth;

        placingPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandling();
    }

    public void InitPlayerCt(GameController gameControl, MapController map)
    {
        this.map = map;
        this.gameControl = gameControl;
    }

    /**
     * Wird in der Update-Funktion jeden Frame aufgerufen.
     * Verarbeitet sämtlichen Spieler-Input.
     **/
    private void InputHandling()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            float moveX = Input.GetAxis("Horizontal") * CAM_SPEED * Time.deltaTime;
            float moveY = Input.GetAxis("Vertical") * CAM_SPEED * Time.deltaTime;
            if (map.InBounds(new Vector2(transform.position.x + moveX, transform.position.y + moveY), new Vector2(camWidth, camHeight)))
            {
                transform.Translate(moveX, moveY, 0);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float scrollVal = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SPEED;
            //Wenn die Cam-Size sich in seinen Bereichsgrenzen befindet, kann sie sich größer oder kleiner ziehen(zoomen)
            if (cam.orthographicSize - scrollVal < MAX_ZOOM_OUT && cam.orthographicSize - scrollVal > MAX_ZOOM_IN)
            {
                cam.orthographicSize -= scrollVal;
            }

            camHeight = cam.orthographicSize * 2;
            float camHalfWidth = screenAspect * cam.orthographicSize;
            camWidth = 2.0f * camHalfWidth;
            if (!map.InBounds(transform.position, new Vector2(camWidth, camHeight)))
                transform.position = map.GetBackInBounds(transform.position, new Vector2(camWidth, camHeight));
        }

        HandleBuildingInput();
    }

    private void HandleBuildingInput()
    {
        if (Input.GetMouseButtonDown(0) && isPlacing && map.IsPlacable((int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)BuildingTypes.Tavern].BuildArea))
        {
            map.PlaceObject((int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)BuildingTypes.Tavern].BuildArea);
            isPlacing = false;
        }
        else if (isPlacing)
        {
            placingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            placingPos.z = 0;

            gameControl.Buildings[(int)BuildingTypes.Tavern].transform.position = new Vector3((int)placingPos.x, (int)placingPos.y);

            //TODO: Ressourcen-fressend da jeden Frame!
            if(!map.IsPlacable((int)placingPos.x, (int)placingPos.y, gameControl.Buildings[(int)BuildingTypes.Tavern].BuildArea))
                gameControl.Buildings[(int)BuildingTypes.Tavern].setSpriteColor(Color.red);
            else
                gameControl.Buildings[(int)BuildingTypes.Tavern].setSpriteColor(Color.white);
        }
        else if (!isPlacing && Input.GetKeyDown(KeyCode.B))
        {
            isPlacing = true;
            placingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            placingPos.z = 0;
            gameControl.Buildings[(int)BuildingTypes.Tavern].gameObject.SetActive(true);
            gameControl.Buildings[(int)BuildingTypes.Tavern].transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
