using System.Collections.Generic;

public interface IPathFinder
{
    List<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd);
}