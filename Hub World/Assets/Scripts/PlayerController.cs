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

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime); Zoom muss durch resizen aller Gameobjekte realisiert werden
        transform.Translate(Input.GetAxis("Horizontal") * CamSpeed * Time.deltaTime, Input.GetAxis("Vertical") * CamSpeed * Time.deltaTime, 0);
    }
}
