using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class SimpleEnemyStateManager : MonoBehaviour
{
    public DoorBaseState currentState;
    public DoorIdleState doorIdleState = new DoorIdleState();
    public DoorAgroState doorAgroState = new DoorAgroState();
    public DoorAttackState doorAttackState = new DoorAttackState();
    public DoorDeathState doorDeathState = new DoorDeathState();

    public Animator animator;
    public NavMeshAgent navMeshAgent;

    [SerializeField] private DoorHealth doorHealth;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private int _attackDamage = 10;

    [SerializeField] private Collider[] attackZones;

    public float walkSpeed => _walkSpeed;
    public float attackDistance => _attackDistance;
    public float attackCooldown => _attackCooldown;
    public int attackDamage => _attackDamage;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SwitchState(doorIdleState);
    }

    private void Update()
    {
        if (currentState != doorDeathState)
        {
            currentState?.OnUpdate(this);
            UpdateAnimatorState();
        }
    }

    public void SwitchState(DoorBaseState newState)
    {
        if (currentState == newState) return;

        currentState?.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void Die()
    {
        SwitchState(doorDeathState);
        animator.SetTrigger("Die");

        // Отключаем ИИ, передвижение и прочее
        //if (navMeshAgent != null) navMeshAgent.enabled = false;
        //this.enabled = false; // выключаем сам стейт-менеджер
    }

    public void DestroySelf()
    {
        Debug.Log("Удаляем");
        Destroy(gameObject);
    }

    public void SetSpeed(float speed) => navMeshAgent.speed = speed;

    public void SetAttackDestination(Collider zone)
    {
        if (zone == null) return;

        var attackPoint = zone.GetComponent<AttackPoint>();
        Vector3 destination = attackPoint != null
            ? attackPoint.GetRandomPositionInZone()
            : zone.transform.position;

        navMeshAgent.SetDestination(destination);
        navMeshAgent.isStopped = false;
    }

    public Collider GetNearestAttackZone()
    {
        if (attackZones == null || attackZones.Length == 0) return null;

        Collider nearestZone = null;
        float minDistance = float.MaxValue;

        foreach (var zone in attackZones)
        {
            if (zone == null || !zone.gameObject.activeSelf) continue;

            var attackPoint = zone.GetComponent<AttackPoint>();
            Vector3 targetPoint = attackPoint != null
                ? attackPoint.GetRandomPositionInZone()
                : zone.transform.position;

            float distance = Vector3.Distance(transform.position, targetPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestZone = zone;
            }
        }
        return nearestZone;
    }

    public float DistanceToCollider(Collider collider)
    {
        if (collider == null) return Mathf.Infinity;

        Vector3 closestPoint = collider.ClosestPoint(transform.position);
        return Vector3.Distance(transform.position, closestPoint);
    }

    public void AttackDoor()
    {
        if (doorHealth != null && !doorHealth.IsDead)
        {
            doorHealth.TakeDamage(attackDamage);
        }
    }

    private void UpdateAnimatorState()
    {
        animator.SetBool("IsIdle", currentState == doorIdleState);
        animator.SetBool("IsWalking", currentState == doorAgroState);
        animator.SetBool("IsAttack", currentState == doorAttackState);
    }
}
