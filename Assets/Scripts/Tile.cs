using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TileStateSO tileState;

    private TileCell cell;

    private TextMeshProUGUI text;
    private Image image;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void SetState(TileStateSO tileState)
    {
        this.tileState = tileState;
        text.text = tileState.number.ToString();
        text.color = tileState.textColor;
        image.color = tileState.color;
    }

    public TileStateSO GetState() => tileState;

    public void SetTileCell(TileCell cell)
    {
        this.cell = cell;
        
    }

    public TileCell GetTileCell() => cell;
}
