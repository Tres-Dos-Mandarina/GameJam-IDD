using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorText : MonoBehaviour
{
    
    private TMP_Text theText;

    public Color32[] colors = { new Color32(127, 127, 127,255), new Color32(209, 209, 209,255) };

    private void Awake()
    {
        theText = this.transform.GetChild(0).GetComponent<TMP_Text>();
        theText.color = colors[0];
    }

    public void OnPointerEnter()
    {
        theText.color = colors[1]; //Or however you do your color
    }

    public void OnPointerExit()
    {
        theText.color = colors[0]; //Or however you do your color
    }
}
