using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueryHunter : MonoBehaviour
{
    public bool isBox;
    public float radius = 20f;
    public SpatialGrid targetGrid;
    public float width = 15f;
    public float height = 30f;
    public IEnumerable<GridEntity> selected = new List<GridEntity>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CollectingBoidsToHunt();
            Debug.Log("input llamado");
        }
    }

    void CollectingBoidsToHunt()
    {
        foreach (var item in Query())//Test
        {
            Destroy(item.gameObject);
            Debug.Log("hay uno");
        }
    }
    public IEnumerable<GridEntity> Query()
    {
       // if (isBox)
        //{
            var h = height * 0.5f;
            var w = width * 0.5f;
            //posicion inicial --> esquina superior izquierda de la "caja"
            //posici�n final --> esquina inferior derecha de la "caja"
            //como funcion para filtrar le damos una que siempre devuelve true, para que no filtre nada.
            return targetGrid.Query(
                transform.position + new Vector3(-w, -h, 0),
                transform.position + new Vector3(w, h, 0),
                x => true);
        //}
        /*else
        {
            //creo una "caja" con las dimensiones deseadas, y luego filtro segun distancia para formar el c�rculo
            return targetGrid.Query(
                transform.position + new Vector3(-radius, -radius, 0),
                transform.position + new Vector3(radius, radius, 0),
                x => {
                    var position2d = x - transform.position;
                    position2d.z = 0;
                    return position2d.sqrMagnitude < radius * radius;
                });
        }*/
    }
    void OnDrawGizmos()
    {
        /*if (targetGrid == null)
            return;

        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        if (isBox)
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
        else
        {
            Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
            Gizmos.DrawWireSphere(transform.position, radius);
        }

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

        }*/
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
       
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, radius);
    }


    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 20, 20), "HOLA");
    }
}
