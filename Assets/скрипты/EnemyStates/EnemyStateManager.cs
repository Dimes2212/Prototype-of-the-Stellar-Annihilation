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
        SwitchState(patrolState); // Начинаем с патруля
    }

    private void Update()
    {
        if (target != null)
            navMeshAgent.destination = target.position;

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
        }
    }

    void OnOffDamager(int isOff)
    {
        damageCollider.enabled = isOff != 0;
    }

    public Transform GetPlayer() => player;
}
