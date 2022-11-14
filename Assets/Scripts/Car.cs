using UnityEngine;
using System.Collections;
public class Car : MonoBehaviour 
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();

    private float tempDir;
    public Collider2D hit;
    public bool pause;
    
    void Start () 
    {
        posOffset = transform.position;
        pause = false;
        hit.enabled = false;
    }

    IEnumerator move()
    {
        yield return new WaitForSeconds(0.5f);
        pause = false;
    }

    void Update () 
    {
        if (!pause)
        {
            hit.enabled = false;
            tempPos = posOffset;
            tempDir = Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            tempPos.z += tempDir;
            transform.position = tempPos;
        }

        if (tempDir <= amplitude + 0.1f)
        {
            pause = true;
            StartCoroutine(move());
        }
        
        if (tempDir <= amplitude + 0.3f)
        {
            hit.enabled = true;
        }
    }
}