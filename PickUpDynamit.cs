using UnityEngine;

public class PickUpDynamit : MonoBehaviour
{
    private float _rotationSpeed = 100;
    
    void Start()
    {
        _rotationSpeed += Random.Range(0, _rotationSpeed / 4.0f);
    }

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.gameObject.SetActive(false);
            PlayerController.Instance.PickUpDynamit();
            PlayerController.Instance.PressWeaponSwitcher(1);
           
        }

    }
}
