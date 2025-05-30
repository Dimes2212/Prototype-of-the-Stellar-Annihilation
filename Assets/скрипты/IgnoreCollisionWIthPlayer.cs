using UnityEngine;

public class IgnoreCollisionWithPlayer : MonoBehaviour
{
    private void Start()
    {
        Collider[] weaponColliders = GetComponentsInChildren<Collider>();

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerObjects)
        {
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

            foreach (Collider weaponCol in weaponColliders)
            {
                foreach (Collider playerCol in playerColliders)
                {
                    Physics.IgnoreCollision(weaponCol, playerCol, true);
                }
            }
        }
    }
}
