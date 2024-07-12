using System.Collections.Generic;
public interface ICell
{
    ICoords Coords { get; set; }
    List<ICell> Neighbors { get; set; }
    ICell Connection { get; set; }

    bool Walkable { get; set; }
    float G { get; set; }
    float H { get; set; }
    float F { get; }

    float GetDistance(ICell other);
    void MarkAsPath();
    void CacheNeighbors(IMap map);

    T CellGameObject<T>();
}