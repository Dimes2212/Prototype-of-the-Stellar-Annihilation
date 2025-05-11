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

    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float attackDistance = 1.5f;

    public float WalkSpeed => walkSpeed;
    public float AttackDistance => attackDistance;

    private void Start()
    {
        // Начинаем с ожидания
        SwitchState(doorIdleState);
    }

    private void Update()
    {
        // Обновляем текущее состояние
        currentState?.OnUpdate(this);
    }

    // Смена состояния
    public void SwitchState(DoorBaseState newState)
    {
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
        navMeshAgent.destination = target.position;
    }

    public Transform GetDoorTarget() => doorTarget;

    public Transform[] GetAttackPoints() => attackPoints;

    public Transform GetAvailableAttackPoint()
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
        return null;  // Если все точки заняты, возвращаем null
    }

    public float DistanceToTarget() => Vector3.Distance(transform.position, doorTarget.position);
}
