using UnityEngine;

public interface ICoords 
{
    public Vector2 Pos { get; set; }
    public float GetDistance(ICoords other);
}
