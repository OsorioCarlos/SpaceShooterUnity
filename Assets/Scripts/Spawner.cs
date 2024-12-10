using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        RestartGame();
    }

    private void RestartGame()
    {
        if (FindObjectOfType<Player>() == null && Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator SpawnEnemy()
    {
        // Controla el numero de niveles
        for (int i = 0; i < 5; i++)
        {
            // Controla el numero de oleadas
            for (int j = 0; j < 3; j++)
            {
                // Controla en numero de enemigos
                for (int k = 0; k < 10; k++)
                {
                    Vector2 randomPosition = new Vector2(transform.position.x, Random.Range(-4.5f, 4.5f));
                    int randomEnemy = Random.Range(0, enemyPrefabs.Length);
                    Instantiate(enemyPrefabs[randomEnemy], randomPosition, Quaternion.identity);
                    yield return new WaitForSeconds(spawnRate);
                }
                yield return new WaitForSeconds(3.0f);
            }
            yield return new WaitForSeconds(5.0f);
        }

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ShowFinalText("Nivel Completado");
        }

    }
}
