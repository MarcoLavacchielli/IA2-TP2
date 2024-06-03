using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySphere : MonoBehaviour
{
    //[SerializeField] float _force;
    Vector3 _velocity;
    [SerializeField] float _maxSpeed;

    [SerializeField] float _radius;
    [SerializeField] float _maxforce;
    [SerializeField] Transform _target;

    private void Update()
    {
        // Vector3 direction = (0, transform.position * _force,0);
        //punto 1
        //transform.position += Vector3.down * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            //transform.position += Vector3.left * Time.deltaTime;
            //_velocity += Vector3.left;
            AddForce(Vector3.left*_maxforce*Time.deltaTime);
        }
        //_velocity += Vector3.down;
        //_velocity.Normalize();

        AddForce(Vector3.down*_maxforce*Time.deltaTime);

        transform.position += _velocity * Time.deltaTime;
    }

    private void AddForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.right * 15f);
    }
    private void Bounce()
    {
        var lowerPos =transform.position+Vector3.down/2;
        if (lowerPos.y<=0&&_velocity.y>0)
        {
            _velocity *= -1;

        }
    }
}
