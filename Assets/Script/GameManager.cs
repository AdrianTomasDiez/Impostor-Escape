using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] prefabs; // Una lista de prefabs que se pueden generar
    public Transform playerPosition; // El transform del jugador
    public float prefabLength; // La longitud de cada prefab
    public int numPrefabs; // El número de prefabs que se deben generar
    private PlayerControls player;
    private bool paused;
    public GameObject pause;
    public GameObject hud;
    public List<GameObject> activePrefabs = new List<GameObject>(); // Los prefabs que están activos en la escena
    private Vector2 spawnPosition; // La posición donde se generará el siguiente prefab

    // Start is called before the first frame update
    void Start()
    {
        // Colocar el punto de partida en la posición adecuada
        spawnPosition = new Vector2(100, 0);

        // Generar los primeros prefabs
        for (int i = 0; i < numPrefabs; i++)
        {
            GeneratePrefab();
        }
        player = FindObjectOfType<PlayerControls>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Comprobar si se necesita generar un nuevo prefab
        if ( !player.isDead && player.transform.position.x > activePrefabs[0].transform.position.x + prefabLength/2f)
        {
            DestroyPrefab();
            GeneratePrefab();
        }
    }

    public void GeneratePrefab()
    {
        // Seleccionar un prefab aleatorio de la lista
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject prefab = Instantiate(prefabs[randomIndex]);

        // Colocar el prefab en la posición adecuada
        prefab.transform.position = spawnPosition;

        // Añadir el prefab a la lista de prefabs activos
        activePrefabs.Add(prefab);

        // Actualizar la posición donde se generará el siguiente prefab
        spawnPosition.x += prefabLength;
    }
    public void ResetLevel()
    {
        // Eliminar todos los prefabs activos
        foreach (GameObject prefab in activePrefabs)
        {
            Destroy(prefab);
        }

        activePrefabs.Clear();

        if (activePrefabs.Count > 0)
        {
            spawnPosition = activePrefabs[activePrefabs.Count - 1].transform.position + new Vector3(prefabLength, 0, 0);
        }
        else
        {
            spawnPosition = new Vector2(100, 0);
        }

        // Generar todos los prefabs desde el principio
        for (int i = 0; i < numPrefabs; i++)
        {
            GeneratePrefab();
        }
    }

    public void DestroyPrefab()
    {
        // Eliminar el primer prefab de la lista de prefabs activos
        GameObject oldPrefab = activePrefabs[0];
        activePrefabs.RemoveAt(0);
        Destroy(oldPrefab);
    }



    public void Pause()
    {
        if (paused == false)
        {
            pause.SetActive(true);
            hud.SetActive(false);
            paused = true;
            Time.timeScale = 0;
        }
        else if (paused == true)
        {
            pause.SetActive(false);
            hud.SetActive(true);
            paused = false;
            Time.timeScale = 1;
        }
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
    public void LoadGameplayScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Gameplay");
    }
}
