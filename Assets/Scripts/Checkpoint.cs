using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool active = false;
    Transform x;
    int y;

    private void Awake()
    {
        x = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            y+=5;
        }
        else
        {
            y = 0;
        }
        x.localEulerAngles = new Vector3(0, y, 0);
    }
}
