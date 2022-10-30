using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 originalPosition;
    BoxCollider2D bc;
    bool waitingfordelay = true;

    public float xspeed;
    public float yspeed;
    public bool bounce;

    private void Awake()
    {
        originalPosition = transform.parent.localPosition;
        rb = transform.parent.gameObject.AddComponent<Rigidbody2D>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();
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
        if (waitingfordelay && bounce && (collision.CompareTag("Ground") || collision.CompareTag("Slidable")))
        {
            if (xspeed > 0 || yspeed > 0)
            {
                StartCoroutine(Delayed());
                xspeed *= -1;
                yspeed *= -1;
            }
            else
            {
                StartCoroutine(Delayed());
                xspeed *= -1;
                yspeed *= -1;
            }
        }
    }

    IEnumerator Delayed()
    {
        waitingfordelay = false;
        yield return new WaitForSeconds(1f);
        waitingfordelay = true;
    }
}
