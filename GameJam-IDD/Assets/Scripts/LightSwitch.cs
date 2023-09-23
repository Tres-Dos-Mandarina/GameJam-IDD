using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameEvent onLightTurnOff;
    public GameEvent onPopUpEnter;
    public GameEvent onPopUpExit;
    public AudioSource switchSound;
    bool isEventCalled = false;
    
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isEventCalled)
            {
                isEventCalled = true;
                onPopUpEnter.Raise(this, 0);
            }

            if (Input.GetButtonDown("Interaction"))
            {
                switchSound.Play();
                onLightTurnOff.Raise(this, EnemyState.Moving);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEventCalled = false;
            onPopUpExit.Raise(this, 0);
        }
    }
}
