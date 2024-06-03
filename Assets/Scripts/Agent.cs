using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour//Rabbit

{
    [SerializeField] Transform _desired;
    [SerializeField] float _speed;
    [SerializeField] float _radius;
    [SerializeField] Transform _hunter;

    private void Update()
    {
        Vector3 dir = _desired.position - transform.position;
        Vector3 hunterPos = _hunter.position - transform.position;
        if (hunterPos.magnitude < _radius)
        {
            transform.position -= hunterPos.normalized * _speed * Time.deltaTime;
        }
        else if (dir.magnitude > _radius && hunterPos.magnitude>_radius)
        {
            transform.position += dir.normalized * _speed * Time.deltaTime;
        }
        else Debug.Log("shot time");
        transform.right = dir;

        Debug.Log(Vector3.Distance(transform.position, _desired.position));
    }
    //NOTAS
    /* RB tiene su propio addforce al parecer- clase 3 se abura sin rb por ahora (1/2 de la clase) */
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }





}
