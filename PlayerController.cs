using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    Vector3 startGamePosition;
    Vector3 targetVelocity;
    Quaternion startGameRotation;


    [SerializeField] private float laneChangeSpeed = 15;
    [SerializeField] private Text CounterText;
    [SerializeField] private Text RecordText;
    Transform player;

    private CapsuleCollider col;
    public Animator anim;
    Rigidbody rb;
    Coroutine movingCoroutine;

    float pointStart;
    float laneOffset;
    float pointFinish;
    float lastVectorX;
    float jumpPower = 14;
    float jumpGravity = -40;
    float realGravity = -9.8f;

    private bool isMoving = false;
    private bool isImmortal;

    private bool isJumping = false;

    public static int counter;

    //public delegate void JumpDel();
    //public static event JumpDel jumpEvent;

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
        //jumpEvent += Jump;
        
    }

    private void Update()
    {
        if (transform.position.z < 0.3f)
        {
            Debug.Log("Z");
            float FixedMoveZ = transform.position.z;
            FixedMoveZ = 0.3f;
            transform.position = new Vector3(transform.position.x, transform.position.y,
            FixedMoveZ);
        }

    }
    public void StartGame()
    {
        anim.enabled = true;

        MouseController.Instance.StartGame();
        //anim.SetTrigger("Run");
    }

    public void StartLevel()
    {
        //StartGame();
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        anim.applyRootMotion = true;
        anim.enabled = false;
        anim.SetTrigger("isIdle");
        counter = 0;

        transform.position = startGamePosition;
        transform.rotation = startGameRotation;

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
    
    void Jump()
    {
        anim.applyRootMotion = false;
        anim.SetBool("isJump", true);
        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
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
        col.center = new Vector3(0, 0.28f, 1.2f);
        col.height = (0.58f);
        anim.SetBool("isFalling", true);

        yield return new WaitForSeconds(0.5f);

        col.center = new Vector3(0, 0.67f, 1.2f);
        col.height = 1.313049f;
        anim.applyRootMotion = true;
        anim.SetBool("isFalling", false);

    }

    private IEnumerator ShieldBonus()
    {
        isImmortal = true;

        yield return new WaitForSeconds(5);

        isImmortal = false;

    }

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
            StartCoroutine(RoadGenerator.Instance.SpeedIncrease());
        }
        if (other.gameObject.tag == "BonusShield")
        {
            StartCoroutine(ShieldBonus());
        }
        if (other.gameObject.tag == "Coin")
        {
            counter++;
            int lastRunscore = int.Parse(CounterText.text.ToString());
            //PlayerPrefs.SetInt("coins",counter);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-lastVectorX);
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
