using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace de.enjoyLife.Confetti
{
    public class DemoSceneManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject Canon;
        [SerializeField]
        private GameObject Gun;
        [SerializeField]
        private GameObject[] objectsToCycleAround;
        [SerializeField]
        private GameObject Camera;
        private int currentIndex = 0;
        [SerializeField]
        private Image leftButtonImage;


        private void Start()
        {
            foreach(GameObject o in objectsToCycleAround)
            {
                o.SetActive(false);
            }
            showObjectWithIndex(currentIndex);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Gun.GetComponent<FireCanon>() == null)
                {
                    Debug.Log("Pleasy apply the FireCanon script to the Canon GameObject.");
                    return;
                }
                Gun.GetComponent<FireCanon>().StartFire();
                if (Canon.GetComponent<FireCanon>() == null)
                {
                    Debug.Log("Pleasy apply the FireCanon script to the Canon GameObject.");
                    return;
                }
                Canon.GetComponent<FireCanon>().StartFire();

            }
            else
            {
                Gun.GetComponent<FireCanon>().StopFire();
                Canon.GetComponent<FireCanon>().StopFire();
            }
        }

        public void nextObject()
        {
            objectsToCycleAround[currentIndex].SetActive(false);
            if (currentIndex >= objectsToCycleAround.Length-1)
                currentIndex = 0;
            else
                currentIndex++;
            showObjectWithIndex(currentIndex);
        }

        private void showObjectWithIndex(int index)
        {
            objectsToCycleAround[currentIndex].SetActive(true);
            try
            {
                Camera.GetComponent<RotateCameraAroundObject>().Target = objectsToCycleAround[currentIndex].transform;
            }
            catch(NullReferenceException)
            {
                Debug.LogError("Camera needs a Rotate Camerea around Script!");
            }
            if(index >= 2)
            {
                leftButtonImage.enabled = false;
            }
            else
            {
                leftButtonImage.enabled = true;
            }
        }

    }
}