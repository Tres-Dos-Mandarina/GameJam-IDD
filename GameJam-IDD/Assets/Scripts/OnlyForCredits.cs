using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnlyForCredits : MonoBehaviour
{
    private Animator anim;
    private bool canType = false;
    private void Awake()
    {
        anim = FindObjectOfType<Animator>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim.Play("Credits");
        StartCoroutine(WaitToType());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && canType)
            GoToMainMenu();
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("_MainMenu");
    }
    IEnumerator WaitToType()
    {
        yield return new WaitForSeconds(4f);
        canType = true;
    }

}
