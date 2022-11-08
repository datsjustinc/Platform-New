using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraAroundObject : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float speed = 10f;

    private Vector3 euler;

    public Transform Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
            transform.position = Target.position + offset;
        }
    }

    // Use this for initialization
    void Start () {
        euler = transform.eulerAngles;
        transform.position = Target.position + offset;
        transform.LookAt(Target);	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        transform.LookAt(Target);
        transform.eulerAngles = new Vector3(euler.x,transform.eulerAngles.y,transform.eulerAngles.z);
    }

    public void setXOffset(float offsetX)
    {
        offset = new Vector3(offsetX, offset.y, offset.z);
        transform.position = Target.position + offset;
    }
}
