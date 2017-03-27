using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AggroPanel : MonoBehaviour {


    public Color normalButtonColor;
    public Color selectedButtonColor;

    private ColorBlock theColor = ColorBlock.defaultColorBlock;
    private int aggro;
    public Button[] buttons;

	// Use this for initialization
	void Start () {
        // buttons = GetComponentsInChildren<Button>();
        SetDefaultColors();
        updateAggro();
    }
	
	// Update is called once per frame
	void Update () {
        updateAggro();
        ColorButtonsAccordingToAggro();
	}

    private void SetDefaultColors()
    {
        theColor.normalColor = normalButtonColor;
        foreach (Button b in buttons)
        {
            b.colors = theColor;
        }
    }

    private void ColorButtonsAccordingToAggro()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(i == aggro)
            {
                theColor.normalColor = selectedButtonColor;
                theColor.highlightedColor = selectedButtonColor;
                buttons[i].colors = theColor;
            }else
            {
                theColor.normalColor = normalButtonColor;
                buttons[i].colors = theColor;
            }
        }
    }

    private void updateAggro()
    {
        aggro = BuildingManager.instance.GetCurrentPanelInfoTower().GetComponent<TowerAI>().getAggro();
    }
}
