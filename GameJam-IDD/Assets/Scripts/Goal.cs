using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameEvent onGoalReached;
    public GameEvent popUpEnter;
    public GameEvent popUpExit;
    bool isLightOff = false;
    bool isEventCalled = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;
        if(isLightOff)
        {
            Debug.Log("Sending Goal Event");
            onGoalReached.Raise(this, null); //GM
        }
        else if (!isEventCalled)
        {
            isEventCalled = true;
            popUpEnter.Raise(this, 1);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isEventCalled = false;
        popUpExit.Raise(this, 1);
    }
    public void IsLightOff(Component sender, object data)
    {
        isLightOff = true;
    }

}
