using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    // Booleano que se invertirá
    private bool toggle = false;

    // Posiciones predefinidas
    public Vector3 positionA = new Vector3(10f, 0f, 0f);
    public Vector3 positionB = new Vector3(-10f, 0f, 0f);

    // Función que invierte el valor del booleano y cambia la posición
    public void ToggleAndChangePosition()
    {
        // Invertir el valor del booleano
        toggle = !toggle;
        this.gameObject.SetActive(false);
        // Cambiar la posición según el valor del booleano
        if (toggle)
        {
            transform.position = positionA;
        }
        else
        {
            transform.position = positionB;
        }
        this.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy(gameObject);
        ToggleAndChangePosition();
    }
}

