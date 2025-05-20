//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Rendering;

//public class EnemyStateManager : MonoBehaviour
//{
//    [SerializeField] public Collider damageCollider;
//    [SerializeField] public NavMeshAgent navMeshAgent;
//    [SerializeField] public Animator animator;
//    [SerializeField] public Transform player;
//    [SerializeField] public float walkSpeed;
//    [SerializeField] public float agroDistance;
//    [SerializeField] public float attackDistance;


//    [SerializeField] public Transform[] patrolPoints;
//    [HideInInspector] public int currentPatrolIndex = 0;

//    private Transform target;

//    BaseState currentState;
//    public IdleState idleState = new IdleState();
//    public AgroState agroState = new AgroState();
//    public AttackState attackState = new AttackState();
//    public PatrolState patrolState = new PatrolState();
//    public DeathState deathState = new DeathState();

//    private void Start()
//    {
//        SwitchState(patrolState);
//    }

//    private void Update()
//    {
//        currentState?.UpdateState(this);
//    }

//    public void SwitchState(BaseState newState)
//    {
//        currentState?.ExitState(this);
//        currentState = newState;
//        currentState.EnterState(this);
//    }

//    public void SetSpeed(float newSpeed)
//    {
//        navMeshAgent.speed = newSpeed;
//    }

//    public void SetDestination(Transform newDestination)
//    {
//        target = newDestination;
//        if (navMeshAgent != null && target != null)
//        {
//            navMeshAgent.destination = target.position;
//        }
//    }

//    public float DistanceToTarget()
//    {
//        if (target == null) return Mathf.Infinity;
//        return Vector3.Distance(transform.position, target.position);
//    }

//    public void CheckConditions()
//    {

//        if (currentState == attackState && DistanceToTarget() >= attackDistance)
//        {
//            SwitchState(agroState);
//            return;
//        }
//    }

//    public void Die()
//    {

//        SwitchState(deathState);
//        animator.SetBool("isDead", true);

//        // Отключаем ИИ, передвижение и прочее
//        if (navMeshAgent != null) navMeshAgent.enabled = false;
//        this.enabled = false; // выключаем сам стейт-менеджер
//    }


//    public Transform GetPlayer() => player;
//}



using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] public Collider damageCollider;
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] public Animator animator;
    [SerializeField] public Transform player;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float agroDistance;
    [SerializeField] public float attackDistance;
    [SerializeField] public Transform[] patrolPoints;


    public AudioSource audioSource;
    public AudioClip deathClip;
    public AudioClip agroClip;
    public AudioClip attackClip;


    [HideInInspector] public int currentPatrolIndex = 0;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public AttackState attackState = new AttackState();
    public PatrolState patrolState = new PatrolState();
    public DeathState deathState = new DeathState();

    private Transform target;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>(); // страховка

        SwitchState(patrolState);
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }

    public void SwitchState(BaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetSpeed(float newSpeed)
    {
        navMeshAgent.speed = newSpeed;
    }

    public void SetDestination(Transform newDestination)
    {
        target = newDestination;
        if (navMeshAgent != null && target != null)
        {
            navMeshAgent.destination = target.position;
        }
    }

    public float DistanceToTarget()
    {
        if (target == null) return Mathf.Infinity;
        return Vector3.Distance(transform.position, target.position);
    }

    public void CheckConditions()
    {
        if (currentState == attackState && DistanceToTarget() >= attackDistance)
        {
            SwitchState(agroState);
            return;
        }
    }

    public void Die()
    {
        SwitchState(deathState);
        animator.SetBool("isDead", true);
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        this.enabled = false;
    }

    public Transform GetPlayer() => player;
}
