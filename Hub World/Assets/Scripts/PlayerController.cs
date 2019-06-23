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

    private MapController map;
    private bool isPlacing = false;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = DEFAULT_ZOOM;

        map = MapObject.GetComponent<MapController>();
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

        if (Input.GetMouseButtonDown(0) && isPlacing)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Instantiate(Tavern, pos, Quaternion.identity);
            map.PlaceObject((int)pos.x, (int)pos.y, Tavern.GetComponent<BoxCollider2D>().size);
            isPlacing = false;
        }
        else if (!isPlacing && Input.GetKeyDown(KeyCode.B))
        {
            isPlacing = true;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float scrollVal = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SPEED;
            //Wenn die Cam-Size sich in seinen Bereichsgrenzen befindet, kann sie sich größer oder kleiner ziehen(zoomen)
            if (cam.orthographicSize - scrollVal < MAX_ZOOM_OUT && cam.orthographicSize - scrollVal > MAX_ZOOM_IN)
            {
                cam.orthographicSize -= scrollVal;
            }
        }
    }
}
