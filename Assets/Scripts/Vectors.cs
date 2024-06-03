using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectors : MonoBehaviour//test de enseñanza de gizmos no sirve de nada. acá se había visto formas de devolver el calculo de pitagoras
{
    [SerializeField] Transform _cube;


    private void OnDrawGizmos()
    {
        Vector2 a = new Vector2(4, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, a);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(Vector3.zero, _cube.position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.ClampMagnitude(_cube.position,4));

    }
}
