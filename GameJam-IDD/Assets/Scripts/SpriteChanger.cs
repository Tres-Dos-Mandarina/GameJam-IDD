using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite ligh;
    public Sprite shadow;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void ChangeLightState(Component sender, object data)
    {
        sr.sprite = ligh;
    }
    public void ChangeShadowState(Component sender, object data)
    {
        sr.sprite = shadow;
    }

}
