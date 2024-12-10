using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [Header("Estadísticas Principales del Powerup")]
    [SerializeField] private Vector2 delimitingPoint1;
    [SerializeField] private Vector2 delimitingPoint2;
    [SerializeField] private float lifeTime;
    [SerializeField] private float velocity;

    // Variables privadas
    private Vector2 direction;
    private float timer;

    // Referencias a otras clases de C#
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LifeTimeCounter();
    }

    private void Movement()
    {
        transform.Translate(direction * velocity * Time.deltaTime);
        if (transform.position.x <= delimitingPoint1.x || transform.position.x >= delimitingPoint2.x)
        {
            direction = new Vector2(-direction.x, Random.Range(direction.y-0.5f, direction.y + 0.5f)).normalized;
        }
        
        if (transform.position.y >= delimitingPoint1.y || transform.position.y <= delimitingPoint2.y)
        {
            direction = new Vector2(Random.Range(direction.x - 0.5f, direction.x + 0.5f), -direction.y).normalized;
        }
    }

    private void LifeTimeCounter()
    {
        timer += 1 * Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public virtual void UsePowerup(GameObject gameObjet)
    {
        // TO DO
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.powerupSFX);
            UsePowerup(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
