using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    // This will handle all of the boss's movements, animations, attacks, etc.
    NavMeshAgent agent;
    Animator anim;
    public bool move;
    [Header("All Attacks")]
    public bool swingAttack;
    public bool pushAttack;
    public bool useWater;
    [Header("Water Atack")]
    [SerializeField] GameObject water;
    [SerializeField] GameObject lightning;
    [SerializeField] Transform center;
    bool waterHazard, isElectric;
    float waterStart = -1.0f, waterEnd = 0.8f, lightningDuration = 3.0f, waitForAnim = 3.3f;
    Vector3 waterPos;
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

    void Movement()
    {
        water.transform.localPosition = waterPos;

        if (useWater)
        {
            if (Vector3.Distance(this.transform.position, center.position) > 2.0f)
            {
                agent.SetDestination(center.position);
            }
            else
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                if (agent.velocity == Vector3.zero)
                {
                    anim.SetTrigger("push");
                }

                if (waitForAnim > 0f)
                {
                    waitForAnim -= Time.deltaTime;
                }
                else
                {
                    anim.ResetTrigger("push");
                    waterHazard = true;
                    waitForAnim = 3.3f;
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
