using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// Falta el On draw Gizmos que est� en HunterPatrolStatePeroNoTieneMuchoSentidoPasarlo. Tambien el darw Gizmos de Hunter(el chase state)
public class HunterIA2 : MonoBehaviour
{
    [SerializeField] float _maxSpeed;
    [SerializeField] float _radius;
    [SerializeField] float _maxForce;
    [SerializeField] Transform[] _wayPointsArray;
    Vector3 _velocity;
    [SerializeField] int _myWayPointInt;
    Renderer _rend;
    public EnergyBar EnergyBarScript;

    public Transform HunterTransform;

    [SerializeField] LayerMask _obstacles;
    public Vector3 _desired;

    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float DecreaseSpeedOfEnergy = 0.05f;

    public float TimeTakenForRecoveryEnergyValue;

    //FOV AGENT VARIABLES: a�n no est�n en el constructor
    [SerializeField] GameObject _player;

    //[SerializeField] LayerMask _obstacle;

    [SerializeField, Range(1, 10)] float _viewRadius;
    [SerializeField, Range(1, 360)] float _viewAngle;

    [Header("nuevos Stats para el g�nerico")]
    private FiniteStateMachineIA2<States> _fsm;
    private enum States { Idle, Moving, DyingBreath, InmuneToDeath }
    #region Variables para Chase
    [SerializeField] GameObject[] _arrayOfEnemies;
    [SerializeField] int _actualPrey;
    [SerializeField] float _rangeToKill;
    [SerializeField] LayerMask _enemies;
    [Header("Variables Query")]
    public bool isBox;
    public float radius = 20f;
    public SpatialGrid targetGrid;
    public float width = 15f;
    public float height = 30f;
    public IEnumerable<GridEntity> selected = new List<GridEntity>();
    public GameObject MyPreyGO;
    public Transform standarHuntingPosition;
    public bool AlreadyHaveAPrey;

    #region Query

    public void CollectingBoidsToHunt()
    {
        if (AlreadyHaveAPrey == false)
        {
            var firstItem = Query().FirstOrDefault();

            if (firstItem != null)
            {
                MyPreyGO = firstItem.gameObject;
                AlreadyHaveAPrey = true;
            }

        }




    }
    public IEnumerable<GridEntity> Query()
    {


        var h = height * 0.5f;
        var w = width * 0.5f;

        return targetGrid.Query(
            transform.position + new Vector3(-w, 0, -h),
            transform.position + new Vector3(w, 0, h),
            x => true);

    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
        if (Application.isPlaying)
        {
            selected = Query();
            var temp = FindObjectsOfType<GridEntity>().Where(x => !selected.Contains(x));
            foreach (var item in temp)
            {
                item.onGrid = false;
            }
            foreach (var item in selected)
            {
                item.onGrid = true;
            }


        }

    }
    #endregion
    #endregion
    private void Start()
    {
        _rend = GetComponent<Renderer>();
        SetUpFSM();
    }


    #region IdleGeneral
    public void OnEnter()
    {
        Debug.Log("entre a idle");
        TimeTakenForRecoveryEnergyValue = MaxEnergy;
    }
    public void OnExit()
    {
        Debug.Log("sali de idle");
    }

    #region Funciones de fisica y eso de Idle
    private void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
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

    public float TimeTakenToStartRecoveringEnergy()
    {
        TimeTakenForRecoveryEnergyValue -= DecreaseSpeedOfEnergy * Time.deltaTime;
        return TimeTakenForRecoveryEnergyValue;
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
            _desired = _wayPointsArray[_myWayPointInt].transform.position - HunterTransform.position;
        }
    }
    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _obstacles);
    }
    void IfIseePlayerUpdate()
    {
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
    #endregion
    public void ThisStateUpdate()//"Update" de este script
    {
        if (TimeTakenToStartRecoveringEnergy() <= 0.1)
        {
            EnergyBarScript.EnergyRecoveryIdleStateFunction();//Funci�n que recarga la energ�a 
        }

        if (EnergyBarScript.currentEnergyValue >= 0.9f)
        {
            _fsm.ChangeState(States.Moving);
            Debug.Log("Se ha cambiado a estado Move del cazador");
        }

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

        HunterTransform.position += _velocity * Time.deltaTime;
        //para que mire al objetivo
        HunterTransform.right = _velocity;
    }
    #endregion

    #region Chase/Moving State General

    #region Funciones de F�sicas de Chase
    private void SeekOfHunter()
    {
        _desired.Normalize();
        _desired *= _maxSpeed;

        Vector3 steering = _desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForceMoving(steering);
    }

    private void AddForceMoving(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    public void ObstacleAvoidanceMoving()
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
         CollectingBoidsToHunt();
            if (MyPreyGO == null)
            {
                AlreadyHaveAPrey=false;
                _desired = standarHuntingPosition.transform.position - HunterTransform.position;
                Debug.Log("yendo al standar pos en hunting mode");
            }
            else
            {
                _desired = MyPreyGO.transform.position - HunterTransform.position;

            }
            //_desired = _arrayOfEnemies[_actualPrey].transform.position - HunterTransform.position;
        }
    }
    public void RangeToKill()
    {
        if (Physics.Raycast(HunterTransform.position, HunterTransform.right, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, -HunterTransform.right, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, HunterTransform.up, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, -HunterTransform.up, _rangeToKill, _enemies))
        {
            /*if (_arrayOfEnemies[_actualPrey].gameObject == null)
            {
                _actualPrey += 1;
                _arrayOfEnemies[_actualPrey].gameObject.SetActive(false);
            }
            else
            {
                _arrayOfEnemies[_actualPrey].gameObject.SetActive(false);
                _actualPrey += 1;
            }*/
            //MyPreyGO.gameObject.SetActive(false);
            Destroy(MyPreyGO.gameObject);
            Debug.Log("he matado");
            CollectingBoidsToHunt();

        }
    }
    #endregion

    public void OnEnterMoving()//Editado IA2
    {
        Debug.Log("Cazador entro a estado Move");
        CollectingBoidsToHunt();
        _rend.material.color = Color.red;
    }
    public void ThisStateUpdateMoving()
    {
        ObstacleAvoidanceMoving();
        SeekOfHunter();

        HunterTransform.position += _velocity * Time.deltaTime;
        //para que mire al objetivo
        HunterTransform.right = _velocity;

        RangeToKill();

        EnergyBarScript.HuntingModeEnergyConsumption();//Funci�n que consume la energ�a

        if (EnergyBarScript.currentEnergyValue <= 0.017f)
        {
            _fsm.ChangeState(States.Idle);
            Debug.Log("Se ha cambiado a estado Idle del cazador");
        }
    }
    public void OnExitMoving()
    {
        Debug.Log("Cazador salio del estado Move");
        _rend.material.color = Color.white;
    }
    #endregion

    public void SetUpFSM()
    {
        var idle = new EventStateIA2("Idle");
        idle.OnEnter = OnEnter;
        idle.OnExit = OnExit;
        idle.OnUpdate = ThisStateUpdate;

        var moving = new EventStateIA2("Moving");
        moving.OnEnter = () => OnEnterMoving();
        moving.OnExit = () => OnExitMoving();
        moving.OnUpdate = () => ThisStateUpdateMoving();

        _fsm = new FiniteStateMachineIA2<States>();
        _fsm.AddState(States.Idle, idle);
        _fsm.AddState(States.Moving, moving);
        _fsm.ChangeState(States.Idle);
    }
    private void Update()
    {
        _fsm.Update();
    }
}