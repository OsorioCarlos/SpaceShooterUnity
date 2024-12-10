using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [Header("Estadísticas Principales del Jugador")]
    [SerializeField] private int lifePoints;
    [SerializeField] private int collisionDamage;
    [SerializeField] private int scoreValue;
    [SerializeField] private float velocity;
    [SerializeField] private float shootRate;
    [SerializeField] private Transform[] projectileSpawnPoints;

    [Header("Referencias a GameObjects")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject[] powerupPrefabs;

    // Variables privadas
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Provoca daño al colisionar con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(collisionDamage);
        }
    }

    private void Movement()
    {
        // Movimiento del enemigo en el eje X
        transform.Translate(Vector2.left * velocity * Time.deltaTime);
        if (transform.position.x < -12)
        {
            Destroy(gameObject);
        }
    }

    private void DecreaseLifePoints(int damage)
    {
        // Disminuye los puntos de vida
        lifePoints -= damage;
    }

    private void Die()
    {
        // Si el enemigo ya no tiene puntos de vida se destruye y genera una explosión
        if (lifePoints <= 0)
        {
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            SpawnPowerup();
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }

    private void SpawnPowerup()
    {
        // Genera un powerup para el jugador
        if (Random.Range(0, 1f) <= 0.90f)
        {
            int randomPowerup = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomPowerup], gameObject.transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        // El daño se anula si el escudo esta activo
        audioManager.PlaySFX(audioManager.hitSFX);
        DecreaseLifePoints(damage);
        Die();
    }

    IEnumerator SpawnProjectile()
    {
        // El enemigo dispara hasta que sea destruido
        while (true)
        {
            foreach (Transform projectileSpawnPoint in projectileSpawnPoints)
            {
                Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(shootRate);
        }
    }
}
