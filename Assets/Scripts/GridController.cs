using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private List<TileStateSO> tileStateLists;
    private Tile[,] board;
    private int width;
    private int height;

    private void Awake()
    {
        tileStateLists = Resources.Load<TileStateListSO>("TileStateListSO").list;

        RectTransform[] cols = transform.GetComponentsInChildren<RectTransform>();
        height = cols.Length;
        width = cols[0].GetComponentsInChildren<RectTransform>().Length;

        board = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                    
            }
        }

    }


}
