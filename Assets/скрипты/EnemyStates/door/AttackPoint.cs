//using UnityEngine;

//public class AttackPoint : MonoBehaviour
//{
//    public bool IsOccupied { get; private set; }

//    public void SetOccupied(bool isOccupied)
//    {
//        IsOccupied = isOccupied;
//    }
//}

using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public void SetOccupied(bool isOccupied)
    {
        IsOccupied = isOccupied;
    }

    // Поворот точки атаки в сторону цели (двери)
    public void OrientPoint(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Игнорируем ось Y для вертикальных перемещений
        Quaternion rotation = Quaternion.LookRotation(direction); // Поворот в сторону цели
        transform.rotation = rotation;
    }
}

