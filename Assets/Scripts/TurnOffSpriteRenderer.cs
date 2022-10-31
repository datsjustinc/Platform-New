using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSpriteRenderer : MonoBehaviour
{
    SpriteRenderer x;
    MeshRenderer y;

    private void Awake()
    {
        x = this.GetComponent<SpriteRenderer>();
        if (x != null)
            x.enabled = false;

        y = this.GetComponent<MeshRenderer>();
        if (y != null)
            y.enabled = false;
    }
}
