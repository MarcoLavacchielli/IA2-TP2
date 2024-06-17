using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    [SerializeField] public float _maxSpeed, _maxForce; //Velocidad máxima a la que va el objeto y la fuerza con la que llega a dicho valor (A mayor Force, más rápido llega a la maxSpeed).
    [SerializeField] public float _viewRadius; //Rango de visión
    [SerializeField] public LayerMask _obstacles; //Layer de Obstaculos

    protected Vector3 _velocity; //Velocity /= Speed. Velocity mide a la velocidad que se mueve el objeto.

    protected void Move()
    {
        _velocity.y = 0; // Ensure Y component of velocity is zero
        transform.position += _velocity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z); // Ensure Y component of position is zero
        if (_velocity != Vector3.zero) transform.forward = _velocity;
    }

    protected Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, _maxSpeed);
    }

    protected Vector3 Seek(Vector3 targetPos, float speed) //Busca al objetivo
    {
        Vector3 desired = (targetPos - transform.position).normalized * speed;
        desired.y = 0; // Ensure Y component of desired direction is zero
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);
        return steering;
    }

    protected Vector3 Flee(Vector3 targetPos) => -Seek(targetPos);

    protected Vector3 Arrive(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist > _viewRadius) return Seek(targetPos);

        return Seek(targetPos, _maxSpeed * (dist / _viewRadius));
    }

    protected Vector3 Pursuit(SteeringAgent targetAgent) //Pursuit /= Seek. Seek busca dentro de un rango, Pursuit persigue un objetivo
    {
        Vector3 futurePos = targetAgent.transform.position + targetAgent._velocity;
        futurePos.y = 0; // Ensure Y component of future position is zero
        Debug.DrawLine(transform.position, futurePos, Color.cyan);

        return Seek(futurePos);
    }

    protected Vector3 Evade(SteeringAgent targetAgent) //Esquiva al Hunter
    {
        return -Pursuit(targetAgent);
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 0); // Ensure Y component of reset position is zero
    }

    protected Vector3 Alignment(List<SteeringAgent> agents) //Alineación de un conjunto de objetos o Flock
    {
        Vector3 desired = Vector3.zero;
        int boidsCount = 0;

        foreach (var item in agents)
        {
            //if (Vector3.Distance(item.transform.position, transform.position) > _viewRadius) continue;

            desired += item._velocity;
            boidsCount++;
        }

        desired /= boidsCount;
        desired.y = 0; // Ensure Y component of desired direction is zero

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    protected Vector3 Separation(List<SteeringAgent> agents) //Separación entre los objetos dentro de ese grupo o Flock
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in agents)
        {
            if (item == this) continue; //Ignorar mi propio calculo
            Vector3 dist = item.transform.position - transform.position;
            if (dist.sqrMagnitude > _viewRadius * _viewRadius) continue;
            desired += dist;
        }

        if (desired == Vector3.zero) return Vector3.zero;
        desired *= -1;
        desired.y = 0; // Ensure Y component of desired direction is zero

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    protected Vector3 Cohesion(List<SteeringAgent> agents) //Establece un promedio entre las distancias de los objetos 
    {
        Vector3 desired = Vector3.zero;
        int boidsCount = 0;

        foreach (var item in agents)
        {
            if (item == this) continue; //Ignorar mi propio calculo
            Vector3 dist = item.transform.position - transform.position;
            if (dist.sqrMagnitude > _viewRadius * _viewRadius) continue;
            //Promedio = Suma / Cantidad
            desired += item.transform.position;
            boidsCount++;
        }

        if (boidsCount == 0) return Vector3.zero; //Si no hay agentes
        desired /= boidsCount;
        desired.y = 0; // Ensure Y component of desired position is zero

        return Seek(desired);
    }

    protected Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - _velocity, _maxForce * Time.deltaTime);
    }

    protected void AddForce(Vector3 force)
    {
        force.y = 0; // Ensure Y component of force is zero
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.green;

        Vector3 leftRayPos = transform.position + transform.up * 0.5f;
        Vector3 rightRayPos = transform.position - transform.up * 0.5f;

        Gizmos.DrawLine(leftRayPos, leftRayPos + transform.forward * _viewRadius);
        Gizmos.DrawLine(rightRayPos, rightRayPos + transform.forward * _viewRadius);
    }
}