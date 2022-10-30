using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    Rigidbody2D rb;
    public float xspeed;
    public float yspeed;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(10*xspeed * Time.deltaTime, 10*yspeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Slidable"))
        {
            Debug.Log("hit something");
            xspeed = -xspeed;
            yspeed = -yspeed;
        }
    }
}
