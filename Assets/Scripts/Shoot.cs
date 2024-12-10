using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [Header("Estad�sticas Principales del Proyectil")]
    [SerializeField] private float velocity;
    [SerializeField] private int damage;
    [SerializeField] private string target;
    [SerializeField] private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve el proyectile en una sola direcci�n
        transform.Translate(direction.normalized * velocity * Time.deltaTime);
        if (transform.position.x > 10 || transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Provoca da�o al jugador o a los enemigos
        if (collision.gameObject.CompareTag(target))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
}
