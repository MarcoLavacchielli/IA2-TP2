using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lucas Peck

public class Boid : SteeringAgent
{
    [SerializeField] Transform _seekTarget /*Objetivo*/, _fleeTarget /*Esquiva al Hunter*/;
    [SerializeField, Range(0f, 2.5f)] float _alignmentWeight = 1;
    [SerializeField, Range(0f, 2.5f)] float _separationWeight = 1;
    [SerializeField, Range(0f, 2.5f)] float _cohesionWeight = 1;

    private void Start()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        var dir = new Vector3(x, 0, z);

        _velocity = dir.normalized * _maxSpeed;

        GameManager.instance.allAgents.Add(this); // Agrega la lista del Game Manager
    }

    void Update()
    {
        Move();

        UpdateBoundPosition();

        if (Vector3.Distance(transform.position, _fleeTarget.position) <= _viewRadius)
        {
            AddForce(Flee(_fleeTarget.position));
        }
        else if (_seekTarget != null)
        {
            AddForce(Arrive(_seekTarget.position));
        }

        transform.position += _velocity * Time.deltaTime;

        if (_velocity != Vector3.zero)
        {
            // Ensure rotation only affects Y-axis (XZ plane movement)
            Quaternion rotation = Quaternion.LookRotation(_velocity, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }

        Flocking();
    }

    private void Flocking()//REEMPLAZAR POR LA BUSQUEDA CON QUERY 
    {
        var boids = GameManager.instance.allAgents;
        AddForce(Alignment(boids) * _alignmentWeight);
        AddForce(Separation(boids) * _separationWeight); // Se aplica un radio más chico al actual
        AddForce(Cohesion(boids) * _cohesionWeight);
    }

    private void UpdateBoundPosition()
    {
        transform.position = GameManager.instance.AdjustPostionToBounds(transform.position); // Ajusta los límites del Game Manager para que no se salgan de adentro.
    }

    protected bool HastToUseObstacleAvoidance()
    {
        Vector3 avoidanceObs = ObstacleAvoidance();
        AddForce(avoidanceObs);
        return avoidanceObs != Vector3.zero;
    }

    protected Vector3 ObstacleAvoidance()
    {
        if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, _viewRadius, _obstacles))
        {
            _velocity = transform.position - transform.up;

        }
        else if (Physics.Raycast(transform.position - transform.up * 0.5f, transform.forward, -_viewRadius, _obstacles))
        {
            _velocity = transform.position + transform.up;
        }
        else
        {
            _velocity = _seekTarget.transform.position - transform.position;
        }
        _velocity.y = 0; // Ensure the velocity remains on the XZ plane
        return Vector3.zero;
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
    }
}