using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Facundo Sebastian Tisera
public class HunterNPC : MonoBehaviour//Player
{
    MyHunterFiniteStateMachine _fsm; //Con esto lo estamos guardando en nuestra variable
    public Transform[] _wayPointsArray; //Patrol array
    public LayerMask _obstacles;
    public LayerMask _enemies;
    public GameObject[] _arrayOfEnemies;
    public GameObject Player;
    [SerializeField, Range(1, 10)] public float viewRadius;
    [SerializeField, Range(1, 360)] public float viewAngle;
    public EnergyBar EnergyBarScript;
    public Transform HunterTransform;
    public float newradius=3;
    void Start()
    {
        HunterTransform = GetComponent<Transform>();
        _fsm = new MyHunterFiniteStateMachine(); //Esto esta creando una nueva FSM;
        _fsm.AddState(PlayerStates.Idle, new HunterPatrolState(this));
        _fsm.AddState(PlayerStates.Move, new Hunter(this));
        //_fsm.AddState(PlayerStates.Dying, new DyingState(this));
        //_fsm.ChangeState(PlayerStates.Idle);
        _fsm.ChangeState(PlayerStates.Idle);

    }
    public void Update()
    {
        _fsm.Update();
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(HunterTransform.position, newradius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(HunterTransform.position + HunterTransform.up * 0.5f, HunterTransform.position + HunterTransform.up * 0.5f + HunterTransform.right * newradius);
        Gizmos.DrawLine(HunterTransform.position - HunterTransform.up * 0.5f, HunterTransform.position - HunterTransform.up * 0.5f + HunterTransform.right * newradius);

        //GIZMOS DE FOV AGENT
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(HunterTransform.position, viewRadius);

        Vector3 DirA = GetAngleFromDir(viewAngle / 2 + HunterTransform.eulerAngles.x);
        Vector3 DirB = GetAngleFromDir(-viewAngle / 2 + HunterTransform.eulerAngles.x);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(HunterTransform.position, HunterTransform.position + DirA.normalized * viewRadius);
        Gizmos.DrawLine(HunterTransform.position, HunterTransform.position + DirB.normalized * viewRadius);
    }
    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}

    public enum PlayerStates
    {
        Idle,
        Move,
        
    }