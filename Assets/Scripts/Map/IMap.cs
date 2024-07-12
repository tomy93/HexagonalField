using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
    public Dictionary<Vector2, ICell> CellMap {get; set;}
}