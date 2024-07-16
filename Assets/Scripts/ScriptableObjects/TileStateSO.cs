using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileState")]
public class TileStateSO : ScriptableObject
{
    public int number;
    public Color color;
    public Color textColor;
}
