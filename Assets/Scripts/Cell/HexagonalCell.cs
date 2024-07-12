using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexagonalCell : MonoBehaviour, ICell
{
    [SerializeField] public SpriteRenderer _spriteRenderer;
    // [SerializeField] public TextMesh _textCoords;

    private Action<HexagonalCell> _onClickCallback;
    private Sprite _pathSprite;
    private Sprite _defaultSprite;

    public ICoords Coords { get; set; }
    public bool Walkable { get; set; }
    public List<ICell> Neighbors { get; set; }
    public ICell Connection { get; set; }
    public float G { get; set; }
    public float H { get; set; }
    public float F => G + H;

    public float GetDistance(ICell other) => Coords.GetDistance(other.Coords);

    public T CellGameObject<T>()
    {
        if (typeof(T) == typeof(HexagonalCell))
        {
            return (T)(object)this;
        }
        else
        {
            throw new InvalidOperationException(
                $"Type {typeof(T)} is not supported by CellGameObject.");
        }
    }

    public void CacheNeighbors(IMap map)
    {
        Neighbors = map.CellMap.Where(t => 
            Coords.GetDistance(t.Value.Coords) == 1)
            .Select(t => t.Value).ToList();
    }

    public void Setup(int q, int r, 
        CellLayout cellDefinition, Action<HexagonalCell> onClickCallback)
    {
        Coords = new HexCoords(q, r);
        transform.position = Coords.Pos;
        Walkable = cellDefinition.isWalkable;

        _spriteRenderer.sprite = cellDefinition.defaultSprite;
        _defaultSprite = cellDefinition.defaultSprite;
        _pathSprite = cellDefinition.pathSprite;
        _onClickCallback = onClickCallback;
    }

    // TODO: for better performance, a cursor click positioning mapping
    // system can be implemented to get the position in coords of the mouse's
    // click... instead of having a collider on each hexagon
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        _onClickCallback?.Invoke(this);
    }

    public void ResetSprite()
    {
        _spriteRenderer.sprite = _defaultSprite;
    }

    public void MarkAsPath()
    {
        _spriteRenderer.sprite = _pathSprite;
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
