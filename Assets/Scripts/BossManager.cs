using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    PlayerMovement player;
    // This will handle all of the boss's movements, animations, attacks, etc.
    NavMeshAgent agent;
    Animator anim;
    Rigidbody rb;
    public bool move;
    [Header("All Attacks")]
    public bool swingAttack;
    public bool pushAttack;
    public bool useWater;
    [Header("Water Atack")]
    [SerializeField] GameObject water;
    [SerializeField] GameObject lightning;
    [SerializeField] Transform center;
    [SerializeField] Transform stands;
    bool waterHazard, isElectric, levitate, moveToStands;
    float waterStart = -1.0f, waterEnd = 0.8f, lightningDuration = 3.0f, waitForAnim = 3.3f;
    Vector3 waterPos, pos;
    enum WaterState
    {
        rising,
        risen,
        electric,
        falling,
        idle
    }
    WaterState state;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", false);
        state = WaterState.idle;
        lightning.SetActive(false);
        waterPos = water.transform.localPosition;
        waterStart = waterPos.y;
        pos = this.transform.localPosition;
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        //player = PlayerMovement.player;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
        AnimationHandler();
        WaterStateHandler();

        if (Input.GetKeyDown(KeyCode.T))
        {
            useWater = true;
        }
    }

    void FixedUpdate()
    {
        if(levitate) Float();   
    }

    void Movement()
    {
        water.transform.localPosition = waterPos;

        if (useWater)
        {
            if (Vector3.Distance(this.transform.position, center.position) > 2.0f)
            {
                if(agent.enabled) agent.SetDestination(center.position);
                pos = this.transform.localPosition;
            }
            else
            {
                if (agent.enabled)
                {
                    if (!agent.isStopped)
                    {
                        agent.velocity = Vector3.zero;
                        agent.isStopped = true;
                    }

                    if (agent.velocity == Vector3.zero)
                    {
                        anim.SetTrigger("push");
                        agent.enabled = false;
                    }
                }
                

                if (waitForAnim < 1.5f)
                {
                    levitate = true;
                }

                if (waitForAnim > 0f)
                {
                    waitForAnim -= Time.deltaTime;
                }
                else
                {
                    anim.ResetTrigger("push");
                    levitate = false;
                    waterHazard = true;
                    waitForAnim = 3.3f;
                    moveToStands = true;
                    useWater = false;
                }
            }
        } 
    }

    void AnimationHandler()
    {
        if (agent.velocity != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void Float()
    {
        //rb.AddForce(Vector3.up * 12.0f);
    }

    void StandsLerp()
    {
  
    }

    void WaterStateHandler()
    {
        if (state == WaterState.idle && waterHazard)
        {
            state = WaterState.rising;
        }
        else if (state == WaterState.rising)
        {
            RaiseWater();
        }
        else if (state == WaterState.risen)
        {
            // SETS AFTER ANIMATION
        }
        else if (state == WaterState.electric)
        {
            Lightning();
        }
        else if (state == WaterState.falling)
        {
            LowerWater();
        }
    }
    void RaiseWater()
    {
        if (waterPos.y < waterEnd)
        {
            waterPos.y += Time.deltaTime / 2;
        }
        else
        {
            waterPos.y = waterEnd;
            //state = WaterState.risen;
            state = WaterState.electric;
        }
    }

    void LowerWater()
    {
        if (waterPos.y > waterStart)
        {
            waterPos.y -= Time.deltaTime / 2;
        }
        else
        {
            waterPos.y = waterStart;
            waterHazard = false;
            state = WaterState.idle;
        }
    }

    void Lightning()
    {
        if (lightningDuration > 0f)
        {
            isElectric = true;
            if (lightning != null) lightning.SetActive(true);
            lightningDuration -= Time.deltaTime;
        }
        else
        {
            isElectric = false;
            lightningDuration = 3.0f;
            lightning.SetActive(false);
            state = WaterState.falling;
        }
    }
}
