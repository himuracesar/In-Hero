using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 20.0f;
    [SerializeField]
    private float lifeTime = 1.5f;
    private float time = 0.0f;

    private Vector3 direction;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position += direction * Time.deltaTime * speed;
            time += Time.deltaTime;

            if(time >= lifeTime)
                Destroy(gameObject);
        }
    }

    /**
     * Asigna la direccion de la bala
     * @param direction Vector de direccion
     */
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().SetCollided();
                Destroy(this.gameObject);
            break;

            case "Scene":
                Destroy(this.gameObject);
            break;
        }
    }

    /**
     * Se le indica a la bala si puede moverse o no
     * @param move Indicador de movimiento
     */
    public void SetCanMove(bool move)
    {
        canMove = move;
    }
}
