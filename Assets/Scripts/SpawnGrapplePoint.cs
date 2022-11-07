using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnGrapplePoint : MonoBehaviour
{
    Camera main;
    public GameObject grapplePoint;
    Vector3 point;
    public int grappleMax;
    PlayerController player;
    public List<GameObject> grappleList = new List<GameObject>();
    public bool grapplePlaced;
    
    void Start()
    {
        player = this.GetComponent<PlayerController>();
        main = this.transform.GetChild(6).GetComponent<Camera>();
        grapplePlaced = false;
    }

    public void ReloadGrapples()
    {
        while (grappleList.Count > 0)
        {
            Destroy(grappleList[0].gameObject);
            grappleList.RemoveAt(0);
        }
    }
    
    public void GrappleCheck()
    {
        
    }

    void Update()
    {
        Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (Input.GetKeyDown(KeyCode.Mouse0) && grappleList.Count < grappleMax)
        {
            if (!hit.collider.CompareTag("Grapple") && !grapplePlaced)
            {
                GameObject x = Instantiate(grapplePoint, mousePos2D, Quaternion.identity);
                x.GetComponent<SpriteRenderer>().color = Color.yellow;
                grappleList.Add(x);
            }
        }
        
        if (hit.collider.CompareTag("Grapple"))
        {
            grapplePlaced = true;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                for (int x = 0; x < grappleList.Count; x++)
                {
                    if (grappleList[x].Equals(hit.collider.gameObject))
                    {
                        grappleList.RemoveAt(x);
                        Destroy(hit.collider.gameObject);
                    }
                }
            }

        }

        else
        {
            grapplePlaced = false;
        }
    }
}
