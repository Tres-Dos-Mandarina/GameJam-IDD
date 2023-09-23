using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameEvent onLightTurnOff;
    public GameEvent onPopUpEnter;
    public GameEvent onPopUpExit;
    public AudioSource switchSound;
    private Animator anim;
    bool isEventCalled = false;
    bool canInteract = false;
    bool canPopap = true;

    private void Start()
    {
        canPopap = true;
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(canInteract && canPopap == true)
        {
            if (Input.GetButtonDown("Interaction"))
            {
                canPopap = false;
                anim.SetBool("IsPressed",true);
                switchSound.Play();
                onLightTurnOff.Raise(this, EnemyState.Moving);
                onPopUpExit.Raise(this, 0);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && canPopap == true)
        {
            if (!isEventCalled)
            {
                isEventCalled = true;
                onPopUpEnter.Raise(this, 0);
            }
            if(!canInteract)
                canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEventCalled = false;
            canInteract = false;
            onPopUpExit.Raise(this, 0);
        }
    }
}
