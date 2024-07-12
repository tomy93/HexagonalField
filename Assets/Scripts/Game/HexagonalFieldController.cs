using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexagonalFieldController : MonoBehaviour
{
    [SerializeField] private MapLayout _defaultMapLayout;
    [SerializeField] private InfoPanelDisplay _infoPanelDisplay;

    [Header("UI")]
    [SerializeField] private Button _btnGenerateField;
    [SerializeField] private Button _btnSelectStartCell;
    [SerializeField] private Button _btnSelectEndCell;

    private HexagonalMap _currentHexMap;
    private GameObject _cursor;
    private GameObject _startBeacon;
    private GameObject _endBeacon;

    private HexagonalCell _currentStartingCell;
    private HexagonalCell _currentEndingCell;

    private HexagonalPathFinder _hexagonalPathFinder = new HexagonalPathFinder();

    private enum EnumCellSelectionPhase { None, Start, End }
    private EnumCellSelectionPhase _cellSelectionPhase;

    void OnEnable()
    {
        _btnGenerateField.onClick.AddListener(GenerateField);

        _btnSelectStartCell.onClick.AddListener( 
            () => SwitchCellSelectionPhase(EnumCellSelectionPhase.Start));

        _btnSelectEndCell.onClick.AddListener(
            () => SwitchCellSelectionPhase(EnumCellSelectionPhase.End));
    }
    
    void OnDisable()
    {
        _btnGenerateField.onClick.RemoveAllListeners();
        _btnSelectStartCell.onClick.RemoveAllListeners();
        _btnSelectEndCell.onClick.RemoveAllListeners();
    }

    void Start ()
    {
       GenerateField();
    }

    private void GenerateField()
    {
        CleanUpScene();

        _currentHexMap = 
            HexagonalFieldGenerator.GenerateHexagonalField(_defaultMapLayout, OnCellClick);

        foreach (var tile in _currentHexMap.CellMap.Values) 
        {
            tile.CacheNeighbors(_currentHexMap);
        } 
    }

    private void CleanUpScene()
    {
        SwitchCellSelectionPhase(EnumCellSelectionPhase.None);

        if(_cursor != null)
        {
            Destroy(_cursor);
        }

        if(_startBeacon != null)
        {
            Destroy(_startBeacon);
        }

        if(_endBeacon != null)
        {
            Destroy(_endBeacon);
        }

        if(_currentHexMap != null)
        {
            HexagonalFieldGenerator.ClearField(_currentHexMap);
        }

        _infoPanelDisplay.UpdateInfo(null, null, 0);
    }

    private void SwitchCellSelectionPhase(EnumCellSelectionPhase cellSelectionPhase)
    {        
        _cellSelectionPhase = cellSelectionPhase;

        if(_cellSelectionPhase == EnumCellSelectionPhase.None)
        {
            return;
        }

        if(_cursor != null)
        {
            Destroy(_cursor);
        }

        GameObject cursorPrefab = 
            _cellSelectionPhase == EnumCellSelectionPhase.Start ? 
                _defaultMapLayout.StartBeaconPrefab : 
                _defaultMapLayout.EndBeaconPrefab;
        
        _cursor = Instantiate(cursorPrefab, Input.mousePosition, Quaternion.identity);
    }

    private void OnCellClick(HexagonalCell cell)
    {
        if (!cell.Walkable)
        {
            return;
        }

        int pathLength = default;

        switch (_cellSelectionPhase)
        {
            case EnumCellSelectionPhase.Start:
                HandleStartCellSelection(cell);
                break;
            case EnumCellSelectionPhase.End:
                HandleEndCellSelection(cell);
                break;
        }

        _infoPanelDisplay.UpdateInfo(
            _currentStartingCell?.Coords,
            _currentEndingCell?.Coords,
            pathLength);

        void HandleStartCellSelection(HexagonalCell cell)
        {
            _currentStartingCell = cell;
            UpdateBeacon(ref _startBeacon, cell, _defaultMapLayout.StartBeaconPrefab);

            if (_currentEndingCell != null)
            {
                ResetAllCells();
                pathLength = _hexagonalPathFinder.FindPathOnMap(
                    _currentStartingCell, _currentEndingCell).Count;
            }
        }

         void HandleEndCellSelection(HexagonalCell cell)
        {
            _currentEndingCell = cell;
            UpdateBeacon(ref _endBeacon, cell, _defaultMapLayout.EndBeaconPrefab);

            if (_currentStartingCell != null)
            {
                ResetAllCells();
                pathLength = _hexagonalPathFinder.FindPathOnMap(
                    _currentStartingCell, _currentEndingCell).Count;
            }
        }

        void UpdateBeacon(ref GameObject beacon, HexagonalCell cell, GameObject prefab)
        {
            if (beacon == null)
            {
                beacon = Instantiate(prefab, cell.Coords.Pos, Quaternion.identity);
            }
            else
            {
                beacon.transform.position = cell.Coords.Pos;
            }
        }

        void ResetAllCells()
        {
            _currentHexMap.CellMap.ToList().ForEach(
                tile => tile.Value.CellGameObject<HexagonalCell>().ResetSprite());
        }
    }

    void Update()
    {
        if(_cellSelectionPhase != EnumCellSelectionPhase.None && _cursor != null)
        {
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = 10.0f;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            _cursor.transform.position = worldPosition;
        }
    }
}
