using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator animator;
    
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
