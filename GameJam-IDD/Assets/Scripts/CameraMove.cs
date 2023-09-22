using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Rigidbody2D playerRB = null;

    /*
    public Player player = null;
    public Vector3 offset = new Vector3(0, 0, -5);
    [Range(0f, 1f)]
    public float smooth = 0.01f;
    public float smoothLock = 4;
    */
    private Vector3 velocity;
    //---
    private float newPos = 0;
    public float cameraSpeed = 0;
    public float distance = 0;

    private int direction = 0;
    private float lastSpeed = 0;
    
    [Header("Y Camera")]
    public bool followYPlayer= false;
    [Range(-3f, 3f)]
    public float verticalPos = 0;

    private void LateUpdate()
    {
        Move5();
    }

    /*public void Move()
    {
        Vector3 target = player.gameObject.transform.position + offset;
        Vector3 movement = Vector3.Lerp(transform.position, target, smooth);

        transform.position = movement;
    }
    public void Move2()
    {
        velocity = playerRB.velocity;
        Vector3 target = playerRB.position;
        Vector3 movement = Vector3.zero;

        //movement = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z) + velocity, newSpeed);

        //if (velocity.x < player.moveSpeed - 1 || velocity.x > -player.moveSpeed + 1) 
        if (velocity.x < player.moveSpeed - smoothLock && velocity.x > -player.moveSpeed + smoothLock)
        {
            Debug.Log("AA");
            movement = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z) + velocity, smooth);
        }
        else
        {
            Debug.Log("BB");

            movement = new Vector3(target.x + (playerRB.velocity.x / 100f * 90f), target.y, transform.position.z);
        }
        transform.position = movement;
    }

    public void Move3()
    {
        velocity = playerRB.velocity;
        Vector3 target = playerRB.position;

        if (velocity.x < player.moveSpeed - smoothLock && velocity.x > -player.moveSpeed + smoothLock)
            transform.position = new Vector3(target.x + (playerRB.velocity.x / 100f * 90f), target.y, transform.position.z);
        else if (playerRB.velocity.x > smoothLock)
        {
            transform.position = new Vector3(target.x + (smoothLock / 100f * 90f), target.y, transform.position.z);
        }
        else if (playerRB.velocity.x < -smoothLock)
        {
            transform.position = new Vector3(target.x + (-smoothLock / 100f * 90f), target.y, transform.position.z);
        }

    }
    public void Move4()
    {
        velocity = playerRB.velocity;
        Vector3 target = playerRB.position;


        transform.position = new Vector3(playerRB.position.x + lookAhead, transform.position.y, transform.position.z);



        if (velocity.x != 0)
        {
            lookAhead = Mathf.Lerp(lookAhead, aheadDistance * (velocity.x / Mathf.Abs(velocity.x)), Time.deltaTime * cameraSpeed);
        }
        else
        {
            lookAhead = Mathf.Lerp(lookAhead, 0, Time.deltaTime * cameraSpeed);
        }
    }*/

    public void Move5()
    {
        velocity = playerRB.velocity;

        direction = (velocity.x > 0) ? 1 : -1;
        if (Mathf.Abs(velocity.x) < lastSpeed) direction = 0;
        if (velocity.x == 0) direction = 0;

        //Sube y baja con el player
        if (followYPlayer) transform.position = new Vector3(playerRB.position.x + newPos, playerRB.position.y + verticalPos, transform.position.z);
        else transform.position = new Vector3(playerRB.position.x + newPos, transform.position.y, transform.position.z);

        newPos = Mathf.Lerp(newPos, distance * direction, Time.deltaTime * cameraSpeed);

        lastSpeed = Mathf.Abs(velocity.x);
    }
}
    



