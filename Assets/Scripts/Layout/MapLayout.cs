using System;
using UnityEngine;

[Serializable]
public class MapLayout
{
    [Header("Grid")]
    [SerializeField] private int _gridWidth = 20;
    [SerializeField] private int _gridDepth = 18;

    [Header("Prefabs")]
    [SerializeField] private HexagonalCell _cellPrefab;
    [SerializeField] private GameObject _startBeaconPrefab;
    [SerializeField] private GameObject _endBeaconPrefab;

    [Header("CellLayout")]
    [SerializeField] private CellLayout[] _cellLayout;

    public int GridWidth { get => _gridWidth;}
    public int GridDepth { get => _gridDepth;}
    public HexagonalCell CellPrefab { get => _cellPrefab;}
    public GameObject StartBeaconPrefab { get => _startBeaconPrefab;}
    public GameObject EndBeaconPrefab { get => _endBeaconPrefab;}
    public CellLayout[] CellLayouts { get => _cellLayout;}
}