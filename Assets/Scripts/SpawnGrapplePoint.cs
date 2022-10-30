using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnGrapplePoint : MonoBehaviour
{
    public Camera main;
    public GameObject grapplePoint;
    public Vector3 point;
    public int grappleCount;
    public int grappleMax;
    public PlayerController player;

    void Start()
    {
        //grappleList = new List<GameObject>();
        SetGrappleCondition();

        main = this.transform.GetChild(6).GetComponent<Camera>();
    }

    public void SetGrappleCondition()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        grappleCount = 0;
        
        if (sceneName.Equals("Tutorial"))
        {
            grappleMax = 5;
        }

        if (sceneName.Equals("Easy"))
        {
            grappleMax = 5;
        }

        if (sceneName.Equals("Hard"))
        {
            grappleMax = 5;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && grappleCount < grappleMax)
        {
            point = main.ScreenToWorldPoint(Input.mousePosition);
            point.z = transform.position.z;
            Instantiate(grapplePoint, point, Quaternion.identity);
            //grappleList.Add(Instantiate(grapplePoint, point, Quaternion.identity));
            Instantiate(grapplePoint, point, Quaternion.identity);
            grappleCount++;
        }

        if (player.teleport)
        {
            /*
            foreach (GameObject grapple in grappleList.ToArray())
            {
                Debug.Log("removing");
                grappleList.Remove(grapple);
                Destroy(grapple)
            }

            SetGrappleCondition();
            */
            GameObject[] grapples = GameObject.FindGameObjectsWithTag("Grapple");   
            
            foreach (GameObject grapple in grapples) 
            {
                Destroy(grapple);
            }
            
            SetGrappleCondition();
        }
    }
}
