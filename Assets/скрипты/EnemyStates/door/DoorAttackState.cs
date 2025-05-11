//using UnityEngine;

//public class DoorAttackState : DoorBaseState
//{
//    private Transform doorTarget;
//    private float attackCooldown = 1.5f;
//    private float lastAttackTime;

//    public override void OnEnter(SimpleEnemyStateManager manager)
//    {
//        Debug.Log("Entering AttackState");

//        doorTarget = manager.GetDoorTarget();

//        manager.animator.SetBool("IsWalking", false);  // Останавливаем анимацию ходьбы
//        manager.animator.SetBool("IsAttack", true);  // Включаем анимацию атаки

//        lastAttackTime = Time.time;
//    }

//    public override void OnExit(SimpleEnemyStateManager manager)
//    {
//        manager.animator.SetBool("IsAttack", false);  // Отключаем анимацию атаки при выходе
//    }

//    public override void OnUpdate(SimpleEnemyStateManager manager)
//    {
//        if (doorTarget == null) return;

//        if (Time.time - lastAttackTime >= attackCooldown)
//        {
//            lastAttackTime = Time.time;

//            // Наносим урон двери
//            var doorHealth = doorTarget.GetComponent<DoorHealth>();
//            if (doorHealth != null && !doorHealth.IsDead())
//            {
//                doorHealth.TakeDamage(10);  // Параметр 10 — это количество урона
//                Debug.Log("Attacking door at " + doorTarget.position);
//            }
//        }
//    }
//}


//using UnityEngine;

//public class DoorAttackState : DoorBaseState
//{
//    private Transform doorTarget;
//    private float attackCooldown = 1.5f;
//    private float lastAttackTime;

//    private float rotationSpeed = 5f;  // Скорость поворота персонажа

//    public override void OnEnter(SimpleEnemyStateManager manager)
//    {
//        Debug.Log("Entering AttackState");

//        doorTarget = manager.GetDoorTarget();

//        manager.animator.SetBool("IsWalking", false);  // Останавливаем анимацию ходьбы
//        manager.animator.SetBool("IsAttack", true);  // Включаем анимацию атаки

//        lastAttackTime = Time.time;
//    }

//    public override void OnExit(SimpleEnemyStateManager manager)
//    {
//        manager.animator.SetBool("IsAttack", false);  // Отключаем анимацию атаки при выходе
//    }

//    public override void OnUpdate(SimpleEnemyStateManager manager)
//    {
//        if (doorTarget == null) return;

//        // Поворот к двери перед атакой
//        RotateTowardsTarget(manager);

//        // Проверка на время между атаками
//        if (Time.time - lastAttackTime >= attackCooldown)
//        {
//            lastAttackTime = Time.time;

//            // Наносим урон двери
//            var doorHealth = doorTarget.GetComponent<DoorHealth>();
//            if (doorHealth != null && !doorHealth.IsDead())
//            {
//                doorHealth.TakeDamage(10);  // Параметр 10 — это количество урона
//                Debug.Log("Attacking door at " + doorTarget.position);
//            }
//        }

//        // Проверка на столкновения или другие влияния (например, если персонажа пытаются сдвинуть)
//        // Это можно дополнительно расширить в зависимости от поведения других объектов.
//        // Здесь просто пример, как можно обновить позицию с учетом столкновений:
//        HandleCollisions(manager);
//    }

//    // Метод для поворота персонажа к цели
//    private void RotateTowardsTarget(SimpleEnemyStateManager manager)
//    {
//        Vector3 directionToTarget = doorTarget.position - manager.transform.position;
//        directionToTarget.y = 0f;  // Игнорируем изменение по оси Y (чтобы только в плоскости вращались)
//        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

//        // Плавный поворот в сторону цели
//        manager.transform.rotation = Quaternion.Slerp(manager.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
//    }

//    // Обработка столкновений (упрощённый вариант)
//    private void HandleCollisions(SimpleEnemyStateManager manager)
//    {
//        // Например, можно проверить, если персонаж столкнулся с чем-то, что не является дверью
//        // и попытаться немного корректировать его позицию, если необходимо.
//        RaycastHit hit;
//        if (Physics.Raycast(manager.transform.position, manager.transform.forward, out hit, 1f))
//        {
//            // Если мы столкнулись с чем-то, можем обработать это, например, развернув персонажа
//            Debug.Log("Collided with: " + hit.collider.name);

//            // Пример — поворот в случайную сторону, чтобы персонаж не застревал
//            if (hit.collider.CompareTag("Obstacle")) // Предположим, что столкновение с препятствием
//            {
//                Vector3 randomDirection = Random.insideUnitSphere * 5f;
//                randomDirection.y = 0f;  // Не изменяем по оси Y
//                manager.navMeshAgent.destination = manager.transform.position + randomDirection;  // Ставим новую цель
//            }
//        }
//    }
//}

using UnityEngine;

public class DoorAttackState : DoorBaseState
{
    private Transform doorTarget;
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void OnEnter(SimpleEnemyStateManager manager)
    {
        Debug.Log("Entering AttackState");

        doorTarget = manager.GetDoorTarget();

        manager.animator.SetBool("IsWalking", false);  // Останавливаем анимацию ходьбы
        manager.animator.SetBool("IsAttack", true);  // Включаем анимацию атаки

        lastAttackTime = Time.time;

        // Получаем точку атаки и ориентируем её на дверь
        Transform attackPoint = manager.GetAttackPoint();
        if (attackPoint != null)
        {
            AttackPoint pointComponent = attackPoint.GetComponent<AttackPoint>();
            if (pointComponent != null)
            {
                pointComponent.OrientPoint(doorTarget.position);  // Ориентируем точку на дверь
            }
        }
    }

    public override void OnExit(SimpleEnemyStateManager manager)
    {
        manager.animator.SetBool("IsAttack", false);  // Отключаем анимацию атаки при выходе
    }

    public override void OnUpdate(SimpleEnemyStateManager manager)
    {
        if (doorTarget == null) return;

        // Врагу не нужно поворачиваться, так как точка атаки уже повернута

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // Наносим урон двери
            var doorHealth = doorTarget.GetComponent<DoorHealth>();
            if (doorHealth != null && !doorHealth.IsDead())
            {
                doorHealth.TakeDamage(10);  // Параметр 10 — это количество урона
                Debug.Log("Attacking door at " + doorTarget.position);
            }
        }
    }
}
