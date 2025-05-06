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
    [HideInInspector] public int currentPatrolIndex = 0;

    private Transform target;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public AttackState attackState = new AttackState();
    public PatrolState patrolState = new PatrolState();

    private void Start()
    {
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

    public Transform GetPlayer() => player;
}
