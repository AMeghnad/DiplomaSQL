using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    #region Variables
    [Header("Main UI")]
    public bool showSelectMenu;
    public bool toggleTogglable;
    public float scrW, scrH;

    [Header("Resources")]
    public Texture2D radialTexture;
    public Texture2D slotTexture;
    [Range(0, 100)]
    public int circleScaleOffset;

    [Header("Icons")]
    public Vector2 iconSize;
    public bool showIcons, showBoxes, showBounds;
    public float iconSizeNum;
    [Range(-360, 360)]
    public int radialRotation;
    [SerializeField]
    private float iconOffset;

    [Header("Mouse Settings")]
    public Vector2 mouse;
    public Vector2 input;
    public Vector2 circleCentre;

    [Header("Input Settings")]
    public float inputDist;
    public float inputAngle;
    public int keyIndex;
    public int mouseIndex;
    public int inputIndex;

    [Header("SectorSettings")]
    public Vector2[] slotPos;
    public Vector2[] boundsPos;
    [Range(1, 8)]
    public int numOfSectors;
    [Range(50, 300)]
    public float circleRadius = 50;
    public float mouseDist, sectorDeg, mouseAngle;
    public int sectorIndex = 0;
    public bool withinCircle;

    [Header("Misc")]
    private Rect debugWindow;

    #endregion

    // Use this for initialization
    void Start()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;

        circleCentre.x = 8 * scrW;
        circleCentre.y = 4.5f * scrH;

        debugWindow = new Rect(Scr(0, 0), Scr(4, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scrH = Screen.height / 9;
            scrW = Screen.width / 16;
            showSelectMenu = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            showSelectMenu = false;
        }
    }

    void OnGUI()
    {
        debugWindow = GUI.Window(0, debugWindow, JoyStickUI, "");
        if (showSelectMenu)
        {
            CalculateMouseAngles();

            sectorDeg = 360 / numOfSectors;
            iconOffset = sectorDeg / 2;
            slotPos = SlotPositions(numOfSectors);
            boundsPos = BoundPositions(numOfSectors);
            // Centre
            //GUI.Box(new Rect(Scr(7.5f, 4), Scr(1, 1)), "");
            // Circle
            GUI.DrawTexture(new Rect(circleCentre.x - circleRadius - circleScaleOffset / 4, circleCentre.y - circleRadius - circleScaleOffset / 4, circleRadius * 2 + circleScaleOffset / 2, circleRadius * 2 + circleScaleOffset / 2), radialTexture);
            if (showBoxes)
            {
                for (int i = 0; i < numOfSectors; i++)
                {
                    GUI.DrawTexture(new Rect(slotPos[i].x - scrW * iconSizeNum * 0.5f, slotPos[i].y - scrH * iconSizeNum * 0.5f, scrW * iconSizeNum, scrH * iconSizeNum), slotTexture);
                }
            }
            if (showBounds)
            {
                for (int i = 0; i < numOfSectors; i++)
                {
                    GUI.Box(new Rect(boundsPos[i].x - scrW * 0.05f, boundsPos[i].y - scrH * 0.05f, scrW * 0.1f, scrH * 0.1f), "");
                }
            }
            if (showIcons)
            {
                SetItemSlots(numOfSectors, slotPos);
            }
        }
    }

    void CalculateMouseAngles()
    {
        mouse = Input.mousePosition;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        mouseDist = Mathf.Sqrt(Mathf.Pow((mouse.x - circleCentre.x), 2) + Mathf.Pow((mouse.y - circleCentre.y), 2));

        inputDist = Vector2.Distance(Vector2.zero, input);

        withinCircle = mouseDist <= circleRadius ? true : false;

        if (input.x != 0 || input.y != 0)
        {
            inputAngle = (Mathf.Atan2(-input.y, input.x) * 180 / Mathf.PI);
        }
        else
        {
            mouseAngle = (Mathf.Atan2(mouse.y * circleCentre.y, mouse.x - circleCentre.x) * 180 / Mathf.PI) + radialRotation;
        }

        if (mouseAngle < 0)
        {
            mouseAngle += 360;
        }

        if (inputAngle < 0)
        {
            inputAngle += 360;
        }
        inputIndex = CheckCurrentSector(inputAngle);
        mouseIndex = CheckCurrentSector(mouseAngle);
        if (input.x != 0 || input.y != 0)
        {
            sectorIndex = inputIndex;
        }
        if (input.x == 0 && input.y == 0)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                sectorIndex = mouseIndex;
            }
        }
    }

    private int CheckCurrentSector(float angle)
    {
        float boundAngle = 0;
        for (int i = 0; i < numOfSectors; i++)
        {
            boundAngle += sectorDeg;
            if (angle < boundAngle)
            {
                return i;
            }
        }
        return 0;
    }

    void SetItemSlots(int slots, Vector2[] pos)
    {
        for (int i = 0; i < slots; i++)
        {
            GUI.DrawTexture(new Rect(Scr(pos[i].x - (scrW * iconSizeNum * 0.5f), pos[i].y - (scrH * iconSizeNum * 0.5f)), Scr(iconSizeNum, iconSizeNum)), slotTexture);
        }
    }

    private Vector2[] SlotPositions(int slots)
    {
        Vector2[] slotPos = new Vector2[slots];
        float angle = iconOffset * radialRotation;
        for (int i = 0; i < slotPos.Length; i++)
        {
            slotPos[i].x = circleCentre.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);

            slotPos[i].y = circleCentre.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            angle += sectorDeg;
        }

        return slotPos;
    }

    private Vector2[] BoundPositions(int slots)
    {
        Vector2[] boundPos = new Vector2[slots];
        float angle = radialRotation;
        for (int i = 0; i < boundPos.Length; i++)
        {
            boundPos[i].x = circleCentre.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            boundPos[i].y = circleCentre.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            angle += sectorDeg;
        }
        return boundPos;
    }

    void JoyStickUI(int windowID)
    {
        GUI.Box(new Rect(Scr(0, 0), Scr(1, 1)), "");
        GUI.Box(new Rect(Scr(.25f + (Input.GetAxis("Horizontal")) * .25f, .25f + (-Input.GetAxis("Vertical")) * .25f), Scr(.5f, .5f)), "");
        GUI.Box(new Rect(Scr(1.25f, .25f), Scr(.5f, .5f)), "Tab");

        if (showSelectMenu)
        {
            GUI.Box(new Rect(Scr(1.25f, .25f), Scr(.5f, .5f)), "");
        }
        GUI.DragWindow();
    }

    private Vector2 Scr(float x, float y)
    {
        Vector2 coord = Vector2.zero;
        coord = new Vector2(scrW * x, scrH * y);
        return coord;
    }
}
