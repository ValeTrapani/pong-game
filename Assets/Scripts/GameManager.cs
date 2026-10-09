using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Button startButton;
    private void Awake()
    {
        Time.timeScale = 0f; // Pausa il gioco all'avvio
    }

    public void StartGame()
    {
        Time.timeScale = 1f; // Riprende il gioco
        startButton.gameObject.SetActive(false); // Nasconde il pulsante di avvio
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Riprende il gioco
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Ricarica la scena corrente
    }
}
