using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnGameOver;

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private TileBoard board;

    public bool IsGameOver { get; private set; }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private void Awake()
    {
        Instance = this;
        IsGameOver = false;
    }


    private void Update()
    {
        if (IsGameOver)
        {
            return;
        }

        if (!board.IsBusy)
        {
            if (board.IsGameOverCheck())
            {
                IsGameOver = true;
                Debug.Log("game over");
                OnGameOver?.Invoke(this, EventArgs.Empty);
            }

            board.HandleInput();
        }

        //if (Input.GetKeyDown(KeyCode.E)) {
        //    IsGameOver = true;
        //    Debug.Log("game over");
        //    OnGameOver?.Invoke(this, EventArgs.Empty);
        //}

    }

    public void RestartGame()
    {
        board.RestartGame(); 
        IsGameOver = false;
    }
}
