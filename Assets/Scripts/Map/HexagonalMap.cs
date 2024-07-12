using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexagonalMap : IMap
{
    public Dictionary<Vector2, ICell> CellMap { get ; set; }
    public GameObject parentGameObject { get ; set; }
}