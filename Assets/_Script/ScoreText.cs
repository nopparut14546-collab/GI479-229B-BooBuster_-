using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "GhostCaptured: " + scoreManager.currentScore;
    }
}
