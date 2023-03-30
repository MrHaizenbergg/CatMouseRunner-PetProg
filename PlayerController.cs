using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject[] Weapons;
    [SerializeField] private float _moveSpeed = 10f;
    Transform player;

    private Health _healthNotChange;
    private CapsuleCollider col;
    public Animator anim;
    Rigidbody rb;
    Coroutine movingCoroutine;
    Coroutine shieldCoroutine;
    Coroutine speedCoroutineCat;

    float pointStart;
    float laneOffset;
    float pointFinish;
    float lastVectorX;
    float jumpPower = 14;
    float jumpGravity = -40;
    float realGravity = -9.8f;
    float intervalLight = 1f;

    private bool isSpeedIncrease;
    private bool isMoving = false;
    private bool isImmortal;
    private bool _isGreatWeaponJump;

    private bool isJumping = false;

    public static int counter;

    private void Awake()
    {
        _healthNotChange = GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        if (isSpeedIncrease)
            StartCoroutine(SpeedIncrease());
    }

    void Start()
    {
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

        laneOffset = MapGenerator.Instance.laneOffset;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        SwipeManager.instance.MoveEvent += MovePlayer;
        counter = PlayerPrefs.GetInt("coins");
        isImmortal = false;

    }

    public void StartGame()
    {
        anim.enabled = true;
        MouseController.Instance.StartGame();

    }

    public void StartLevel()
    {
        //StartCoroutine(ItemGeneratorFabric.Instance.ThrowItem());
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
        pointStart = 0;
        pointFinish = 0;
        anim.SetTrigger("isIdle");
        anim.applyRootMotion = true;
        anim.enabled = false;
        //isSpeedIncrease = false;
        counter = 0;

        transform.position = startGamePosition;
        transform.rotation = startGameRotation;

        ItemGeneratorFabric.Instance.StopThrowItem();
        //ShotGun.Instance.StopReloadCoroutine();
        LoseShotGun();
        LoseDynamit();
        LoseGreatWeapon();
        //StopCoroutine(speedCoroutineCat);
        Health.Instance.ChangeHealth(+100);
        HealthMouse.Instance.ChangeHealthMouse(+100);
        MouseController.Instance.ResetGame();
        RoadGenerator.Instance.ResetLevel();
    }

    void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeManager.Direction.Down] && isJumping == false)
        {
            StartCoroutine(Slide());
        }
        if (swipes[(int)SwipeManager.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Right] && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Up] && isJumping == false)
        {
            Jump();
        }

    }

    public IEnumerator SpeedIncrease()
    {
        rb.velocity = new Vector3(0, 0, 2f);
        yield return new WaitForSeconds(2);
        isSpeedIncrease = false;
    }

    public void PickUpShotGun()
    {
        anim.SetBool("isRiffleRun", true);
        Weapons[0].SetActive(true);
        activeShotGunButton.gameObject.SetActive(true);
    }
    public void LoseShotGun()
    {
        anim.SetBool("isRiffleRun", false);
        Weapons[0].SetActive(false);
        activeShotGunButton.gameObject.SetActive(false);
    }

    public void PickUpDynamit()
    {
        Weapons[1].SetActive(true);
        activeThrowDynamit.gameObject.SetActive(true);
    }
    public void LoseDynamit()
    {
        Weapons[1].SetActive(false);
        activeThrowDynamit.gameObject.SetActive(false);
    }

    public void PickUpGreatWeapon()
    {
        anim.SetBool("isGreatWeaponRun", true);
        _isGreatWeaponJump = true;
        Weapons[2].SetActive(true);
        activeGreatWeapon.gameObject.SetActive(true);
    }
    public void LoseGreatWeapon()
    {
        anim.SetBool("isGreatWeaponRun", false);
        _isGreatWeaponJump = false;
        Weapons[2].SetActive(false);
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

        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    public void Death()
    {
        RoadGenerator.Instance.StopGame();
        if (_isGreatWeaponJump==true)
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
        //RoadGenerator.Instance.TurnLeft();
        rb.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void MoveHorizontal(float speed)
    {
        anim.applyRootMotion = false;
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;

        if (isMoving)
        {
            StopCoroutine(movingCoroutine);
            isMoving = false;
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }

    private IEnumerator MoveCoroutine(float VectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(VectorX, rb.velocity.y, 0);
            lastVectorX = VectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {

            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        isMoving = false;
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
            isJumping = false;

            Physics.gravity = new Vector3(0, realGravity, 0);
        }
    }

    private IEnumerator Slide()
    {
        anim.applyRootMotion = false;
        anim.SetBool("isFalling", true);
        col.center = new Vector3(0, 0.28f, 1.27f);
        col.height = 0.58f;

        yield return new WaitForSeconds(0.8f);

        col.center = new Vector3(0, 0.67f, 0f);
        col.height = 1.313049f;
        anim.applyRootMotion = true;
        anim.SetBool("isFalling", false);

    }

    //private IEnumerator ShieldBonus()
    //{
    //    isImmortal = true;
    //    _healthNotChange.enabled = false;
    //    yield return new WaitForSeconds(5);
    //    isImmortal = false;
    //    _healthNotChange.enabled = true;

    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        if (other.gameObject.tag == "Lose")
        {
            if (isImmortal)
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
            isSpeedIncrease = true;
        }
        if (other.gameObject.tag == "BonusShield")
        {
            //StartCoroutine(ShieldBonus());
        }
        if (other.gameObject.tag == "Coin")
        {
            counter++;
            int lastRunscore = int.Parse(CounterText.text.ToString());
            PlayerPrefs.SetInt("lastRunscore", lastRunscore);
            CounterText.text = counter.ToString();
        }
        if (other.gameObject.tag == "CatTurnLeft")
        {

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
            MoveHorizontal(-lastVectorX);
        }
        if (collision.gameObject.tag == "Toaster")
        {
            //ItemGeneratorFabric.Instance.GetItem((ItemType)0);
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-20);
            Destroy(collision.gameObject);
            //StartCoroutine(ShieldBonus());
        }
        if (collision.gameObject.tag == "Cubok1")
        {
            //ItemGeneratorFabric.Instance.GetItem((ItemType)1);
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-10);
            Destroy(collision.gameObject);
            //shieldCoroutine=StartCoroutine(ShieldBonus());
        }
        if (collision.gameObject.tag == "Cubok2")
        {
            //ItemGeneratorFabric.Instance.GetItem((ItemType)2);
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-20);
            Destroy(collision.gameObject);
            //shieldCoroutine=StartCoroutine(ShieldBonus());
        }
        if (collision.gameObject.tag == "Telek")
        {
            // ItemGeneratorFabric.Instance.GetItem((ItemType)3);
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-40);
            Destroy(collision.gameObject);
            //shieldCoroutine = StartCoroutine(ShieldBonus());
        }
        if (collision.gameObject.tag == "Plant")
        {
            //ItemGeneratorFabric.Instance.GetItem((ItemType)4);
            LoseDynamit();
            LoseShotGun();
            LoseGreatWeapon();
            anim.SetTrigger("isHit");
            Health.Instance.ChangeHealth(-20);
            Destroy(collision.gameObject);
            //shieldCoroutine = StartCoroutine(ShieldBonus());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane")
        {
            if (rb.velocity.x == 0 && isJumping == false)
            {
                //rb.velocity = new Vector3(rb.velocity.x,-10,rb.velocity.z);
            }
        }
    }
}
