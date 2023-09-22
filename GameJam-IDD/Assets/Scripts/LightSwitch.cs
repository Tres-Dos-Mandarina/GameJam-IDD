using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameEvent onLightTurnOff;
    public AudioSource switchSound;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetButtonDown("Interaction"))
            {
                Debug.Log("Interaction______________________________________________________________________________________________________________");
                switchSound.Play();
                onLightTurnOff.Raise(this, EnemyState.Moving);
            }
        }
    }
}
