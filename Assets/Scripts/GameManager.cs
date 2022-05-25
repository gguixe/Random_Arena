using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    //Pauses & Controls
    private bool gameIsPaused = true;
    private bool initTutorial = false;
    public GameObject controls_scheme;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void Start()
    {
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0) && initTutorial == false)
        //{
        //    gameIsPaused = !gameIsPaused;
        //    initTutorial = true;
        //    PauseGame();
        //}

        if (Input.GetKeyDown(KeyCode.Escape) || ((Input.GetKeyDown(KeyCode.Mouse0)) && gameIsPaused == true))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
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
}
