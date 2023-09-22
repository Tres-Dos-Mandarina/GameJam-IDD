using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ToggleType
{
    Speedrun,
    Sound,
}


public class UI_Toggle : MonoBehaviour
{
    [SerializeField] private ToggleType _toggleType;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private GameObject _light;
    [SerializeField] private GameObject _rays;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private GameObject _darken;
    [SerializeField]private Color _color;
    private SaveData _saveData;

    private Toggle _toggle;
    private Image _background;

    private void Awake()
    { 
        _toggle = GetComponent<Toggle>();
        _saveData = GameObject.Find("Data Saver").GetComponent<SaveData>();
    }

    public void OnSwitch(bool isOn)
    {
        this.GetComponentInChildren<Image>().sprite = isOn ? _onSprite : _offSprite;
        _color = _darken.GetComponent<Image>().color;

        switch (_toggleType)
        {
            case ToggleType.Speedrun:
                _saveData.config.speedrun = isOn;
                _light.SetActive(isOn);
                _rays.SetActive(isOn);
                _shadow.SetActive(isOn);
                if (isOn == true)
                    _color.a -= .2f;
                else
                    _color.a += .2f;
                break;
            case ToggleType.Sound:
                _saveData.config.audio = isOn;
                _light.SetActive(isOn);
                _rays.SetActive(isOn);
                _shadow.SetActive(isOn);
                if (isOn == true)
                    _color.a -= .2f;
                else
                    _color.a += .2f;

                break;
                default: break;
        }
        _darken.GetComponent<Image>().color = _color; 
    }
}
