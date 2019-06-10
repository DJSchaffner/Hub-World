using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int MAX_ZOOM_OUT = 8;
    private const int MAX_ZOOM_IN = 4;
    private const int CAM_SPEED = 2;
    private const int ZOOM_SPEED = 2;
    private const int DEFAULT_ZOOM = 5;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = DEFAULT_ZOOM;
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

        //Wenn der linke Maus-Button geklickt wurde
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
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

    /**
     * Wird aufgerufen wenn der linke Mausbutton geclickt wurde.
     * Erzeugt einen Ray von der relativen Mausposition aus.
     * Das von dem Ray als erstes erreichte Objekt in der Szene
     * wird auf seine Eigenschaften überprüft.
     * 
     * Falls das Objekt den Tag "interactable" besitzt und einen collider
     * wird es als Gebäude behandelt, da diese ausschließlich diese Eigenschaften besitzen.
     **/
    private void OnClick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //Wenn das angeklickte Object den Tag interactable und einen Collider besitzt 
        if (hit.collider != null && hit.transform.tag == "interactable")
        {
            Debug.Log(hit.transform.gameObject.ToString());
            Debug.Log("was clicked!");

            //Holen der Building-Controller Componente, welche jedes Gebäude besitzt
            BuildingController building = hit.transform.GetComponent<BuildingController>();
            //Gebäude-Spezifische Logik ausführen
            building.InteractWith();
        }
    }
}
