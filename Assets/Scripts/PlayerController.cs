using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    public enum state
    {
        move,
        jump,
        dash,
        slide,
        wallJump,
        grapple,
    }
    public state statevisualizer;

    public PlayerMoveModel moveModel = new PlayerMoveModel();
    public PlayerJumpModel jumpModel = new PlayerJumpModel();
    public PlayerDashModel dashModel = new PlayerDashModel();
    public PlayerSlideModel slideModel = new PlayerSlideModel();
    public PlayerGrappleModel grappleModel = new PlayerGrappleModel();

    public PlayerState generalState;
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerDashState dashState = new PlayerDashState();
    public PlayerSlidingState slideState = new PlayerSlidingState();
    public PlayerWallJumpState wallJumpState = new PlayerWallJumpState();
    public PlayerGrappleState grappleState = new PlayerGrappleState();

    [Header("Components")]
    public Rigidbody2D playerRB;
    public Animator playerAnim;

    private int groundHash;
    private int jumpVelHash;
    private int extraJumpTriggerHash;

    [Header("LayerMasks")]
    private LayerMask groundMask;

    //Vars for development (should not be serializing after build)
    /*[SerializeField] */
    private float groundHitDis = 0.28f;
    /*[SerializeField] */
    private float slideHitDis = 0.1f;
    [SerializeField] private float slideAllowHitDis = 1f;
    /*[SerializeField] */

    public Conveyor conveyor = null;
    DataTracker dt;
    GameObject EndScreen;

    TMP_Text textonscreen;
    public int unlocks = 0;
    int totalunlocks;

    public GameObject spawn;
    public bool teleport = false;
    float defaultspeed;

    Stopwatch stopwatch;
    int deaths = 0;

    Moving[] objectswithmoving;
    public SpawnGrapplePoint sgp;

    Slider healthbar;
    public float currenthealth;

    List<GameObject> collectiblespending = new List<GameObject>();

    public GameObject drone;

    public AudioSource audio;
    public AudioClip grapple;
    public AudioClip checkpoint;
    public AudioClip collectible;
    public AudioClip death;
    public AudioClip end;

    public GameObject checkpointEffect;
    public GameObject collectedEffect;

    // Start is called before the first frame update
    void Start()
    {
        statevisualizer = state.move;
        defaultspeed = this.moveModel.hspeed;

        spawn = GameObject.Find("Spawn");
        spawn.transform.localPosition = this.transform.localPosition;

        stopwatch = new Stopwatch();
        stopwatch.Start();

        objectswithmoving = FindObjectsOfType(typeof(Moving)) as Moving[];
        sgp = this.GetComponent<SpawnGrapplePoint>();

        EndScreen = GameObject.Find("EndScreen");
        EndScreen.gameObject.SetActive(false);

        healthbar = GameObject.Find("Health bar").GetComponent<Slider>();
        currenthealth = 1.0f;
        audio = GetComponent<AudioSource>();

        totalunlocks = GameObject.Find("Unlockables").transform.childCount;
        textonscreen = GameObject.Find("Texties").GetComponent<TMP_Text>();

        playerRB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        grappleModel.playerLine = GetComponent<LineRenderer>();
        grappleModel.playerLine.enabled = false;
        grappleModel.playerLine.SetPosition(0, transform.position);

        grappleModel.GrappleSensor = transform.Find("GrappleSensor").GetComponent<GrappleArea>();

        groundHash = Animator.StringToHash("isGrounded");
        extraJumpTriggerHash = Animator.StringToHash("ExJumpTrigger");
        jumpVelHash = Animator.StringToHash("jumpVelocity");

        groundMask = LayerMask.GetMask("Ground");
        dt = FindObjectOfType<DataTracker>().GetComponent<DataTracker>();
        ChangeState(moveState);
    }

    public void ChangeState(PlayerState newState)
    {
        if (generalState != null)
        {
            generalState.ExitState(this);
        }
        generalState = newState;
        if (generalState != null)
        {
            generalState.EnterState(this);
        }
    }

    string ConvertTimeToString(TimeSpan x)
    {
        string part = x.Seconds < 10 ? $"0{x.Seconds}" : $"{x.Seconds}";
        return $"{x.Minutes}:" + part + $".{x.Milliseconds}";
    }

    // Update is called once per frame
    void Update()
    {
        textonscreen.text = "Time: " + ConvertTimeToString(stopwatch.Elapsed) +
            $"\nDeaths: {deaths}" +
            $"\nCollectibles: {unlocks} / {totalunlocks}" +
            $"\nGrapples: {sgp.grappleMax-sgp.grappleList.Count} / {sgp.grappleMax}";

        healthbar.value = currenthealth;
        if (currenthealth <= 0f)
            teleport = true;

        playerAnim.SetBool(groundHash, jumpModel.isGrounded);
        playerAnim.SetFloat(jumpVelHash, playerRB.velocity.y);
        generalState.Update(this);
        checkGround();
        checkSlide();
        flip();
        //reset Platform Collision========================
        if (jumpModel.platformTimer > 0f)
        {
            jumpModel.platformTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.layer = 6;
        }

        //Timers
        if (slideModel.slidingCancelTimer >= 0) slideModel.slidingCancelTimer -= Time.deltaTime;
    }

    public void GotCollectible(GameObject x)
    {
        collectiblespending.Add(x);
        x.GetComponent<BoxCollider2D>().enabled = false;
        x.GetComponent<MeshRenderer>().enabled = false;
    }

    public void RemoveCollectible(bool y)
    {
        while (collectiblespending.Count > 0)
        {
            if (y)
            {
                GameObject x = collectiblespending[0];
                collectiblespending.RemoveAt(0);
                Destroy(x);
                unlocks++;
            }
            else
            {
                GameObject x = collectiblespending[0];
                collectiblespending.RemoveAt(0);
                x.GetComponent<BoxCollider2D>().enabled = true;
                x.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void OnMove(InputValue input)
    {
        /*if (generalState != dashState)*/
        moveModel.HorizontalMovement = input.Get<Vector2>().x;
        if (generalState != slideState)
        {
            if (moveModel.HorizontalMovement < 0f)
                moveModel.Direction = PlayerMoveModel.PlayerDirection.Left;
            else if (moveModel.HorizontalMovement > 0f)
                moveModel.Direction = PlayerMoveModel.PlayerDirection.Right;
        }
        moveModel.VerticalMovement = input.Get<Vector2>().y;
    }

    void OnJump()
    {
        if (generalState != jumpState && jumpModel.jumpCount > 0 && generalState != slideState && generalState != wallJumpState)
        {
            gameObject.layer = 6;
            if (moveModel.VerticalMovement >= 0f)
            {
                jumpModel.jumpCount -= 1;
                playerRB.velocity = Vector2.up * jumpModel.jumpSpeed;
                ChangeState(jumpState);

            }
            else
            {
                if (!jumpModel.isPlatform)
                {
                    jumpModel.jumpCount -= 1;
                    playerRB.velocity = Vector2.up * jumpModel.jumpSpeed;
                    ChangeState(jumpState);
                }
                else
                {
                    if (jumpModel.platformTimer <= 0f)
                    {
                        ChangeState(jumpState);
                        gameObject.layer = 7;
                        jumpModel.platformTimer = jumpModel.platformMaxTimer;
                    }
                }
            }
        }
        else if (generalState == slideState)
        {
            jumpModel.jumpCount -= 1;
            ChangeState(wallJumpState);
        }
        else
        {
            if (jumpModel.jumpCount > 0)
            {
                playerRB.velocity = Vector2.up * jumpModel.jumpSpeed;
                jumpModel.jumpCount -= 1;
                playerAnim.SetTrigger(extraJumpTriggerHash);

            }
        }
    }

    void OnDash()
    {
        if (generalState != dashState && dashModel.allowDash && generalState != slideState)
        {
            ChangeState(dashState);
        }
    }

    void OnQuit()
    {
        Application.Quit();
    }

    void OnGrapple()
    {
        Vector3 grappleDir = (grappleModel.point.transform.position - transform.position).normalized;
        float length = Vector3.Distance(transform.position, grappleModel.point.transform.position);
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, grappleDir, length, groundMask);
        if (hitGround.collider == null && generalState != dashState && grappleModel.point.activeSelf)
        {
            GameObject target = grappleModel.GrappleSensor.closestGrapplePoint;
            if (target != null) StartCoroutine(disableGrapplePoint(target));
            ChangeState(grappleState);
            audio.PlayOneShot(grapple, 0.2f);
        }
    }

    void flip()
    {
        transform.localScale = new Vector3((float)moveModel.Direction, transform.localScale.y, transform.localScale.z);
    }

    void checkGround()
    {
        RaycastHit2D hitJump = Physics2D.Raycast(transform.position + moveModel.groundSensor.transform.localPosition, Vector2.down, groundHitDis, groundMask);
        if (hitJump.collider != null)
        {
            jumpModel.isGrounded = true;
            if (hitJump.collider.CompareTag("Platform"))
                jumpModel.isPlatform = true;
            dashModel.allowDash = true;
            if (playerRB.gravityScale != slideModel.normalGravity)
                playerRB.gravityScale = slideModel.normalGravity;
            if (playerRB.velocity.y < 0f && generalState != dashState && generalState != moveState && generalState != grappleState)
            {
                ChangeState(moveState);
                jumpModel.jumpCount = jumpModel.jumpCountMax;
            }
        }
        else
        {
            jumpModel.isGrounded = false;
        }

        RaycastHit2D hitSlide = Physics2D.Raycast(transform.position + moveModel.groundSensor.transform.localPosition, Vector2.down, slideAllowHitDis, groundMask);
        if (hitSlide.collider != null)
        {
            slideModel.canSlide = false;
        }
        else
        {
            slideModel.canSlide = true;
        }
    }

    void checkSlide()
    {
        Vector3 HitSlideStart = transform.position +
            new Vector3(slideModel.slideDetectPos.transform.localPosition.x * (int)moveModel.Direction,
            slideModel.slideDetectPos.transform.localPosition.y,
            0);
        RaycastHit2D hit = Physics2D.Raycast(HitSlideStart,
            new Vector2((int)moveModel.Direction, 0), slideHitDis, groundMask);
        if (hit.collider != null && hit.collider.CompareTag("Slidable") && slideModel.canSlide && slideModel.slidingCancelTimer <= 0f && generalState != slideState)
        {
            ChangeState(slideState);
        }
        else if (hit.collider == null && generalState != jumpState && generalState != dashState && generalState != wallJumpState && generalState != grappleState)
        {
            ChangeState(moveState);
        }
    }

    IEnumerator disableGrapplePoint(GameObject point)
    {
        if (point != null)
            point.SetActive(false);
        yield return new WaitForSecondsRealtime(grappleModel.grapplePointExcludeCD);
        if (point != null)
            point.SetActive(true);
    }

    void FixedUpdate()
    {
        generalState.FixedUpdate(this);

        if (teleport)
        {
            audio.PlayOneShot(death, 0.75f);
            gameObject.transform.position = spawn.transform.position;
            drone.transform.position = new Vector3(spawn.transform.position.x -5, spawn.transform.position.y + 5, spawn.transform.position.z);
            moveModel.hspeed = 0;
            playerRB.velocity = new Vector2(0, 0);
            teleport = false;
            currenthealth = 1.0f;
            RemoveCollectible(false);
            ChangeState(moveState);
            StartCoroutine(Delay());
            sgp.ReloadGrapples();
            foreach (Moving x in objectswithmoving)
            {
                x.ResetPosition();
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
        moveModel.hspeed = defaultspeed;
    }

    public void LevelEnded()
    {
        audio.PlayOneShot(end, 0.5f);
        RemoveCollectible(true);
        stopwatch.Stop();
        dt.timetaken += stopwatch.Elapsed;
        EndScreen.SetActive(true);

        this.GetComponent<SpawnGrapplePoint>().enabled = false;
        this.GetComponent<PlayerInput>().enabled = false;
        ChangeState(moveState);

        int x = SceneManager.GetActiveScene().buildIndex;

        if (dt.levelcomplete[x] == false)
        {
            dt.levelcomplete[x] = true;
            dt.recordsno100[x] = stopwatch.Elapsed;
        }
        else if (unlocks < totalunlocks && dt.recordsno100[x] > stopwatch.Elapsed)
        {
            EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text += $"You beat your previous time by {ConvertTimeToString(dt.recordsno100[x]-stopwatch.Elapsed)}!\n";
            dt.recordsno100[x] = stopwatch.Elapsed;
        }
        if (unlocks >= totalunlocks && dt.allcollectibles[x] == false)
        {
            dt.allcollectibles[x] = true;
            dt.records100[x] = stopwatch.Elapsed;
            EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text += "You got all the collectibles!\n";
        }
        else if (unlocks >= totalunlocks && dt.records100[x] > stopwatch.Elapsed)
        {
            EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text += "You got all the collectibles!\n";
            EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text += $"You beat your previous time by {ConvertTimeToString(dt.records100[x] - stopwatch.Elapsed)}!\n";
            dt.records100[x] = stopwatch.Elapsed;

            if (dt.recordsno100[x] > stopwatch.Elapsed)
                dt.recordsno100[x] = stopwatch.Elapsed;
        }
        else if (unlocks >= totalunlocks)
        {
            EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text += "You got all the collectibles!\n";
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
            conveyor = collision.gameObject.GetComponent<Conveyor>();

        if (collision.gameObject.CompareTag("Spike"))
        {
            teleport = true;
            deaths++;
            dt.totaldeaths++;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
            conveyor = null;

    }
    
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Poison"))
        {
            currenthealth -= 0.010f;
        }
    }

}
