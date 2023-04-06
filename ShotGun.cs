using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Singleton<ShotGun>
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 100f;
    [SerializeField] private float timeToReload = 3;
    [SerializeField] private GameObject[] _bullets;
    [SerializeField] private Transform spawnBullet;

    //Coroutine liveBullet;
    private Coroutine _reloadCoroutine;
    public float timeDestroy = 3f;

    private List<GameObject> _inactiveBullets = new List<GameObject>();

    private bool _reload = false;
    public float spread;
    public float shootForce;

    public Camera fpsCam;

    private void Awake()
    {
        _inactiveBullets.Add(_bullets[0]);
        _inactiveBullets.Add(_bullets[1]);
    }

    public void PressShootShotGun()
    {
        _reloadCoroutine = StartCoroutine(ShootShotGun());
    }

    public IEnumerator ShootShotGun()
    {
        Debug.Log("Shoot");
        if (!_reload)
        {
            Shoot();
            Reload();
        }
            _reload = true;
            yield return new WaitForSeconds(3);
            _reload = false;
    }

    public void Reload()
    {
        PlayerController.Instance.anim.SetTrigger("isReload");
        Debug.Log("Reload");
    }

    public void Shoot()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(0);

        PlayerController.Instance.anim.SetTrigger("isShoot");
        Vector3 dirWithoutSpread = targetPoint - spawnBullet.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 dirWithSpread = dirWithoutSpread + new Vector3(x, y, 0);
      
        GameObject currentBullet = Instantiate(_inactiveBullets[0], spawnBullet.position, Quaternion.identity);

        currentBullet.SetActive(true);

        currentBullet.transform.forward = dirWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(dirWithSpread.normalized * shootForce, ForceMode.Impulse); 
    }
}