using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameEvent onLightTurnOff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetButtonDown("Interaction"))
            {
                Debug.Log("Interaction______________________________________________________________________________________________________________");
                onLightTurnOff.Raise(this, EnemyState.Moving);
            }
        }
    }
}
