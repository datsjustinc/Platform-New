using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlappingControls : MonoBehaviour
{
    public TMP_Text[] collisions = new TMP_Text[3];
    public TMP_Text error;
    string collision;

    private void Start()
    {
        Ping();
    }

    public void Ping()
    {
        CheckForErrors();
        if (collision == "")
            error.text = "";
        else
            error.text = "Warning: Your " + collision + " key is acting as multiple actions. Game may be buggy as a result.";
    }

    void CheckForErrors()
    {
        for (int i = 0; i<collisions.Length; i++)
        {
            string nextText = collisions[i].text;

            if (i < 2 && nextText == collisions[i + 1].text)
            {
                collision = nextText;
                return;
            }
            if (i < 1 && nextText == collisions[i + 2].text)
            {
                collision = nextText;
                return;
            }

            switch (nextText)
            {
                case ("Left Arrow"):
                    collision = nextText;
                    return;
                case ("Right Arrow"):
                    collision = nextText;
                    return;
                case ("A"):
                    collision = nextText;
                    return;
                case ("D"):
                    collision = nextText;
                    return;
                case ("Left Click"):
                    collision = nextText;
                    return;
            }
        }

        collision = "";
    }
}
