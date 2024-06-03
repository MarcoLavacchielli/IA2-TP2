/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : MonoBehaviour
{
    public Vector3 Position { get { return transform.position; } }
    public bool IsWalkable { get; set; } = true;
    public List<Node> Connections { get; set; } = new List<Node>();
    public Node Parent { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get { return GCost + HCost; } }

    void OnDrawGizmos()
    {
        Gizmos.color = IsWalkable ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 1000f);

        foreach (Node connection in Connections)
        {
            Gizmos.DrawLine(transform.position, connection.transform.position);
        }
    }

    void Start()
    {
        Debug.Log($"Node at {transform.position} started.");
    }
}*/
