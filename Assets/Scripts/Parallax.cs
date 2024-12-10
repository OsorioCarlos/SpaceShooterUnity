using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [SerializeField] private float velocity;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float widthImage;

    // Variables privadas
    private Vector2 startPosition;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        /* Calcula la posición del "Background 00" en base al tamaño de la imagen
         y reposiciona la imagen para dar el efecto de movimiento infinito */
        float mod = (velocity * Time.time) % widthImage;
        transform.position = startPosition + mod * direction;
    }
}
