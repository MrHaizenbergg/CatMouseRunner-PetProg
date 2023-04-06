using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float _rotationSpeed = 100;

    void Start()
    {
        _rotationSpeed += Random.Range(0, _rotationSpeed / 4.0f);
    }

    void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.gameObject.SetActive(false);
        }
        
    }
}
