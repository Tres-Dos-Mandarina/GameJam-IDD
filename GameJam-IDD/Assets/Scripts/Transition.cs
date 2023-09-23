using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator animator;
    
    public void FadeOut()
    {
        Debug.Log("FadeOut");
        //animator.SetBool("FadeIn", false);
        animator.Play("FastFadeOut");
    }

    public void FadeIn()
    {
        Debug.Log("FadeIn");
         animator.SetBool("FadeIn", true);
    }
}
