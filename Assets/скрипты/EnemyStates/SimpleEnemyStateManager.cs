using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyStateManager : MonoBehaviour
{
    public DoorBaseState currentState;
    public DoorAgroState doorAgroState;
    public DoorAttackState doorAttackState;

    public Animator animator;
    public NavMeshAgent navMeshAgent;

    [SerializeField] private Transform doorTarget;  // Цель - дверь
    [SerializeField] private float _walkSpeed = 3.5f;
    [SerializeField] private float _attackDistance = 1.5f;

    public float walkSpeed { get { return _walkSpeed; } }
    public float attackDistance { get { return _attackDistance; } }

    private void Start()
    {
        // Начинаем с агрессии
        SwitchState(doorAgroState);
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

    // Установка скорости движения
    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    // Установка цели для навигации
    public void SetDestination(Transform target)
    {
        navMeshAgent.destination = target.position;
    }

    // Получение цели - двери
    public Transform GetDoorTarget()
    {
        return doorTarget;
    }

    // Расстояние до цели
    public float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, doorTarget.position);
    }
}
