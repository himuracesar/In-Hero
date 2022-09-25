using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;

    [SerializeField]
    private float rotationSpeed = 8.0f;
    [SerializeField]
    private float speed = 0.25f;
    private float angle;
    private float nextFire = 0.0f;
    private float collided = 0.0f;
    [SerializeField]
    private float fireRate = 0.2f;
    [SerializeField]
    private float timeCollided = 0.2f;

    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask pointLayer;

    [SerializeField]
    BulletEnemy bullet;

    [SerializeField]
    GameStateManager gameManager;
    [SerializeField]
    private CheckMovement check;

    private int health = 4;
    public int currentPoint = 0;
    public int destPoint;

    private Vector3 direction;
    private Vector3 directionMove;

    private bool isCollided;
    private bool seeingPlayer = false;
    private bool seeingPoint = false;
    public bool intelligence = true;
    public bool canFire = true;

    [Header("Materials")]
    public Material impactMat;
    public Material defaultMat;

    private List<BulletEnemy> bullets = new List<BulletEnemy>();

    GameStateManager.GameState gameState = GameStateManager.GameState.IN_GAME;

    public AudioSource aImpact;

    public enum EnemyState
    {
        FOLLOW,
        TOPLAYER,
        REINCORPORATE
    }

    public EnemyState enemyState = EnemyState.FOLLOW;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.AddEnemy();

        destPoint = currentPoint + 1;
        directionMove = gameManager.GetPointWay(destPoint).transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameStateActive() == GameStateManager.GameState.IN_GAME)
        {
            if (player)
                direction = player.transform.position - transform.position;

            Vector3 rotationAxis = Vector3.Cross(transform.forward, direction.normalized);
            rotationAxis = new Vector3(0.0f, rotationAxis.y, 0.0f);

            float dotProduct = Vector3.Dot(transform.forward, direction.normalized);

            angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            transform.Rotate(rotationAxis, angle * Time.deltaTime * rotationSpeed);

            if(canFire)
                Fire();

            if (isCollided && collided < timeCollided)
            {
                collided += Time.deltaTime;
            }
            else
            {
                collided = 0.0f;
                isCollided = false;
                //gameObject.GetComponent<Renderer>().material = defaultMat;
                transform.GetChild(1).gameObject.GetComponent<Renderer>().material = defaultMat;
            }

            if (gameState != gameManager.GetGameStateActive())
            {
                gameState = GameStateManager.GameState.IN_GAME;
                Pause(true);
            }

            if (intelligence)
            {
                if (player)
                {
                    Ray ray = new Ray(transform.position, transform.forward);

                    RaycastHit hit;

                    seeingPlayer = Physics.Raycast(ray, out hit, 2000, playerLayer);

                    if (hit.collider)
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            Debug.DrawLine(ray.origin, hit.point, Color.green);
                            enemyState = EnemyState.TOPLAYER;
                            directionMove = player.transform.position - transform.position;
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, hit.point, Color.red);
                        }
                    }
                }


                if (enemyState == EnemyState.FOLLOW)
                {
                    float d = Vector3.Distance(transform.position, gameManager.GetPointWay(destPoint).transform.position);
                    if (d < 2.0f)
                    {
                        currentPoint = (currentPoint < gameManager.GetTotalPoints()) ? currentPoint + 1 : 0;
                        destPoint = ((currentPoint + 1) < gameManager.GetTotalPoints()) ? currentPoint + 1 : 0;

                        directionMove = gameManager.GetPointWay(destPoint).transform.position - transform.position;
                    }
                }

                if (check.CanMove(new Vector3(directionMove.x * speed * Time.deltaTime * 2.0f, 0.0f, directionMove.z * speed * Time.deltaTime * 2.0f)))
                {
                    transform.position = new Vector3(transform.position.x + directionMove.x * speed * Time.deltaTime,
                                                 transform.position.y,
                                                 transform.position.z + directionMove.z * speed * Time.deltaTime);
                }
                else //if (enemyState == EnemyState.TOPLAYER)
                {
                    int _startVal = 0;

                    if (enemyState == EnemyState.FOLLOW)
                    {
                        currentPoint = (currentPoint < gameManager.GetTotalPoints()) ? currentPoint + 2 : 0;
                        _startVal = currentPoint;
                    }

                    for (int iPoint = _startVal; iPoint < gameManager.GetTotalPoints(); iPoint++)
                    {
                        directionMove = gameManager.GetPointWay(iPoint).transform.position - transform.position;

                        Ray ray = new Ray(transform.position, directionMove);

                        RaycastHit hit;

                        seeingPoint = Physics.Raycast(ray, out hit, 2000); // pointLayer);

                        Debug.DrawLine(ray.origin, hit.point, Color.red);

                        if (hit.collider.gameObject.tag == "Way")
                        {
                            enemyState = EnemyState.FOLLOW;
                            destPoint = iPoint;
                            currentPoint = iPoint - 1;
                            seeingPoint = false;

                            Debug.DrawLine(ray.origin, hit.point, Color.green);

                            break;
                        }
                    }
                }
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

    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            BulletEnemy b = Instantiate(bullet, transform.position, Quaternion.identity);
            b.SetDirection(transform.forward);

            bullets.Add(b);
        }
    }

    public void SetCollided()
    {
        aImpact.Play();

        health--;
        gameManager.AddToScore(1);

        isCollided = true;

        transform.GetChild(1).gameObject.GetComponent<Renderer>().material = impactMat;

        if (health == 0)
        {
            for (int iBullet = 0; iBullet < bullets.Count; iBullet++)
            {
                if(bullets[iBullet])
                    Destroy(bullets[iBullet].gameObject);
            }
            bullets.Clear();

            gameManager.EnemyDeath();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                player.Damage(4);
                gameManager.EnemyDeath();
                gameManager.AddToScore(4);
                Destroy(this.gameObject);
                break;
        }
    }

    public void Pause(bool pause)
    {
        foreach (BulletEnemy b in bullets)
        {
            b.SetCanMove(pause);
        }
    }

    public void Reincorporate()
    {

    }
}
