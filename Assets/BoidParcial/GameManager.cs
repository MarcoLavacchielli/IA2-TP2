using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lucas Peck

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float _boundWidth;
    [SerializeField] float _boundDepth;  // Cambiado de _boundHeight a _boundDepth

    public List<SteeringAgent> allAgents = new List<SteeringAgent>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        // Cambiado para usar _boundDepth en lugar de _boundHeight
        Gizmos.DrawWireCube(transform.position, new Vector3(_boundWidth, 0, _boundDepth));
    }

    public Vector3 AdjustPostionToBounds(Vector3 pos)
    {
        // Pasar los límites y dividirlos

        float depth = _boundDepth / 2;  // Cambiado de height a depth
        float width = _boundWidth / 2;

        if (pos.z > depth) pos.z = -depth;
        if (pos.z < -depth) pos.z = depth;

        if (pos.x > width) pos.x = -width;
        if (pos.x < -width) pos.x = width;

        return pos;
    }
}