using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSprite(Sprite sprite)
    {
        this.render.sprite = sprite;
    }

    public void InteractWith()
    {
        Debug.Log("I'm opening my Menues!!");
    }
}
