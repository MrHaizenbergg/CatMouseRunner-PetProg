using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    Vector3 startGamePosition;
    Vector3 targetVelocity;
    Quaternion startGameRotation;

    [SerializeField] private float laneChangeSpeed = 15;
    [SerializeField] private Text CounterText;
    [SerializeField] private Text RecordText;
    [SerializeField] private Button activeShotGunButton;
    [SerializeField] private Button activeThrowDynamit;
    [SerializeField] private Button activeGreatWeapon;
    [SerializeField] public GameObject[] Weapons;
    [SerializeField] GameObject[] weaponsForBack;
    [SerializeField] private GameObject[] weaponsPool;

    private Health _healthNotChange;
    private CapsuleCollider _col;
    public Animator anim;
    private Rigidbody rb;
    private StateMachineWeapon _smWeapon;

    Coroutine movingCoroutine;
    Coroutine shieldCoroutine;

    private float _pointStart;
    private float _laneOffset;
    private float _pointFinish;
    private float _lastVectorX;

    private float _jumpPower = 14;
    private float _jumpGravity = -40;
    private float _realGravity = -9.8f;

    private bool _isSpeedIncrease;
    private bool _isMoving = false;
    private bool _isImmortal;
    private bool _isGreatWeaponJump;

    private bool _isJumping = false;

    public static int counter { get; private set; }

    private int currentIndex;

    public event Action<int> ChangeIndexWeapon;

    private void Awake()
    {
        _healthNotChange = GetComponent<Health>();
        PressWeaponSwitcher(0);
    }

    private void FixedUpdate()
    {
        if (_isSpeedIncrease)
            StartCoroutine(SpeedIncrease());
        if (_isImmortal)
            StartCoroutine(ShieldBonus());
    }

    void Start()
    {
        _smWeapon = new StateMachineWeapon();
        _smWeapon.Initialize(new EmptyState());

        int lastRunScore = PlayerPrefs.GetInt("lastRunscore");
        int recordScore = PlayerPrefs.GetInt("recordScore");

        if (lastRunScore > recordScore)
        {
            recordScore = lastRunScore;
            PlayerPrefs.SetInt("recordScore", recordScore);
            RecordText.text = recordScore.ToString();
        }
        else
        {
            RecordText.text = recordScore.ToString();
        }

        _laneOffset = MapGenerator.Instance.laneOffset;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        SwipeManager.instance.MoveEvent += MovePlayer;
        counter = PlayerPrefs.GetInt("coins");
    }

    public void StartGame()
    {
        anim.enabled = true;
        MouseController.Instance.StartGame();
    }

    public void StartLevel()
    {
        ItemGeneratorFabric.Instance.StartThrowItem();
        RoadGenerator.Instance.StartLevel();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        _pointStart = 0;
        _pointFinish = 0;
        anim.SetTrigger("isIdle");
        anim.applyRootMotion = true;
        anim.enabled = false;
        counter = 0;
        _isImmortal = false;

        transform.position = startGamePosition;
        transform.rotation = startGameRotation;

        ItemGeneratorFabric.Instance.StopThrowItem();

        LoseShotGun();
        LoseDynamit();
        LoseGreatWeapon();

        Health.Instance.ChangeHealth(+100);
        HealthMouse.Instance.ChangeHealthMouse(+100);
        MouseController.Instance.ResetGame();
        RoadGenerator.Instance.ResetLevel();
    }

    void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeManager.Direction.Down] && _isJumping == false)
        {
            StartCoroutine(Slide());
        }
        if (swipes[(int)SwipeManager.Direction.Left] && _pointFinish > -_laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Right] && _pointFinish < _laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Up] && _isJumping == false)
        {
            Jump();
        }

    }

    public IEnumerator SpeedIncrease()
    {
        rb.velocity = new Vector3(0, 0, 2f);
        yield return new WaitForSeconds(2);
        _isSpeedIncrease = false;
    }

    public void PressWeaponSwitcher(int index)
    {
        currentIndex += index;

        if (currentIndex < 0) currentIndex = Weapons.Length - 1;
        else if (currentIndex > Weapons.Length - 1) currentIndex = 0;

        if (Weapons[0] != null && currentIndex == 0)
        {
            _smWeapon.ChangeState(new ShotGunState());
        }
        if (Weapons[1] != null && currentIndex == 1)
        {
            _smWeapon.ChangeState(new DynamiteState());
        }
        if (Weapons[2] != null && currentIndex == 2)
        {
            _smWeapon.ChangeState(new GreatWeaponState());
        }
    }

    public void PickUpShotGun()
    {
        activeThrowDynamit.gameObject.SetActive(false);
        anim.SetBool("isGreatWeaponRun", false);
        activeGreatWeapon.gameObject.SetActive(false);

        anim.SetBool("isRiffleRun", true);

        if (weaponsPool[1].activeInHierarchy)
        {
            weaponsForBack[1].SetActive(true);
            Weapons[1].SetActive(false);
        }
        if (weaponsPool[2].activeInHierarchy)
        {
            weaponsForBack[2].SetActive(true);
            Weapons[2].SetActive(false);
        }

        Weapons[0] = weaponsPool[0];
        Weapons[0].SetActive(true);
        if (weaponsPool[0].activeInHierarchy)
            weaponsForBack[0].SetActive(false);
        activeShotGunButton.gameObject.SetActive(true);
    }
    public void LoseShotGun()
    {
        anim.SetBool("isRiffleRun", false);
        if (Weapons[0] != null)
            Weapons[0].SetActive(false);

        Weapons[0] = null;
        activeShotGunButton.gameObject.SetActive(false);
    }

    public void PickUpDynamit()
    {
        anim.SetBool("isRiffleRun", false);
        activeShotGunButton.gameObject.SetActive(false);
        anim.SetBool("isGreatWeaponRun", false);
        activeGreatWeapon.gameObject.SetActive(false);

        if (weaponsPool[0].activeInHierarchy)
        {
            weaponsForBack[0].SetActive(true);
            Weapons[0].SetActive(false);
        }
        if (weaponsPool[2].activeInHierarchy)
        {
            weaponsForBack[2].SetActive(true);
            Weapons[2].SetActive(false);
        }

        Weapons[1] = weaponsPool[1];
        Weapons[1].SetActive(true);
        if (Weapons[1].activeInHierarchy)
            weaponsForBack[1].SetActive(false);
        activeThrowDynamit.gameObject.SetActive(true);
    }
    public void LoseDynamit()
    {
        if (Weapons[1] != null)
            Weapons[1].SetActive(false);

        Weapons[1] = null;
        activeThrowDynamit.gameObject.SetActive(false);
    }

    public void PickUpGreatWeapon()
    {
        anim.SetBool("isRiffleRun", false);
        activeShotGunButton.gameObject.SetActive(false);
        activeThrowDynamit.gameObject.SetActive(false);

        anim.SetBool("isGreatWeaponRun", true);
        _isGreatWeaponJump = true;


        if (weaponsPool[0].activeInHierarchy)
        {
            weaponsForBack[0].SetActive(true);
            Weapons[0].SetActive(false);
        }
        if (weaponsPool[1].activeInHierarchy)
        {
            Weapons[1].SetActive(false);
            weaponsForBack[1].SetActive(true);
        }

        Weapons[2] = weaponsPool[2];
        Weapons[2].SetActive(true);
        if (Weapons[2].activeInHierarchy)
            weaponsForBack[2].SetActive(false);
        activeGreatWeapon.gameObject.SetActive(true);
    }
    public void LoseGreatWeapon()
    {
        anim.SetBool("isGreatWeaponRun", false);
        _isGreatWeaponJump = false;
        if (Weapons[2] != null)
            Weapons[2].SetActive(false);

        Weapons[2] = null;
        weaponsForBack[2].SetActive(false);
        activeGreatWeapon.gameObject.SetActive(false);
    }

    public void VictoryCat()
    {
        anim.SetTrigger("isVictory");
    }

    void Jump()
    {
        anim.applyRootMotion = false;

        if (_isGreatWeaponJump == true)
            anim.SetTrigger("isGreatWeaponJump");
        else
            anim.SetBool("isJump", true);

        _isJumping = true;
        rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, _jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    public void Death()
    {
        RoadGenerator.Instance.StopGame();
        if (_isGreatWeaponJump == true)
            anim.SetTrigger("isGreatWeaponDeath");
        else
            anim.SetTrigger("isDying");

        anim.applyRootMotion = true;
        MouseController.Instance.ResetGame();
        ItemGeneratorFabric.Instance.StopThrowItem();
    }

    public void CatTurnLeft()
    {
        rb.transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    public void CatTurnRight()
    {
        rb.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void MoveHorizontal(float speed)
    {
        anim.applyRootMotion = false;
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;

        if (_isMoving)
        {
            StopCoroutine(movingCoroutine);
            _isMoving = false;
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }

    private IEnumerator MoveCoroutine(float VectorX)
    {
        _isMoving = true;
        while (Mathf.Abs(_pointStart - transform.position.x) < _laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(VectorX, rb.velocity.y, 0);
            _lastVectorX = VectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(_pointStart, _pointFinish), Mathf.Max(_pointStart, _pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(_pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {

            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        _isMoving = false;
    }

    private IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);

            anim.SetBool("isJump", false);

        }
        while (rb.velocity.y != 0);
        {
            _isJumping = false;

            Physics.gravity = new Vector3(0, _realGravity, 0);
        }
    }

    private IEnumerator Slide()
    {
        anim.applyRootMotion = false;
        anim.SetBool("isFalling", true);
        _col.center = new Vector3(0, 0.28f, 1.27f);
        _col.height = 0.58f;

        yield return new WaitForSeconds(0.8f);

        _col.center = new Vector3(0, 0.67f, 0f);
        _col.height = 1.313049f;
        anim.applyRootMotion = true;
        anim.SetBool("isFalling", false);
    }

    private IEnumerator ShieldBonus()
    {
        Health.Instance.ImmortalCatTrue();
        yield return new WaitForSeconds(5);
        Health.Instance.ImmortalCatFalse();
        _isImmortal = false;
        _healthNotChange.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        if (other.gameObject.tag == "Lose")
        {
            if (_isImmortal)
            {
                transform.parent.gameObject.SetActive(true);
            }
            else
            {
                ResetGame();
            }
        }
        if (other.gameObject.tag == "BonusFish")
        {
            _isSpeedIncrease = true;
        }
        if (other.gameObject.tag == "BonusShield")
        {
            _isImmortal = true;
        }
        if (other.gameObject.tag == "Coin")
        {
            counter++;
            int lastRunscore = int.Parse(CounterText.text.ToString());
            PlayerPrefs.SetInt("lastRunscore", lastRunscore);
            CounterText.text = counter.ToString();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-_lastVectorX);
        }
        if (collision.gameObject.tag == "Toaster")
        {
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Cubok1")
        {
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-10);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Cubok2")
        {
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Telek")
        {
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-40);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Plant")
        {
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-20);
            Destroy(collision.gameObject);
        }
    }
}
