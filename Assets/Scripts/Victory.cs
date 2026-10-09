using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{

    [SerializeField]
    private TMP_Text leftScoreText;

    [SerializeField]
    private TMP_Text rightScoreText;

    [SerializeField]
    private GameObject leftWinText;

    [SerializeField]
    private GameObject rightWinText;

    public Button restartButton;


    private void Update()
    {
        if (leftScoreText.text == "5")
        {
            Debug.Log("Left Player Wins!");
            leftWinText.SetActive(true);
            restartButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else if (rightScoreText.text == "5")
        {
            Debug.Log("Right Player Wins!");
            rightWinText.SetActive(true);
            restartButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

    }
}
