using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        //FadeIn();
    }
    public void FadeOut()
    {
        Debug.Log("FadeOut");
        animator.SetBool("FadeIn", false);
    }

    public void FadeIn()
    {
        Debug.Log("FadeIn");
         animator.SetBool("FadeIn", true);
    }
}
