using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyStateManager : MonoBehaviour
{
    public DoorBaseState currentState;
    public DoorIdleState doorIdleState;
    public DoorAgroState doorAgroState;
    public DoorAttackState doorAttackState;
    public DoorDeathState doorDeathState;

    public Animator animator;
    public NavMeshAgent navMeshAgent;

    [SerializeField] private Transform doorTarget;  // Цель - дверь
    [SerializeField] private Transform[] attackPoints;  // Точки для атаки

    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _attackDistance = 1.5f;

    public float walkSpeed { get { return _walkSpeed; } }
    public float attackDistance { get { return _attackDistance; } }

    private void Awake()
    {
        // Инициализация всех состояний
        doorIdleState = new DoorIdleState();
        doorAgroState = new DoorAgroState();
        doorAttackState = new DoorAttackState();
        doorDeathState = new DoorDeathState();
    }

    private void Start()
    {
        Debug.Log("SimpleEnemyStateManager started.");
        // Проверим, назначены ли все состояния и компоненты
        if (doorIdleState == null || doorAgroState == null || doorAttackState == null || doorDeathState == null)
        {
            Debug.LogError("One or more states are not assigned in the Inspector.");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned.");
            return;
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned.");
            return;
        }

        SwitchState(doorIdleState);  // Начинаем с ожидания
    }

    private void Update()
    {
        // Проверим, что состояние меняется
        currentState?.OnUpdate(this);

        // Обновляем анимации в зависимости от состояния
        UpdateAnimatorState();
    }

    // Смена состояния
    public void SwitchState(DoorBaseState newState)
    {
        if (currentState == newState)
            return; // Предотвращаем ненужный переход в одно и то же состояние

        Debug.Log("Switching to state: " + newState.GetType().Name); // Логируем переход

        currentState?.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    public void SetDestination(Transform target)
    {
        if (target == null) return;
        navMeshAgent.destination = target.position;
        navMeshAgent.isStopped = false;  // Убедитесь, что агент двигается
        Debug.Log("Setting destination to: " + target.position);  // Логируем установку цели
    }

    public Transform GetDoorTarget()
    {
        return doorTarget;
    }

    public Transform[] GetAttackPoints()
    {
        return attackPoints;
    }

    public Transform GetAttackPoint()
    {
        // Выбираем ближайшую свободную точку для атаки
        foreach (var point in attackPoints)
        {
            AttackPoint attackPoint = point.GetComponent<AttackPoint>();
            if (attackPoint != null && !attackPoint.IsOccupied)
            {
                return point;
            }
        }
        return null;
    }

    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, doorTarget.position);
    }

    private void UpdateAnimatorState()
    {
        if (currentState == doorIdleState)
        {
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttack", false);
        }
        else if (currentState == doorAgroState)
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttack", false);
        }
        else if (currentState == doorAttackState)
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttack", true);
        }
        else if (currentState == doorDeathState)
        {
            animator.SetBool("IsDead", true);
        }
    }
}
