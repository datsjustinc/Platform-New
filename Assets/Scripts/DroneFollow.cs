using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFollow : MonoBehaviour
{
    public float minDistance;
    public float speed;
    public int offset;
    public Transform player;
     
     
    void FixedUpdate () 
    {
        transform.LookAt(player);
        
        if (Vector3.Distance(transform.position, player.position) >= minDistance)
        {
            Vector3 follow = player.position;
            
            //follow.y = this.transform.position.y;
            follow.y = player.position.y + offset;
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, follow, speed * Time.deltaTime);
        }
    }
}