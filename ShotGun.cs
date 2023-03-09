using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Singleton<ShotGun>
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _timeToReload = 3;
    [SerializeField] private GameObject[] _bullets;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private GameObject mouseObject;
    Coroutine liveBullet;
    Coroutine _reloadCoroutine;
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
        //bullet += 2;
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
        Vector3 dirWithoutSpread = targetPoint - _spawnBullet.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 dirWithSpread = dirWithoutSpread + new Vector3(x, y, 0);
        //float quater = Quaternion.Euler(0, 0, 0);
        GameObject currentBullet = Instantiate(_inactiveBullets[0], _spawnBullet.position, Quaternion.identity);

        currentBullet.SetActive(true);

        currentBullet.transform.forward = dirWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(dirWithSpread.normalized * shootForce, ForceMode.Impulse);

        //currentBullet.GetComponent<Rigidbody>().velocity=dirWithSpread.normalized*shootForce;

        //    Destroy(currentBullet);
        //if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, _range))
        //{
        //    Debug.Log(hit.transform.name);

        //    HealthMouse target = hit.transform.GetComponent<HealthMouse>();


        //    if (target != null)
        //    {

        //        Animator anim = target.GetComponent<Animator>();
        //        anim.SetTrigger("isHitMouse");
        //        target.ChangeHealthMouse(-_damage);
        //    }
        //    //yield return new WaitForSeconds(3);
        //}
    }
}
//Vector3 rotateGun = transform.eulerAngles;
//rotateGun.y = 165.9f;
//rotateGun.x = -181.5f;
//rotateGun.z = -12.4f;
//Weapons[0].transform.position = new Vector3(0.12f, 0.25f, 0.25f);
//Weapons[0].transform.rotation=Quaternion.Euler(rotateGun);