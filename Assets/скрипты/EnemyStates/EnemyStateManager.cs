using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] public Collider damageCollider;
    [SerializeField] UnityEngine.AI.NavMeshAgent navMeshAgent;
    Transform target;
    [SerializeField] public Animator animator; 
    [SerializeField] Transform player;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float agroDistance;
    [SerializeField] public float attackDistance;



    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public AttackState attackState = new AttackState();

    public void SwitchState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        SwitchState(idleState);
    }

    private void Update()
    {
        SetDestination(player);
        navMeshAgent.destination = target.position;
        currentState.UpdateState(this);
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
        return (transform.position - target.transform.position).magnitude;
    }
    public void CheckConditions()
    {
        if (currentState != attackState)
        {
            if (DistanceToTarget() >= attackDistance)
            {
                
                
                SwitchState(agroState);
                

                return;
            }
        }
    }
    void OnOffDamager(int isOff)
    {
        if (isOff == 0)
        {
            damageCollider.enabled = false;
        }
        else
        {
            damageCollider.enabled = true;
        }
    }
}
