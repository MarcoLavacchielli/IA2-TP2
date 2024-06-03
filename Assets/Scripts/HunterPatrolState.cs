using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Facundo Sebastian Tisera
public class HunterPatrolState : StateFather//=IdleState
{
    [SerializeField] float _maxSpeed;
    [SerializeField] float _radius;
    [SerializeField] float _maxForce;
    [SerializeField] Transform[] _wayPointsArray;
    Vector3 _velocity;
    [SerializeField] int _myWayPointInt;
    Renderer _rend;
    //public EnergyBar EnergyBarScript;

    public Transform HunterTransform;

    [SerializeField] LayerMask _obstacles;
    Vector3 _desired;

    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float DecreaseSpeedOfEnergy = 0.05f;

    public float TimeTakenForRecoveryEnergyValue;

    //FOV AGENT VARIABLES: aún no están en el constructor
    [SerializeField] GameObject _player;

    //[SerializeField] LayerMask _obstacle;

    [SerializeField, Range(1, 10)] float _viewRadius;
    [SerializeField, Range(1, 360)] float _viewAngle;
    public HunterPatrolState(HunterNPC p)//constructor
    {
        _maxSpeed = 5;
        _radius = 3;
        _maxForce = 3;
        _wayPointsArray = p._wayPointsArray;
        _myWayPointInt = 0;
        _rend = p.GetComponent<Renderer>();
        //EnergyBarScript = p.EnergyBarScript;

        HunterTransform = p.transform;
        _obstacles = p._obstacles;

        MaxEnergy = 1;
        MinEnergy = 0;
        DecreaseSpeedOfEnergy = 0.05f;

        _viewAngle = p.viewAngle;
        _viewRadius = p.viewRadius;
        _player = p.Player;
    }



    void IfIseePlayerUpdate()
    {
        /*foreach (var agent in _otherAgents)
        {
            agent.ChangeColor(InFieldOfView(agent.transform.position) ? Color.red : agent.myInitialMaterialColor);

        }
        */
        if (InFieldOfView(HunterTransform.position) == false)
        {
            _rend.material.color = Color.white;

        }
    }

    //FOV (Field of View)
    bool InFieldOfView(Vector3 endPos)
    {
        Vector3 dir = endPos - HunterTransform.position;
        if (dir.magnitude > _viewRadius) return false;
        if (!InLineOfSight(HunterTransform.position, endPos)) return false;
        if (Vector3.Angle(HunterTransform.forward, dir) > _viewAngle / 2) return false;
        return true;
    }

    //LOS (Line of Sight)
    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _obstacles);
    }



    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    //aca termina funiones de FOV AGENT
    public void ObstacleAvoidance()
    {
        if (Physics.Raycast(HunterTransform.position + HunterTransform.up * 0.5f, HunterTransform.right, _radius, _obstacles))
        {
            _desired = HunterTransform.position - HunterTransform.up;

        }
        else if (Physics.Raycast(HunterTransform.position - HunterTransform.up * 0.5f, HunterTransform.right, _radius, _obstacles))
        {
            _desired = HunterTransform.position + HunterTransform.up;
        }
        else
        {
            _desired = _wayPointsArray[_myWayPointInt].transform.position - HunterTransform.position;
        }
    }
    public float TimeTakenToStartRecoveringEnergy()
    {
        TimeTakenForRecoveryEnergyValue -= DecreaseSpeedOfEnergy * Time.deltaTime;

        return TimeTakenForRecoveryEnergyValue;
    }


    public override void ThisStateUpdate()//"Update" de este script
    {
        /* if (TimeTakenToStartRecoveringEnergy() <= 0.1)
         {
             EnergyBarScript.EnergyRecoveryIdleStateFunction();//Función que recarga la energía 

         }


         if (EnergyBarScript.currentEnergyValue >= 0.9f)
         {


             HunterFSM.ChangeState(PlayerStates.Move);
             Debug.Log("Se ha cambiado a estado Move del cazador");

         }
        */
        IfIseePlayerUpdate();

        Vector3 ActualDir = _wayPointsArray[_myWayPointInt].position - HunterTransform.position;
        if (ActualDir.magnitude < _radius)//IF para pasar de un waypoint a otro
        {
            if (_myWayPointInt >= _wayPointsArray.Length - 1)
            {
                _myWayPointInt = 0;
            }
            else
            {
                _myWayPointInt += 1;
            }
        }

        WayPointsSystem(_myWayPointInt);
        /*if(transform.position== _wayPointsArray[_myWayPointInt].transform.position)
        {
            if (_myWayPointInt >= _wayPointsArray.Length)
            {
                _myWayPointInt = 0;
            }
            else
            {
                _myWayPointInt += 1;
            }
        }*/

        HunterTransform.position += _velocity * Time.deltaTime;
        //para que mire al objetivo
        HunterTransform.right = _velocity;
    }

    private void WayPointsSystem(int ActualWaypoint)
    {
        _desired = _wayPointsArray[ActualWaypoint].transform.position - HunterTransform.position;
        _desired.Normalize();
        _desired *= _maxSpeed;

        Vector3 steering = _desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);


        if (_desired.magnitude < _radius)//IF para pasar de un waypoint a otro
        {
            if (_myWayPointInt >= _wayPointsArray.Length - 1)
            {
                _myWayPointInt = 0;
            }
            else
            {
                _myWayPointInt += 1;
            }
        }


    }

    private void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(HunterTransform.position, _radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(HunterTransform.position + HunterTransform.up * 0.5f, HunterTransform.position + HunterTransform.up * 0.5f + HunterTransform.right * _radius);
        Gizmos.DrawLine(HunterTransform.position - HunterTransform.up * 0.5f, HunterTransform.position - HunterTransform.up * 0.5f + HunterTransform.right * _radius);

        //GIZMOS DE FOV AGENT
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(HunterTransform.position, _viewRadius);

        Vector3 DirA = GetAngleFromDir(_viewAngle / 2 + HunterTransform.eulerAngles.y);
        Vector3 DirB = GetAngleFromDir(-_viewAngle / 2 + HunterTransform.eulerAngles.y);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(HunterTransform.position, HunterTransform.position + DirA.normalized * _viewRadius);
        Gizmos.DrawLine(HunterTransform.position, HunterTransform.position + DirB.normalized * _viewRadius);
    }

    public override void OnEnter()
    {
        Debug.Log("entre a idle");
        TimeTakenForRecoveryEnergyValue = MaxEnergy;
    }


    public override void OnExit()
    {
        Debug.Log("sali de idle");
    }
}
