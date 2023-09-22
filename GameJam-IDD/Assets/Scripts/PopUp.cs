using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUp : MonoBehaviour
{
    public int id;
    public Animator anim;
    public TMP_Text text;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartDialoge(Component sender, object data)
    {
        if(data is int && (int)data == id)
        {
            anim.SetBool("in", true);
        }
    }
    public void HideDialoge(Component sender, object data)
    {
        if (data is int && (int)data == id)
        {
            anim.SetBool("in", false);
        }
    }
}
