using UnityEngine;

public class GoToTarget : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] private Transform player;

    private void Update()
    {
        if (navMeshAgent != null && player != null)
        {
            navMeshAgent.destination = player.position;
        }
    }


}
