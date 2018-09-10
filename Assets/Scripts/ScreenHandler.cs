using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHandler : MonoBehaviour
{

    public float scrW, scrH;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        scrW = Screen.width / 16;
        scrH = Screen.height / 9;
        //GUI.Box(new Rect(7 * scrW, 2 * scrH, scrW, scrH), "");
    }
}
