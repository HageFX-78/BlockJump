using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerAction : MonoBehaviour
{
    [Header("Control Settings")]
    float firstTapTime;
    [SerializeField] float durationBetweenTaps;
    [SerializeField] bool hasFirstTapped = false;

    [SerializeField] float dragDeadzone;
    Vector3 firstTouchPos;
    [SerializeField] bool isDragging;

    [SerializeField] float swipeDeadZone;
    [SerializeField] float swipeDuration;
    [SerializeField] float swipeDistance;

    [Header("References")]
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI coinCount;
    [SerializeField] UIController uiCon;
    [SerializeField] Animator anim;

    AudioManager am;
    private int currentLane;
    public Rigidbody rb;
    IEnumerator switchLane;
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStr;
    [SerializeField] float dropStr;
    [SerializeField] float switchLaneDuration;
    [SerializeField] float switchLaneFrameSplit;
    [SerializeField] float moveDistance;
    bool canJump, lockL, lockR;

    [Header("Test Jump Settings")]
    [SerializeField] float jumpDuration;
    [SerializeField] float jumpFrameSplit;
    [SerializeField] float jumpDistance;

    //data
    int gotCoins;
    int currentScore;
    int scoreMultiplier, coinMultiplier, hardModeMultiplier;
    float moveIncrementAmt;

    private void Awake()
    {
        am = AudioManager.amInstance;
    }
    private void Start()
    {
        gotCoins = 0;
        currentScore = 0;
        scoreMultiplier = 1;

        if (PlayerPrefs.GetInt("item0")==1)
        {
            coinMultiplier = 2;
        }
        else
        {
            coinMultiplier = 1;
        }
        StartCoroutine(moveSpeedIncrement());
        if (PlayerPrefs.GetInt("hmActive") == 1)
        {
            hardModeMultiplier = 2;
            moveIncrementAmt = 0.3f;
        }
        else
        {
            hardModeMultiplier = 1;
            moveIncrementAmt = 0.1f;
        }

        currentLane = 1;
        canJump = true;
        lockL = false;//Lock move left
        lockR = false;//Lock move right

        StartCoroutine(scoreCounter());
    }
    private void Update()
    {   
        if(Time.timeScale!=0)
        {
            if (Time.time > firstTapTime + durationBetweenTaps)
            {
                hasFirstTapped = false;
            }

            //TAP
            if (Input.GetMouseButtonDown(0) && !hasFirstTapped)
            {
                //Debug.Log($"Tapped!");
                hasFirstTapped = true;
                firstTapTime = Time.time;
                firstTouchPos = Input.mousePosition;
            }
            //SWIPE
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 swipeDelta = Input.mousePosition - firstTouchPos;
                if (swipeDelta != Vector2.zero && Time.time - firstTapTime <= swipeDuration && swipeDelta.magnitude>= swipeDistance)
                {
                    OnSwipe(swipeDelta);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, 1 * moveSpeed * Time.deltaTime);

        
    }
    void OnSwipe(Vector2 sd)
    {
        float positiveX = Mathf.Abs(sd.x);
        float positiveY = Mathf.Abs(sd.y);

        if (positiveX > positiveY)
        {
            if (sd.x > 0)
            {
                if(currentLane<2 && !lockR)
                {
                    currentLane++;
                    //switchLane = moveToSide(true);
                    am.plySF("drop");
                    StartCoroutine(moveToSide(true));                
                }              
                //Debug.Log($"Swiped Right");
            }
            else
            {
                if (currentLane >0 && !lockL)
                {
                    currentLane--;
                    am.plySF("drop");
                    //switchLane = moveToSide(false);
                    StartCoroutine(moveToSide(false));

                }
                //Debug.Log($"Swiped Left");
            }
        }
        else if (positiveX < positiveY)
        {
            if (sd.y > 0 && canJump)
            {

                canJump = false;
                rb.AddForce(Vector3.up * jumpStr, ForceMode.Impulse);
                if(anim!= null)
                {
                    anim.SetBool("isJumping", true);
                }
                //StartCoroutine(jump());
                am.plySF("jump");
            }
            else if(sd.y < 0 && !canJump)
            {
                rb.AddForce(Vector3.down * dropStr, ForceMode.Impulse);
                Invoke("fixPosition", .3f);
                if (anim != null)
                {
                    anim.SetBool("forcedDrop", true);
                }
                am.plySF("drop");
            }
        }
        //*/
    }
    IEnumerator moveToSide(bool right)
    {
        float countDown = 0;
        float splitOffset = switchLaneDuration / switchLaneFrameSplit;
        float splitDist = moveDistance / switchLaneFrameSplit;
        while(countDown< switchLaneFrameSplit)
        {
            if(right)
            {
                transform.position += new Vector3(splitDist, 0, 0);
            }
            else
            {
                transform.position -= new Vector3(splitDist, 0, 0);
            }
            countDown++;
            yield return new WaitForSeconds(splitOffset);
        }
        fixPosition();
    }

    private void fixPosition()
    {
        switch (currentLane)
        {
            case 0:
                transform.position = new Vector3(-3, transform.position.y, transform.position.z);
                break;
            case 1:
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(3, transform.position.y, transform.position.z);
                break;
        }
    }
    //Test jump
    IEnumerator jump()
    {
        float countDown = 0;
        float splitOffset = jumpDuration / jumpFrameSplit;
        float splitDist = jumpDistance / jumpFrameSplit;
        while (countDown < jumpFrameSplit)
        {

            transform.position += new Vector3(0, splitDist, 0);
            countDown++;
            yield return new WaitForSeconds(splitOffset);
        }

    }
    IEnumerator scoreCounter()
    {
        while (true)
        {
            currentScore += 100 * scoreMultiplier * hardModeMultiplier;
            score.text = $"{currentScore}";
            yield return new WaitForSeconds(0.5f);
        }

    }
    IEnumerator moveSpeedIncrement()
    {
        while(true)
        {
            moveSpeed += moveIncrementAmt;
            yield return new WaitForSeconds(2f);
        }
    }

    //--------------------------------------------------------------------Collision and trigger actions--------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.CompareTag("Jumpable"))
        {
            canJump = true;
            if (anim != null)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("forcedDrop", false);
            }
        }
        else if (collision.transform.CompareTag("Obstacle"))
        {
            am.plySF("death");
            uiCon.gameOver(gotCoins, currentScore);
        }
        
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("coin"))
        {
            other.gameObject.SetActive(false);
            gotCoins += 1*coinMultiplier* hardModeMultiplier;
            currentScore += 100* hardModeMultiplier;
            coinCount.text = $"Coins\n{gotCoins}";

            am.plySF("coin");
        }
        else if (other.transform.CompareTag("bigcoin"))
        {
            other.gameObject.SetActive(false);
            gotCoins += 10* coinMultiplier* hardModeMultiplier;
            currentScore += 1000* hardModeMultiplier;
            coinCount.text = $"Coins\n{gotCoins}";

            am.plySF("bigCoin");
        }
        else if(other.transform.CompareTag("SlideLockL"))//Hit left collision box so cant move to the right where there's a platform
        {
            lockR = true;
        }
        else if (other.transform.CompareTag("SlideLockL"))
        {
            lockL = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.transform.CompareTag("SlideLockL"))//Hit left collision box so cant move to the right where there's a platform
        {
            lockR = false;
        }
        else if (other.transform.CompareTag("SlideLockL"))
        {
            lockL = false;
        }
    }
}
