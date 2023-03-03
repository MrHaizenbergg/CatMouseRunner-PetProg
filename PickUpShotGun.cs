using UnityEngine;

public class PickUpShotGun : MonoBehaviour
{
    float rotationSpeed = 100;

    void Start()
    {
        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.gameObject.SetActive(false);
            PlayerController.Instance.PickUpShotGun();
        }

    }
}
