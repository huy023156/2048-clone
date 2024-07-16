using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        if (!board.IsBusy )
        {
            if (board.IsGameOverCheck())
                Debug.Log("Game Over");
            else
            {
                board.HandleInput();
            }
        } 
    }

    

    public void SetGameOver(bool isGameOver) { 
        IsGameOver = isGameOver;
    }
}
