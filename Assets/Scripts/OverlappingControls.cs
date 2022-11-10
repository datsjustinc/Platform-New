using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlappingControls : MonoBehaviour
{
    public TMP_Text[] collisions = new TMP_Text[3];
    public TMP_Text error;

    private void Start()
    {
        Ping();
    }

    public void Ping()
    {
        bool x = CheckForErrors();
        Debug.Log(x);
        error.gameObject.SetActive(x);
    }

    bool CheckForErrors()
    {
        for (int i = 0; i<collisions.Length; i++)
        {
            string nextText = collisions[i].text;

            if (i < 2 && nextText == collisions[i + 1].text)
                return true;
            if (i < 1 && nextText == collisions[i + 2].text)
                return true;

            switch (nextText)
            {
                case ("Left Arrow"):
                    return true;
                case ("Right Arrow"):
                    return true;
                case ("A"):
                    return true;
                case ("D"):
                    return true;
                case ("Left Click"):
                    return true;
            }
        }

        return false;
    }
}
