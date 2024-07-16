using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileStateList")]

public class TileStateListSO : ScriptableObject
{
    public List<TileStateSO> list;
}
