using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    //Pauses & Controls
    private bool gameIsPaused = true;
    public GameObject controls_scheme;

    //Wave
    private float maxWaves = 10;
    private float CurrentWave = 1;
    private float maxEnemies = 2;
    private float maxSimultaneousEnemies = 2;
    private float currentEnemiesAlive = 2;
    public static float killedEnemies = 0;
    private GameObject[] EnemiesAlive;

    //GUI
    public TextMeshProUGUI waveGUI;
    public TextMeshProUGUI EnemiesGUI;
    public GameObject RetryButton;


    //State
    public bool ActiveWave = true;
    public static bool setupNewWave = false;
    public GameObject Powerups;

    //Spawn
    private GameObject[] SpawnPoints;
    public GameObject SpawnEnemy;
    public GameObject PortalEffect;
    private int spawn;
    private bool blockSpawn = false;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        PauseGame();
    }

    private void Start()
    {
        SpawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        FindObjectOfType<SoundManager>().Play("music"); //Sound/Music
    }

    // Update is called once per frame
    void Update()
    {

        if(setupNewWave)
        {
            NewWave();
            PlayerHealth.PHealth = PlayerHealth.maxhealth; //HEAL
            setupNewWave = false;
            ActiveWave = true;
        }

        if (ActiveWave)
        {
            ScoreManager();
            SpawnManager();
        }


        if (Input.GetKeyDown(KeyCode.Escape) || ((Input.GetKeyDown(KeyCode.Mouse0)) && gameIsPaused == true))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }

        if(PlayerHealth.PHealth <= 0) //Gameover
        {
            waveGUI.text = "GAMEOVER";
            RetryButton.SetActive(true);
        }

    }
    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            controls_scheme.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            controls_scheme.SetActive(false);
        }
    }

    void ScoreManager()
    {
        EnemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
        currentEnemiesAlive = EnemiesAlive.Length;
        EnemiesGUI.text = "Chickens: " + (killedEnemies) + "/" + maxEnemies;

        waveGUI.text = "Wave " + CurrentWave;

        if (killedEnemies >= maxEnemies && (CurrentWave+1 != maxWaves))
        {
            waveGUI.text = "WAVE FINISHED! SELECT DEMONIC RANDOMIZER";
            ActiveWave = false;
            Powerups.SetActive(true);

        }
        else if(killedEnemies >= maxEnemies && (CurrentWave + 1 == maxWaves))
        {
            waveGUI.text = "VICTORY! THANKS FOR PLAYING!";
            ActiveWave = false;
            //killedEnemies = 0;
            //CurrentWave = 20; 
            //maxEnemies = 100; 
            //maxSimultaneousEnemies = 100; 
        }
    }

    void SpawnManager()
    {
        if(currentEnemiesAlive < maxSimultaneousEnemies) //Hay menos vivos que simultaneos permitidos
        {
            if(killedEnemies + currentEnemiesAlive < maxEnemies) //Matados+Current es menor que el maximo de wave
            {
                //SPAWN ENEMY HERE
                //Instantiate(SpawnEnemy, SpawnPoints[spawn].transform.position, Quaternion.identity);
                //if(blockSpawn == false) //Block Spawn until invoked
                //{
                //    Invoke("SpawnEnemies", 2.0f);
                //StartCoroutine(SpawnEnemies(spawn, 2.0f));
                //}
                //blockSpawn = true;
                //Instantiate(PortalEffect, SpawnPoints[spawn].transform.position, Quaternion.identity);

                //SPAWN ENEMY HERE
                spawn = Random.Range(0, SpawnPoints.Length);
                Instantiate(PortalEffect, SpawnPoints[spawn].transform.position, Quaternion.identity);
                Instantiate(SpawnEnemy, SpawnPoints[spawn].transform.position, Quaternion.identity);
            }
        }

    }

    //void SpawnEnemies()
    //{
    //    spawn = Random.Range(0, SpawnPoints.Length);
    //    Instantiate(SpawnEnemy, SpawnPoints[spawn].transform.position, Quaternion.identity);
    //    blockSpawn = false;
    //}

    //IEnumerator SpawnEnemies(int spawn, float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //    Instantiate(SpawnEnemy, SpawnPoints[spawn].transform.position, Quaternion.identity);
    //    // Now do your thing here
    //}

    void NewWave()
    {
        killedEnemies = 0;
        CurrentWave += 1; //Increase wave number
        maxEnemies += 2; //Increase maximum numbers to beat
        if (CurrentWave % 2 == 0)
            maxSimultaneousEnemies += 1; //Every odd round Simulateneous Enemies increase
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        PlayerHealth.PHealth = 100;
        RetryButton.SetActive(false);
        killedEnemies = 0;
        setupNewWave = false;
        PlayerHealth.PHealth = PlayerHealth.maxhealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
