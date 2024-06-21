using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

// Lucas Peck

public class Boid : SteeringAgent
{
    [SerializeField] Transform _seekTarget /*Objetivo*/, _fleeTarget /*Esquiva al Hunter*/;
    [SerializeField, Range(0f, 2.5f)] float _alignmentWeight = 1;
    [SerializeField, Range(0f, 2.5f)] float _separationWeight = 1;
    [SerializeField, Range(0f, 2.5f)] float _cohesionWeight = 1;

    [Header("Variables Query")]
    public bool isBox;
    public float radius = 20f;
    public SpatialGrid targetGrid;
    public float width = 15f;
    public float height = 30f;
    public IEnumerable<GridEntity> selected = new List<GridEntity>();
    public GridEntity[] GridArray;
    public List<GridEntity> GridList;

    public GameObject seekTargetGO, fleeTargetGO;

    private void Awake()
    {
        targetGrid = GetComponentInParent<SpatialGrid>();
    }
    private void OnEnable()
    {
        targetGrid = GetComponentInParent<SpatialGrid>();
        seekTargetGO = GameObject.FindGameObjectWithTag("SeekTarget");
        fleeTargetGO = GameObject.FindGameObjectWithTag("Hunter");
        _seekTarget = seekTargetGO.transform;
        _fleeTarget = fleeTargetGO.transform;
        if (targetGrid == null)
            targetGrid = GetComponentInParent<SpatialGrid>();
    }
    private void Start()
    {
        targetGrid = GetComponentInParent<SpatialGrid>();

        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        var dir = new Vector3(x, 0, z);

        _velocity = dir.normalized * _maxSpeed;

        GameManager.instance.allAgents.Add(this); // Agrega la lista del Game Manager
    }
    public void SeekForNullReferences()
    {
        if (seekTargetGO == null || fleeTargetGO == null || targetGrid == null || _seekTarget == null || _fleeTarget == null)
        {
            targetGrid = GetComponentInParent<SpatialGrid>();
            seekTargetGO = GameObject.FindGameObjectWithTag("SeekTarget");
            fleeTargetGO = GameObject.FindGameObjectWithTag("Hunter");
            _seekTarget = seekTargetGO.transform;
            _fleeTarget = fleeTargetGO.transform;

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
    public List<SteeringAgent> BoidsNearMe()
    {
        // Crea una lista para almacenar los SteeringAgents encontrados
        List<SteeringAgent> steeringAgents = new List<SteeringAgent>();

        // Itera sobre cada GridEntity en el resultado de Query
        foreach (GridEntity entity in Query())
        {
            // Intenta obtener el componente SteeringAgent del GridEntity
            SteeringAgent agent = entity.GetComponent<SteeringAgent>();

            // Si el SteeringAgent existe, agrégalo a la lista
            if (agent != null)
            {
                steeringAgents.Add(agent);
            }
        }

        // Retorna la lista de SteeringAgents encontrados
        return steeringAgents;



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
        SeekForNullReferences();
    }

    private void Flocking()//REEMPLAZAR POR LA BUSQUEDA CON QUERY 
    {
        //UNA LISTA SIEMPRE ME VA A DAR NULOS, SIN ES UN ARRAY NO SE PUEDE EXPANDIR, 


        //var boids = GameManager.instance.allAgents;
        //GridArray = Query().ToArray();
        GridList = Query().ToList();
        AddForce(Alignment(GridList) * _alignmentWeight);
        AddForce(Separation(GridList) * _separationWeight); // Se aplica un radio más chico al actual
        AddForce(Cohesion(GridList) * _cohesionWeight);
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