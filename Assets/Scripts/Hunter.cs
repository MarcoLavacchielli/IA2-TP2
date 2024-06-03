using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Facundo Sebastian Tisera
public class Hunter : StateFather//=moveState
{
    [SerializeField] Transform _target;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _rangeToKill;
    [SerializeField] float _radius;
    [SerializeField] float _maxForce;
    
    [SerializeField] LayerMask _obstacles;
    [SerializeField] LayerMask _enemies;
    Vector3 _velocity;
    Vector3 _desired;

    [SerializeField] GameObject[] _arrayOfEnemies;
    [SerializeField] int _actualPrey;
    public Renderer _rend;
    public EnergyBar EnergyBarScript;

    public Transform HunterTransform;
    public Hunter(HunterNPC p)//constructor
    {
        _maxSpeed = 7;
        _rangeToKill = 1;
        _radius = 3;
        _maxForce = 3;
        _obstacles = p._obstacles;
        _enemies = p._enemies;
        _arrayOfEnemies = p._arrayOfEnemies;
        _actualPrey = 0;

        _actualPrey = 0;
        _rend = p.GetComponent<Renderer>();

        EnergyBarScript = p.EnergyBarScript;

        HunterTransform = p.transform;
    }

   

    private void seek()
    {
       
        _desired.Normalize();
        _desired *= _maxSpeed;

        Vector3 steering = _desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);

    }
   
    private void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

   
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
            _desired = _arrayOfEnemies[_actualPrey].transform.position- HunterTransform.position;
        }
    }
    public void RangeToKill()
    {
        
        if (Physics.Raycast(HunterTransform.position, HunterTransform.right, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, -HunterTransform.right, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, HunterTransform.up, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, -HunterTransform.up, _rangeToKill, _enemies))
        {
            if (_arrayOfEnemies[_actualPrey].gameObject == null)
            {
                _actualPrey += 1;
                
                _arrayOfEnemies[_actualPrey].gameObject.SetActive(false);
            }
            else
            {
               
                _arrayOfEnemies[_actualPrey].gameObject.SetActive(false);
                _actualPrey += 1;

            }
           
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(HunterTransform.position, _radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(HunterTransform.position + HunterTransform.up * 0.5f, HunterTransform.position + HunterTransform.up * 0.5f + HunterTransform.right * _radius);
        Gizmos.DrawLine(HunterTransform.position - HunterTransform.up * 0.5f, HunterTransform.position - HunterTransform.up * 0.5f + HunterTransform.right * _radius);
    }

    public override void OnEnter()
    {
        Debug.Log("Cazador entro a estado Move");
        _rend.material.color = Color.red;
    }
    
    
    public override void ThisStateUpdate()
    {
        
        ObstacleAvoidance();
        seek();


        HunterTransform.position += _velocity * Time.deltaTime;
        //para que mire al objetivo
        HunterTransform.right = _velocity;

        RangeToKill();

        EnergyBarScript.HuntingModeEnergyConsumption();//Función que consume la energía

        if (EnergyBarScript.currentEnergyValue <= 0.017f)
        {
            

            HunterFSM.ChangeState(PlayerStates.Idle);
            Debug.Log("Se ha cambiado a estado Idle del cazador");

        }

    }

    public override void OnExit()
    {
        Debug.Log("Cazador salio del estado Move");
        _rend.material.color = Color.white;
    }
}
