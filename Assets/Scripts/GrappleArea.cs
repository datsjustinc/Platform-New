using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleArea : MonoBehaviour
{
    [SerializeField] private List<GameObject> possiblePoints;
    [SerializeField] private List<GameObject> usedPoints;
    private GameObject targetPoint;
    public GameObject closestGrapplePoint;
    [SerializeField] private GameObject pl;

    void Start()
    {
        targetPoint = GameObject.Find("GrappleTarget");
        pl = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (closestGrapplePoint != null)
        {
            targetPoint.SetActive(true);
            if (targetPoint.transform.position != closestGrapplePoint.transform.position)
            {
                targetPoint.transform.position = closestGrapplePoint.transform.position;
            }
        }
        else
        {
            targetPoint.SetActive(false);
        }
    }

    private void closestPoint()
    {
        if (possiblePoints.Count > 0)
        {
            closestGrapplePoint = possiblePoints[0];
            foreach (var point in possiblePoints)
            {
                if (point.activeSelf)
                {
                    if (Vector3.Distance(pl.transform.position, point.transform.position) <= Vector3.Distance(pl.transform.position, closestGrapplePoint.transform.position))
                    {
                        closestGrapplePoint = point;
                    }
                }
            }
        }
        else
        {
            closestGrapplePoint = null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9) //layer 9: GrapplePoint
        {
            possiblePoints.Add(collision.gameObject);
            closestPoint();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            possiblePoints.Remove(collision.gameObject);
            closestPoint();
        }
    }
}
