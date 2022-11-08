using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CycleImages : MonoBehaviour {

    [SerializeField]
    private Sprite[] images;
    [SerializeField]
    private float swapTime = 1f;
    private Image imgComp;
    Queue<Sprite> imagesQueue;


    // Use this for initialization
    void Start () {
        if (images.Length == 0)
            this.enabled = false;
        imagesQueue = new Queue<Sprite>();
        foreach(Sprite i in images)
        {
            imagesQueue.Enqueue(i);
        }
        imgComp = GetComponent<Image>();
        StartCoroutine("NextPic");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator NextPic()
    {
        imgComp.sprite = imagesQueue.Dequeue();
        imagesQueue.Enqueue(imgComp.sprite);
        yield return new WaitForSeconds(swapTime);
        StartCoroutine("NextPic");
    }
}
