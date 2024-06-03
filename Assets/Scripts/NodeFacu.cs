using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeFacu : MonoBehaviour
{
    public NodeFacu[] MyNeighboursScriptsArray;
    [SerializeField] float _radius = 5;
    public LayerMask _Player;
    public void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.transform.position, _radius);

    }
        public void SetMetaNodeforotherHuntersWhenPlayerVisualized()
        {
            if (Physics.Raycast(this.transform.position, this.transform.right, _radius, _Player) || Physics.Raycast(this.transform.position, -this.transform.right, _radius, _Player) || Physics.Raycast(this.transform.position, this.transform.up, _radius, _Player) || Physics.Raycast(this.transform.position, -this.transform.up, _radius, _Player))
            {
               //función que marcaAlPlayerEnEstenodopara los otrosHunters si es que un hunter lo ve al player, entonces este nodo es la meta();

                // O función para marcar nuevo punto de ruta si se pierde de vista al player 

            }
        }
        
        /* void ThisNodeIStheMetaForTheOtherHuntersBecausePlayerWasVisualized()
        {

        }
        */
}
