using UnityEngine;
using UnityEngine.AI;

public class BossManager : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public bool move;
    bool swingAttack, pushAttack, useWater;
    public bool waterHazard;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", false);
    }

    void Update()
    {
        
    }
}
