using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsSpawner : MonoBehaviour
{
    // Referencia al prefab que se va a instanciar
    public GameObject prefab;

    // Lista para almacenar las instancias
    public List<GameObject> instances = new List<GameObject>();

    // Cantidad de prefabs a instanciar inicialmente
    private int initialQuantity = 90;

    void Awake()
    {
        // Aseg�rate de que el prefab est� asignado
        if (prefab != null)
        {
            for (int i = 0; i < initialQuantity; i++)
            {
                InstantiateAndAddToList();

            }

            // Iniciar la corrutina para revisar y re-instanciar prefabs cada 10 segundos
            StartCoroutine(CheckAndRefillList());
        }
        else
        {
            Debug.LogError("Prefab no asignado en el Inspector.");
        }
    }

    void InstantiateAndAddToList()
    {
        // Instancia el prefab
        GameObject instance = Instantiate(prefab);

        // Hace que el objeto instanciado sea hijo del objeto en el que est� este script
        instance.transform.SetParent(transform);
        instance.gameObject.SetActive(true);

        // Agrega la instancia a la lista
        instances.Add(instance);
    }

    IEnumerator CheckAndRefillList()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);

            // Revisar la lista y rellenar los espacios nulos
            for (int i = 0; i < instances.Count; i++)
            {
                if (instances[i] == null)
                {
                    GameObject instance = Instantiate(prefab);
                    instance.transform.SetParent(transform);
                    instance.gameObject.SetActive(true);
                    instances[i] = instance;
                }
            }

            // En caso de que necesitemos a�adir m�s elementos para mantener el tama�o de la lista
            while (instances.Count < initialQuantity)
            {
                InstantiateAndAddToList();
            }
        }
    }


    /*// Referencia al prefab que se va a instanciar
    public GameObject prefab;

    // Cantidad de prefabs a instanciar
    private int quantity = 90;

    void Awake()
    {
        // Aseg�rate de que el prefab est� asignado
        if (prefab != null)
        {
            for (int i = 0; i < quantity; i++)
            {
                // Instancia el prefab
                GameObject instance = Instantiate(prefab);

                // Hace que el objeto instanciado sea hijo del objeto en el que est� este script
                instance.transform.SetParent(transform);

                // Opcional: puedes configurar la posici�n, rotaci�n, etc. de cada instancia aqu�
               // instance.transform.localPosition = Vector3.zero; // Por ejemplo, establecer la posici�n local a cero
            }
        }
        else
        {
            Debug.LogError("Prefab no asignado en el Inspector.");
        }
    }*/

}
