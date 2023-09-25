using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnlyForCredits : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = FindObjectOfType<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim.Play("Credits");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("_MainMenu");
    }
}
