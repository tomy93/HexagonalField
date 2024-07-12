using System;
using System.Collections.Generic;
using System.Linq;

public class HexagonalPathFinder : IPathFinder
{
    public List<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd) 
    {
        var toSearch = new List<ICell>() { cellStart };
        var processed = new List<ICell>();

        while (toSearch.Any()) 
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
            {
                if (t.F < current.F || t.F == current.F && t.H < current.H)
                {
                    current = t;
                }
            } 

            processed.Add(current);
            toSearch.Remove(current);
            
            if (current == cellEnd) 
            {
                var currentPathTile = cellEnd;
                var path = new List<ICell>();
                var count = 100;
                while (currentPathTile != cellStart) 
                {
                    path.Add(currentPathTile);
                    currentPathTile.MarkAsPath();
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0)
                    {
                        throw new Exception();
                    }
                }
                
                // Debug.Log(path.Count);
                return path;
            }

            foreach (var neighbor in current.Neighbors.Where(t => 
                            t.Walkable && !processed.Contains(t))) 
            {
                var inSearch = toSearch.Contains(neighbor);
                var costToNeighbor = current.G + current.GetDistance(neighbor);

                if (!inSearch || costToNeighbor < neighbor.G) 
                {
                    neighbor.G = costToNeighbor;
                    neighbor.Connection = current;

                    if (!inSearch) 
                    {
                        neighbor.H = neighbor.GetDistance(cellEnd);
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
        return new List<ICell>();
    }
}