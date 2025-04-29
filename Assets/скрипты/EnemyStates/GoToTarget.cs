using UnityEngine;

public class GoToTarget : MonoBehaviour
{
    [SerializeField] UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    private void Update()
    {
        if (navMeshAgent != null && player != null)
        {
            navMeshAgent.destination = player.position;
        }
    }


}
