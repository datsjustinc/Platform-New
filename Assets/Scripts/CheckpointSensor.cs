using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSensor : MonoBehaviour
{
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = this.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            pc.RemoveCollectible(true);
            pc.currenthealth = 1.0f;
            pc.sgp.ReloadGrapples();
            pc.spawn.transform.position = collision.gameObject.transform.position;
        }
        if (collision.gameObject.CompareTag("Unlock"))
        {
            pc.GotCollectible(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("End"))
        {
            pc.LevelEnded();
        }

    }
}