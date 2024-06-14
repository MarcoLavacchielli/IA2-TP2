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
       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            CollectingBoidsToHunt();
            Debug.Log("input llamado");
        }*/


        if (Query()!=null)
        {
            CollectingBoidsToHunt();
        }
    }

    public void CollectingBoidsToHunt()
    {
        var firstItem = Query().FirstOrDefault();
       
        
        //HunterScript._desired=firstItem.transform.position;
        /*
        foreach (var item in Query())//Test
        {
            Debug.Log("hay uno");
            Destroy(item.gameObject);
        }
        */
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

}
