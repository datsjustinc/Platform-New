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

    void Start()
    {
        player = this.GetComponent<PlayerController>();
        main = this.transform.GetChild(6).GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && grappleList.Count < grappleMax)
        {
            point = main.ScreenToWorldPoint(Input.mousePosition);
            point.z = transform.position.z;
            GameObject x = Instantiate(grapplePoint, point, Quaternion.identity);
            x.GetComponent<SpriteRenderer>().color = Color.yellow;
            grappleList.Add(x);
        }

        if (player.teleport)
        {
            while (grappleList.Count > 0)
            {
                Destroy(grappleList[0].gameObject);
                grappleList.RemoveAt(0);
            }
        }
    }
}
