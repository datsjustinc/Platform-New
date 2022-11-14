using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSensor : MonoBehaviour
{
    PlayerController pc;
    GameObject currentcheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        pc = this.GetComponentInParent<PlayerController>();
    }

    IEnumerator DestroyEffect(GameObject x)
    {
        yield return new WaitForSeconds(1f);
        Destroy(x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            pc.RemoveCollectible(true);
            pc.currenthealth = 1.0f;
            pc.sgp.ReloadGrapples();

            if (collision.gameObject.CompareTag("Checkpoint") && collision.gameObject != currentcheckpoint)
            {
                currentcheckpoint = collision.gameObject;
                pc.audio.PlayOneShot(pc.checkpoint, 0.75f);
                Instantiate(pc.checkpointEffect, pc.transform.position, Quaternion.identity);
                pc.spawn.transform.position = collision.gameObject.transform.position;
            }

        }
        if (collision.gameObject.CompareTag("Unlock"))
        {
            pc.GotCollectible(collision.gameObject);
            pc.audio.PlayOneShot(pc.collectible, 0.5f);
            GameObject x = Instantiate(pc.collectedEffect, pc.transform.position, Quaternion.identity);
            StartCoroutine(DestroyEffect(x));
        }
        if (collision.gameObject.CompareTag("End"))
        {
            pc.LevelEnded();
        }

    }
}