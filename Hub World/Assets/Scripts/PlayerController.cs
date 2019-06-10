using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int CamSpeed;
    public int ZoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
 
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
    void OnClick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //Wenn das angeklickte Object den Tag interactable und einen Collider besitzt 
        if (hit.collider != null && hit.transform.tag == "interactable")
        {
            Debug.Log(hit.transform.gameObject.ToString());
            Debug.Log("was clicked!");

            BuildingController building = hit.transform.GetComponent<BuildingController>();
            building.InteractWith();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Zoom muss durch resizen aller Gameobjekte realisiert werden
        //transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime); 
        transform.Translate(Input.GetAxis("Horizontal") * CamSpeed * Time.deltaTime, Input.GetAxis("Vertical") * CamSpeed * Time.deltaTime, 0);

        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }
}
