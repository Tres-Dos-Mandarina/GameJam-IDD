using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameEvent onLightTurnOff;
    public GameEvent onPopUpEnter;
    public GameEvent onPopUpExit;
    public AudioSource switchSound;
    bool isEventCalled = false;
    bool canInteract = false;
    private void Update()
    {
        if(canInteract)
        {
            if (Input.GetButtonDown("Interaction"))
            {
                switchSound.Play();
                onLightTurnOff.Raise(this, EnemyState.Moving);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
