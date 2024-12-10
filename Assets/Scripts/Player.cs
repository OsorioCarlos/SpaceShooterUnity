using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    // Variables publicas desde el Inspector de Unity
    [Header("Estadísticas Principales del Jugador")]
    [SerializeField] private int lifePoints;
    [SerializeField] private int collisionDamage;
    [SerializeField] private float velocity;
    [SerializeField] private float shootRate;
    [SerializeField] private float colddown;
    [SerializeField] private Transform[] projectileSpawnPoints;

    [Header("Referencias a GameObjects")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private GameObject centrarLayout;

    [Header("Referencias a UI")]
    [SerializeField] private TextMeshProUGUI lifesText;
    [SerializeField] private TextMeshProUGUI bombsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    // Variables privadas
    private int score = 0;
    private int bombs = 3;
    private int shootLevel = 0;
    private bool hasShield = false;
    private bool restartGame = false;

    // Temporizadores
    private float shootTimer = 0;
    private float bombTimer = 0;
    private float shieldTimer = 0;

    // Referencias a otras clases de C#
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        centrarLayout.gameObject.SetActive(false);
        shootTimer = shootRate;
        bombTimer = colddown;
        shieldGameObject.SetActive(false);
        UpdateUIText(lifesText, "HP: " + lifePoints);
        UpdateUIText(bombsText, "x: " + bombs);
        UpdateUIText(scoreText, "Puntaje: " + score);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LimitMovement();
        Shoot();
        ShootBomb();
        ShieldTimer();
        ResetGame();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Provoca daño al colisionar con un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(collisionDamage);
        }
    }

    private void UpdateUIText(TextMeshProUGUI UItext, string text)
    {
        UItext.text = text;
    }

    private void Movement()
    {
        // Movimiento del jugador en los ejes X e Y
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * velocity * Time.deltaTime);
    }

    private void LimitMovement()
    {
        // Limite del movimiento del jugador en pantalla
        float xClamped = Mathf.Clamp(transform.position.x, -8.5f, 7.5f);
        float yClamped = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = new Vector2(xClamped, yClamped);
    }

    private void Shoot()
    {
        // Disparo de proyectiles, limitado por un ratio de disparo
        shootTimer += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && shootTimer > shootRate)
        {
            for (int i = 0; i < projectileSpawnPoints.Length; i++)
            {
                if (CanIUseSpawnPoint(i))
                {
                    Instantiate(projectilePrefab, projectileSpawnPoints[i].position, Quaternion.identity);
                }
            }
            audioManager.PlaySFX(audioManager.shootSFX);
            shootTimer = 0;
        }
    }

    private void ShootBomb()
    {
        // Disparo de bombas, limitado por un ratio de disparo
        bombTimer += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.F) && bombTimer > colddown && bombs > 0)
        { 
            Instantiate(bombPrefab, projectileSpawnPoints[0].position, Quaternion.identity);
            bombTimer = 0;
            DecreaseBombs();
        }
    }

    private void ShieldTimer()
    {
        // El escudo se mantiene activo por 10 segundos
        if (hasShield)
        {
            shieldTimer += 1 * Time.deltaTime;
            if (shieldTimer >= 10f)
            {
                TurnOffShield();
            }
        }
    }

    private bool CanIUseSpawnPoint(int index)
    {
        // En cada nivel aumenta la cantidad de disparos que el jugador puede realizar
        if (shootLevel == 0 && index == 0)
        {
            return true;
        }
        
        if (shootLevel == 1 && (index == 1 || index == 2))
        {
            return true;
        }

        if (shootLevel > 1)
        {
            return true;
        }

        return false;
    }

    private void IncreaseShootRate()
    {
        if (shootLevel > 2)
        {
            shootRate = 0.2f;
        } else
        {
            shootRate = 0.3f;
        }
    }

    public void IncreaseShootLevel()
    {
        // Incrementa el nivel de disparo sin superar el nivel máximo
        if (shootLevel < 3)
        {
            shootLevel++;
            IncreaseShootRate();
        }
    }

    private void DecreaseShootLevel()
    {
        // Disminuye el nivel de disparo sin pasar el nivel mínimo
        if (shootLevel > 0)
        {
            shootLevel--;
            IncreaseShootRate();
        }
    }

    public void IncreaseBombs()
    {
        // Incrementa el numero de bombas y actualiza la UI
        bombs++;
        UpdateUIText(bombsText, "x: " + bombs);
    }

    private void DecreaseBombs()
    {
        // Disminuye el numero de bombas sin pasar el numero minimo y actualiza la UI
        if (bombs > 0)
        {
            bombs--;
            UpdateUIText(bombsText, "x: " + bombs);
        }
    }

    public void IncreaseLifePoints()
    {
        // Incrementa los puntos de vida de manera aleatoria y actualiza la UI
        lifePoints += Random.Range(5, 11);
        UpdateUIText(lifesText, "HP: " + lifePoints);
    }

    private void DecreaseLifePoints(int damage)
    {
        // Disminuye los puntos de vida y actualiza la UI
        lifePoints -= damage;
        string text = "HP: " + lifePoints;
        if (lifePoints <= 0)
        {
            text = "HP: 0";
        }
        UpdateUIText(lifesText, text);
    }

    public void TurnOnShield()
    {
        // Activa el escudo
        shieldTimer = 0;
        hasShield = true;
        shieldGameObject.SetActive(true);
    }

    private void TurnOffShield()
    {
        // Desactiva el escudo
        shieldTimer = 0;
        hasShield = false;
        shieldGameObject.SetActive(false);
    }

    private void ResetGame()
    {
        if (restartGame && Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TakeDamage(int damage)
    {
        // El daño se anula si el escudo esta activo
        if (!hasShield)
        {
            audioManager.PlaySFX(audioManager.hitSFX);
            DecreaseLifePoints(damage);
            DecreaseShootLevel();
        }

        // Si el jugador ya no tiene puntos de vida se destruye y genera una explosión
        if (lifePoints <= 0)
        {
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            ShowFinalText("Game Over");
            Destroy(gameObject);
        }
    }

    public void AddScore(int scoreValue)
    {
        // Aumenta el puntaje y actualiza la UI
        score += scoreValue;
        UpdateUIText(scoreText, "Puntaje: " + score);
    }

    public void ShowFinalText(string text)
    {
        restartGame = true;
        finalText.text = text;
        finalScoreText.text = "Puntaje Final: " + score;
        centrarLayout.gameObject.SetActive(true);
    }
}
