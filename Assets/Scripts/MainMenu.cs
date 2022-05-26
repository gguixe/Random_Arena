using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public TextMeshProUGUI creditsText;
    public GameObject credits_background;

    public void Start()
    {
        FindObjectOfType<SoundManager>().Play("chickens"); //Sound/Music
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        creditsText.enabled = !creditsText.enabled;
        credits_background.SetActive(!credits_background.activeSelf);
    }
}
