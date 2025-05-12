using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyStateManager : MonoBehaviour
{
    // Состояния
    public DoorBaseState currentState;
    public DoorIdleState doorIdleState = new DoorIdleState();
    public DoorAgroState doorAgroState = new DoorAgroState();
    public DoorAttackState doorAttackState = new DoorAttackState();
    public DoorDeathState doorDeathState = new DoorDeathState();

    // Компоненты
    [Header("Components")]
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    // Настройки
    [Header("Settings")]
    [SerializeField] private DoorHealth doorHealth;
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private int _attackDamage = 10;

    // Зоны атаки
    [Header("Attack Zones")]
    [SerializeField] private Collider[] attackZones;

    // Свойства
    public float walkSpeed => _walkSpeed;
    public float attackDistance => _attackDistance;
    public float attackCooldown => _attackCooldown;
    public int attackDamage => _attackDamage;

    private void Awake()
    {
        // Инициализация компонентов
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

        // Дополнительные действия при смерти
        if (animator != null)
        {
            animator.SetTrigger("IsDead");
        }
    }

    // Остальные методы без изменений
    public void SetSpeed(float speed) => navMeshAgent.speed = speed;

    public void SetDestination(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        navMeshAgent.isStopped = false;
    }

    public float DistanceToCollider(Collider collider)
    {
        if (collider == null) return Mathf.Infinity;
        Vector3 closestPoint = collider.ClosestPoint(transform.position);
        return Vector3.Distance(transform.position, closestPoint);
    }

    public Collider GetNearestAttackZone()
    {
        if (attackZones == null || attackZones.Length == 0) return null;

        Collider nearestZone = null;
        float minDistance = float.MaxValue;

        foreach (var zone in attackZones)
        {
            if (zone == null || !zone.gameObject.activeSelf) continue;

            float distance = DistanceToCollider(zone);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestZone = zone;
            }
        }
        return nearestZone;
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