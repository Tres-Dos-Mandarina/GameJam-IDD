using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameEvent onGoalReached;
    public GameEvent popUpEnter;
    public GameEvent popUpExit;
    bool isLightOff = false;
    bool isEventCalled = false;
    public float distanceToPlayer = 3f;
    private bool sendMessage = false;
    private Transform player;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        if(Vector3.Distance(player.position, transform.position) < distanceToPlayer)
        {
            if(!isEventCalled && !sendMessage && !isLightOff)
            {
                    isEventCalled = true;

                    popUpEnter.Raise(this, 1);
                    sendMessage = true;
                
            }
        }
        else
        {
            isEventCalled = false;
            sendMessage = false;
            popUpExit.Raise(this, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;
        if(isLightOff)
        {
            onGoalReached.Raise(this, null); //GM
        }
        //else if (!isEventCalled)
        //{
        //    isEventCalled = true;
        //    popUpEnter.Raise(this, 1);
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        //isEventCalled = false;
        //popUpExit.Raise(this, 1);
    }
    public void IsLightOff(Component sender, object data)
    {
        isLightOff = true;
    }

}
