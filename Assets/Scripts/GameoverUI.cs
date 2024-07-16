using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour
{
    [SerializeField]
    private Button playAgainBtn;

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

        playAgainBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
            Hide();
        });

        Hide();
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false); 
    }
}
