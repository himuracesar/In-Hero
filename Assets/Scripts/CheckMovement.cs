using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Checa el movimiento de un objeto con respecto al escenario
 * 
 * @author Cesar Himura
 * @version 1.0
 */
public class CheckMovement : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerScene;
    [SerializeField]
    private LayerMask layerEnemy;
    [SerializeField]
    public Transform sensor;
    [SerializeField]
    public float sphereRadio = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Indica si el objeto puede moverse a la posicion.
     * La posicion es evaluada contra el escenario, si existe una colision no puede moverse, caso contrario cuando la colision
     * contra el escenario no exista
     */
    public bool CanMove(Vector3 position)
    {
        sensor.position = transform.position + position;
        //sensor.Translate(position.x, position.y, position.z);

        Collider[] colliders = Physics.OverlapSphere(sensor.position, sphereRadio, layerScene);

        if (colliders.Length > 0)
        {
            return false;
        }


        if (layerEnemy > 0)
        {
            colliders = Physics.OverlapSphere(sensor.position, sphereRadio, layerEnemy);

            if (colliders.Length > 1)
            {
                return false;
            }
        }


        return true;
    }
}
