using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Speed")]
    public float followSpeed;

    [Header("Offsets")] 
    public float xOffset;
    public float yOffset;
    
    [Header("Player Transform")]
    public Transform target;

    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x + xOffset, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
    }
}
