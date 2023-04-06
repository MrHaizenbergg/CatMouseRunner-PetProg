using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Singleton<MouseController>
{
    [SerializeField] private int speedX = 15;
    [SerializeField] private int jumpPower = 17;

    private Rigidbody _rb;
    private Animator _anim;
    private bool _isMoving = false;
    public bool isJumpingMouse = false;

    private bool _isSpeedForMouseIncrease;

    private Transform _transCat;
    private Coroutine _movingCoroutine;
    private Coroutine _randomLine;

    Vector3 startGamePosition;
    Vector3 targetVelocity;
    Quaternion startGameRotation;

    private float _lastVectorX;
    private float _pointStart;
    private float _pointFinish;
    private float _laneOffset = 2.5f;
    private float _jumpGravity = -40;
    private float _realGravity = -9.8f;

    public void StartGame()
    {
        _anim.enabled = true;
        if (isJumpingMouse == false)
        {
            StartCoroutine(RandomLine());
        }
    }

    public void ResetGame()
    {
        _rb.velocity = Vector3.zero;
        _pointStart = 0;
        _pointFinish = 0;

        _anim.applyRootMotion = true;
        _anim.enabled = false;
        _anim.SetTrigger("isTurnBack");

        transform.position = startGamePosition;
        transform.rotation = startGameRotation;

        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (_isSpeedForMouseIncrease)
            StartCoroutine(SpeedForMouseIncrease());
    }

    public IEnumerator SpeedForMouseIncrease()
    {
        _rb.velocity = new Vector3(0, 0, 2f);
        yield return new WaitForSeconds(2);
        _isSpeedForMouseIncrease = false;
    }

    public void DeathMouse()
    {
        RoadGenerator.Instance.StopGame();
        _anim.SetTrigger("isDyingMouse");
        _anim.applyRootMotion = true;
        ItemGeneratorFabric.Instance.StopThrowItem();
        StopCoroutine(_randomLine);
    }

    //public void MoveSlowMouse()
    //{
    //    //float time = Time.deltaTime * 5f;
    //    Vector3 pos = new Vector3(0, 0, -0.2f);
    //    _rb.AddForce(pos, ForceMode.Impulse);
    //}

    private void JumpMouse()
    {
        isJumpingMouse = true;

        _anim.applyRootMotion = false;
        _anim.SetBool("isJump", true);
        _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        Physics.gravity = new Vector3(0, _jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());
    }

    public void LookToCat()
    {
        Vector3 fromTo = _transCat.transform.position - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
    }


    private IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);
            _anim.SetBool("isJump", false);
        }
        while (_rb.velocity.y != 0);
        {
            isJumpingMouse = false;

            //Physics.gravity = new Vector3(0, realGravity, 0);
        }
    }

    private IEnumerator RandomLine()
    {
        if (isJumpingMouse == false)
        {
            int i = Random.Range(1, 3);
            switch (i)
            {
                case 1:
                    if (_pointFinish > -_laneOffset)
                        StartCoroutine(MoveHorizontal(-0.5f));
                    break;
                case 2:
                    if (_pointFinish < _laneOffset)
                        StartCoroutine(MoveHorizontal(0.5f));
                    break;
                default:
                    if (_pointFinish < -_laneOffset)
                        StartCoroutine(MoveHorizontal(-0.5f));
                    if (_pointFinish > _laneOffset)
                        StartCoroutine(MoveHorizontal(0.5f));
                    break;

            }
        }

        yield return new WaitForSeconds(3);
        _randomLine = StartCoroutine(RandomLine());
    }
    private IEnumerator MoveHorizontal(float speed)
    {
        _anim.applyRootMotion = false;
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;

        if (_isMoving)
        {
            StopCoroutine(_movingCoroutine);
            _isMoving = false;
        }
        yield return new WaitForFixedUpdate();
        _movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }


    private IEnumerator MoveCoroutine(float VectorX)
    {
        _isMoving = true;
        while (Mathf.Abs(_pointStart - transform.position.x) < _laneOffset)
        {
            yield return new WaitForFixedUpdate();

            _rb.velocity = new Vector3(VectorX * speedX, _rb.velocity.y, 0);
            _lastVectorX = VectorX;
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(_pointStart, _pointFinish), Mathf.Max(_pointStart, _pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        _rb.velocity = Vector3.zero;
        transform.position = new Vector3(_pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, -10, _rb.velocity.z);
        }
        _isMoving = false;
    }

    private enum TrackPos { Left = -1, Center = 0, Right = 1 }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle3")
            if (isJumpingMouse == false)
                JumpMouse();
        if (other.gameObject.name == "ColMouseRunLeft")
            StartCoroutine(MoveHorizontal((int)TrackPos.Left));
        if (other.gameObject.name == "ColMouseRunRight")
            StartCoroutine(MoveHorizontal((int)TrackPos.Right));
        if (other.gameObject.name == "ColSavePosition")
            StartCoroutine(MoveHorizontal(0));
        if (other.gameObject.name == "Cheese")
        {
            _isSpeedForMouseIncrease = true;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _transCat = GetComponent<Transform>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
    }
}
