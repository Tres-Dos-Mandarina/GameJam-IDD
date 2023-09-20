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
    public void FadeOut(Component sender, object data)
    {
        Debug.Log("FadeOut");
        animator.SetBool("FadeIn", false);
    }
    public void GoalHandler(Component sender, object data)
    {
        if (sender is Goal)
        {
            if (data is string)
            {
                Debug.Log(data);
            }
        }
    }
    public void FadeIn(Component sender, object data)
    {
        Debug.Log("FadeIn");
         animator.SetBool("FadeIn", true);
    }
}
