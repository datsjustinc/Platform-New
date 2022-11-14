using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearData : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ResetData);
    }

    public void ResetData()
    {
        GameObject.Find("Data Tracker").GetComponent<DataTracker>().ResetData();
    }
}
