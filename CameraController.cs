using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform cam;
    
    private void Update()
    {
        cam.transform.position = new Vector3(transform.position.x, player.transform.position.y+1.5f,
            transform.position.z);
    }
}
