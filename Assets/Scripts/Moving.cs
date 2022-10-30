using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 originalPosition;
    public float xspeed;
    public float yspeed;
    public bool bounce;

    private void Awake()
    {
        originalPosition = transform.parent.localPosition;
        rb = transform.parent.gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ResetPosition()
    {
        transform.parent.localPosition = originalPosition;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(10*xspeed * Time.deltaTime, 10*yspeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bounce && (collision.CompareTag("Ground") || collision.CompareTag("Slidable")))
        {
            xspeed = -xspeed;
            yspeed = -yspeed;
        }
    }
}
