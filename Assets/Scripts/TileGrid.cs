using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
	public TileRow[] Rows { get; private set; }
	public TileCell[] Cells { get; private set; }

	private void Awake()
	{
		Rows = GetComponentsInChildren<TileRow>();
		Cells = GetComponentsInChildren<TileCell>();
	}

	private void Start()
	{
		for (int y = 0; y < GetHeight(); y++)
		{
			for (int x = 0; x < GetWidth(); x++)
			{
				Rows[y].Cells[x].SetCoordinate(x, y);
			}
		}
	}

	public int GetSize() => Cells.Length;
	public int GetHeight() => Rows.Length;
	public int GetWidth() => GetSize() / GetHeight();
}
