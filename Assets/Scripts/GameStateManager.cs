using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameStateManager : MonoBehaviour
{
    public PlayerInputs inputs;

    [Header("Text's Performanc's Screen")]
    public Text txtScore;
    public Text txtWinLose;
    public Text txtScorePerformance;
    public Text txtMessage;

    [Header("Health Bar")]
    public HealthBar bar;

    [SerializeField]
    private int totalEnemies;
    private int score;
    private int healthPlayer = 0;

    private float time = 0.0f;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject performanceScreen;

    [Header("Audio")]
    public AudioSource aExplosion;

    [Header("Star")]
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    [Header("Black Stars")]
    public GameObject blackStar1;
    public GameObject blackStar2;
    public GameObject blackStar3;

    public enum GameState
    {
        IN_GAME,
        GAMEOVER_GAME,
        WIN_GAME,
        PAUSE_GAME
    }

    private GameState gameState = GameState.IN_GAME;

    [Header("Path")]
    public List<GameObject> points;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.IN_GAME)
        {
            time += Time.deltaTime;

            if (inputs.isPressStart())
            {
                gameState = GameState.PAUSE_GAME;
                pauseScreen.SetActive(true);
                inputs.PressStartFalse();
            }
        }

        if (gameState == GameState.GAMEOVER_GAME)
        {
            if (inputs.isPressStart())
                RetryLevel();
        }

        if (gameState == GameState.WIN_GAME)
        {
            if (inputs.isPressStart())
                RetryLevel();
        }

        if (gameState == GameState.PAUSE_GAME)
        {
            if (inputs.isPressStart())
            {
                gameState = GameState.IN_GAME;
                pauseScreen.SetActive(false);
                inputs.PressStartFalse();
            }
        }
    }

    /**
     * Asigna vida al jugado
     */
    public void SetPlayerLife(int life)
    {
        bar.UpdateBar();
        healthPlayer = life;
        if (life <= 0)
        {
            gameState = GameState.GAMEOVER_GAME;
            ShowPerformanceScreen();
        }
    }

    /**
     * Agrega un enemigo al escenario
     */
    public void AddEnemy()
    {
        totalEnemies++;
    }

    /**
     * Elimina un eneigo de la lista de enemigos y revisa si el jugador
     * gano el juego
     */
    public void EnemyDeath()
    {
        aExplosion.Play();
        totalEnemies--;
        if(totalEnemies == 0)
        {
            gameState = GameState.WIN_GAME;
            ShowPerformanceScreen();
        }

    }

    /**
     * Suma puntos al score del jugador
     */
    public void AddToScore(int points)
    {
        score += points;
        txtScore.text = "Score: " + score;
    }

    /**
     * Obtiene el estado activo del juego
     */
    public GameState GetGameStateActive()
    {
        return gameState;
    }

    /**
     * Sale de la pausa
     */
    public void PauseOut()
    {
        pauseScreen.SetActive(false);
        gameState = GameState.IN_GAME;
        inputs.PressStartFalse();
    }

    /**
     * Obtiene un punto del camino que siguen los enemigos 
     * en movimiento
     */
    public GameObject GetPointWay(int index)
    {
        return points[index];
    }

    public int GetTotalPoints()
    {
        return points.Count;
    }

    /**
     * Reitenta el nivel
     */
    public void RetryLevel()
    {
        gameState = GameState.IN_GAME;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * Muestra la pantalla de calificacion de desempeño y crea la calificacion
     * basado en el nivel de vida y tiempo que se tardo el jugador en eliminar a los
     * enemigos
     */
    public void ShowPerformanceScreen()
    {
        float _finalScore = 0;
        int _califTime = 0;
        int _califLife = 0;

        if (gameState == GameState.WIN_GAME)
        {
            txtWinLose.text = "You Win!";

            if (healthPlayer > 1 && healthPlayer <= 6)
            {
                _finalScore += score * 1.1f;
                _califLife = 1;
            }
            else if (healthPlayer > 6 && healthPlayer <= 10)
            {
                _finalScore += score * 2;
                _califLife = 2;
            }
            else if (healthPlayer > 10 && healthPlayer <= 14)
            {
                _finalScore += score * 3;
                _califLife = 3;
            }
            else if (healthPlayer == 15)
            {
                _finalScore += score * 4;
            }

            if (time <= 30)
            {
                _califTime = 3;
                _finalScore += score * 3;
            } else if(time > 30 && time <= 50)
            {
                _califTime = 2;
                _finalScore += score * 2;
            } else 
            {
                _califTime = 1;
                _finalScore += score * 1.1f;
            }

            int average = (_califTime + _califLife) / 2;

            if(average == 3)
            {
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
                blackStar1.SetActive(false);
                blackStar2.SetActive(false);
                blackStar3.SetActive(false);
                txtMessage.text = "A real hero does not use cape!";
            } else if(average == 2)
            {
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(false);
                blackStar1.SetActive(false);
                blackStar2.SetActive(false);
                blackStar3.SetActive(true);
                txtMessage.text = "It was a great victory but you can go back \nand obtain somethig better!";
            } else if (average == 1)
            {
                star1.SetActive(true);
                star2.SetActive(false);
                star3.SetActive(false);
                blackStar1.SetActive(false);
                blackStar2.SetActive(true);
                blackStar3.SetActive(true);
                txtMessage.text = "You can do it better! Yo must return to \ncombat and obtain another victory!";
            }
        }
        else if (gameState == GameState.GAMEOVER_GAME)
        {
            txtWinLose.text = "You Lose!";
            _finalScore = score;

            star1.SetActive(false);
            star2.SetActive(false);
            star3.SetActive(false);
            blackStar1.SetActive(true);
            blackStar2.SetActive(true);
            blackStar3.SetActive(true);
            txtMessage.text = "A hero never surrender. Try again and obtain the victory!";
        }

        txtScorePerformance.text = ((int)_finalScore).ToString();
        performanceScreen.SetActive(true);
    }
}
