/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPatrol : MonoBehaviour
{
    public GraphController graph;
    private int currentPatrolIndex = 0;
    public float speed = 2f;
    public Transform[] patrolPoints = new Transform[0];

    void Start()
    {
        graph = GetComponent<GraphController>();
        if (graph == null)
        {
            Debug.LogError("Graph component not found on the EnemyPatrol object.");
            return;
        }

        SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
    {

        if (patrolPoints.Length > 0)
        {
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.2f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
        else
        {
            Debug.LogWarning("No patrol points assigned to the enemy.");
        }
    }

    void SetDestination(Vector3 destination)
    {
        Vector3 currentPosition = transform.position;
        var path = graph.FindPath(currentPosition, destination);

        if (path != null && path.Count > 0)
        {
            foreach (Node node in path)
            {
                Vector3 nodePosition = node.Position;
                transform.Translate((nodePosition - currentPosition).normalized * Time.deltaTime * speed);
                currentPosition = transform.position;
            }
        }
        else
        {
            Debug.LogWarning("No valid path found.");
        }
    }
}
*/