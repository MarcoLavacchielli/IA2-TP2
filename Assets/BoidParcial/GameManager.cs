using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lucas Peck

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float _boundWidth;
    [SerializeField] float _boundHeight;

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
        Gizmos.DrawWireCube(transform.position, new Vector3(_boundWidth, _boundHeight));
    }

    public Vector3 AdjustPostionToBounds(Vector3 pos)
    {
        //Pasar los limites y dividiendolos

        float height = _boundHeight / 2;
        float width = _boundWidth / 2;

        if (pos.y > height) pos.y = -height;
        if (pos.y < -height) pos.y = height;

        if (pos.x > width) pos.x = -width;
        if (pos.x < -width) pos.x = width;

        return pos;
    }

}
