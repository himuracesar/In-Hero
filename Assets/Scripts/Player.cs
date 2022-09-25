using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputs inputs;

    [SerializeField]
    private float speed = 180.0f;
    [SerializeField]
    private float rotationSpeed = 7.0f;
    [SerializeField]
    private float fireRate = 0.1f;
    private float angle = 0.0f;
    private float nextFire = 0.0f;
    [SerializeField]
    private float distAnchor;
    [SerializeField]
    private float maxDistAnchor = 80.0f;

    [SerializeField]
    Transform shooter;
    [SerializeField]
    private Transform anchor;
    [SerializeField]
    private Bullet bullet;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private CheckMovement check;
    [SerializeField]
    private GameStateManager gameManager;

    private Vector3 antPosition;

    public int maxHealth = 15;
    public int health;

    [SerializeField]
    private AudioSource aShot;

    private List<Bullet> bullets = new List<Bullet>();

    GameStateManager.GameState gameState = GameStateManager.GameState.IN_GAME;

    [Header("Materials")]
    public Material impactMat;
    public Material defaultMat;

    [Header("Collision")]
    private bool isCollided = false;
    public float timeCollided = 0.2f;
    public float collided = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        antPosition = transform.position;
        gameManager.SetPlayerLife(health);
        distAnchor = Vector3.Distance(transform.position, anchor.position);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameStateActive() == GameStateManager.GameState.IN_GAME)
        {
            if (gameState != gameManager.GetGameStateActive())
            {
                gameState = GameStateManager.GameState.IN_GAME;
                Pause(true);
            }

            if (isCollided && collided < timeCollided)
            {
                collided += Time.deltaTime;
            }
            else
            {
                collided = 0.0f;
                isCollided = false;
                //gameObject.GetComponent<Renderer>().material = defaultMat;
                transform.GetChild(2).gameObject.GetComponent<Renderer>().material = defaultMat;
            }

            if (inputs.IsMoving())
            {
                float _delta = Time.deltaTime * speed;


                if (check.CanMove(new Vector3(inputs.GetDelta().x * _delta * 2.0f, 0.0f, inputs.GetDelta().y * _delta * 2.0f)))
                {
                    antPosition = transform.position;
                    //transform.Translate(inputs.GetDelta().x * _delta, 0.0f, inputs.GetDelta().y * _delta);
                    transform.position += new Vector3(inputs.GetDelta().x * _delta, 0.0f, inputs.GetDelta().y * _delta);

                    distAnchor = Vector3.Distance(transform.position, anchor.position);

                    if (distAnchor > maxDistAnchor)
                    {
                        Vector3 d = transform.position - antPosition;
                        anchor.transform.position = anchor.transform.position + d;
                        camera.transform.position = camera.transform.position + d;
                    }
                }
            }

            if (inputs.IsRotating())
            {
                Vector3 deltaRotate = new Vector3(inputs.GetDeltaRotate().x, 0.0f, inputs.GetDeltaRotate().y);

                Vector3 rotationAxis = Vector3.Cross(transform.forward, deltaRotate.normalized);
                rotationAxis = new Vector3(0.0f, rotationAxis.y, 0.0f);

                float dotProduct = Vector3.Dot(transform.forward, deltaRotate.normalized);

                angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

                transform.Rotate(rotationAxis, angle * Time.deltaTime * rotationSpeed);
            }

            if (inputs.IsFiring() && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Bullet b = Instantiate(bullet, shooter.position, Quaternion.identity);
                b.SetDirection(shooter.transform.forward);
                aShot.Play();

                bullets.Add(b);
            }
        }
        else
        {
            if (gameManager.GetGameStateActive() == GameStateManager.GameState.PAUSE_GAME)
            {
                if (gameState != gameManager.GetGameStateActive())
                {
                    gameState = GameStateManager.GameState.PAUSE_GAME;
                    Pause(false);
                }
            }
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        isCollided = true;
        transform.GetChild(2).gameObject.GetComponent<Renderer>().material = impactMat;
        //gameManager.SetPlayerLife(health);
        if (health <= 0)
        {
            health = 0;
            Destroy(this.gameObject);
        }
        gameManager.SetPlayerLife(health);
    }

    public void Pause(bool pause)
    {
        foreach (Bullet b in bullets)
        {
            b.SetCanMove(pause);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
