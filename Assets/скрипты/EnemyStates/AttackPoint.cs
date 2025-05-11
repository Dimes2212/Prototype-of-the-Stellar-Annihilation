using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public void SetOccupied(bool isOccupied)
    {
        IsOccupied = isOccupied;
    }
}
