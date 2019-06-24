using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class PlayerController : MonoBehaviour
{
    private const int MAX_ZOOM_OUT = 50;
    private const int MAX_ZOOM_IN = 10;
    private const int CAM_SPEED = 20;
    private const int ZOOM_SPEED = 10;
    private const int DEFAULT_ZOOM = 15;

    public GameObject MapObject;
    public GameObject Tavern;
    public GameController GameControl;

    Vector3 placingPos;
    private MapController map;
    private bool isPlacing = false;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = DEFAULT_ZOOM;

        map = MapObject.GetComponent<MapController>();
        placingPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandling();
    }

    /**
     * Wird in der Update-Funktion jeden Frame aufgerufen.
     * Verarbeitet sämtlichen Spieler-Input.
     **/
    private void InputHandling()
    {
        transform.Translate(Input.GetAxis("Horizontal") * CAM_SPEED * Time.deltaTime, Input.GetAxis("Vertical") * CAM_SPEED * Time.deltaTime, 0);

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float scrollVal = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SPEED;
            //Wenn die Cam-Size sich in seinen Bereichsgrenzen befindet, kann sie sich größer oder kleiner ziehen(zoomen)
            if (cam.orthographicSize - scrollVal < MAX_ZOOM_OUT && cam.orthographicSize - scrollVal > MAX_ZOOM_IN)
            {
                cam.orthographicSize -= scrollVal;
            }
        }

        if (Input.GetMouseButtonDown(0) && isPlacing)
        {
            map.PlaceObject((int)placingPos.x, (int)placingPos.y, GameControl.Buildings[0].BuildArea);
            isPlacing = false;
        }
        else if (isPlacing)
        {
            placingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            placingPos.z = 0;

            GameControl.Buildings[0].transform.position = new Vector3((int)placingPos.x, (int)placingPos.y);
        }
        else if (!isPlacing && Input.GetKeyDown(KeyCode.B))
        {
            isPlacing = true;
            placingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            placingPos.z = 0;
            GameControl.Buildings[0].gameObject.SetActive(true);
            GameControl.Buildings[0].transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
