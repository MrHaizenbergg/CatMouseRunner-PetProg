using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform cam;
    
    private void Update()
    {
        cam.transform.position = new Vector3(transform.position.x, player.transform.position.y + 3.5f,
            player.transform.position.z + (-3.9f));
        Vector3 rotateCam = transform.eulerAngles;
        rotateCam.y = 0f;
        rotateCam.x = 16f;
        rotateCam.z = 0f;
        cam.transform.rotation = Quaternion.Euler(rotateCam);

    }
}
