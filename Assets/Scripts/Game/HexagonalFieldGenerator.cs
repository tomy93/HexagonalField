using System;
using System.Collections.Generic;
using UnityEngine;

public class HexagonalFieldGenerator
{
    public static HexagonalMap GenerateHexagonalField(
        MapLayout defaultMapLayout, Action<HexagonalCell> onCellClicked)
    {
        MapLayout layout = defaultMapLayout;
        HexagonalMap hexMap = new HexagonalMap();
        hexMap.parentGameObject = new GameObject("Field");
        hexMap.CellMap = new Dictionary<Vector2, ICell>();

        for (var r = 0; r < layout.GridDepth ; r++) 
        {
            var rOffset = r >> 1;
            for (var q = -rOffset; q < layout.GridWidth - rOffset; q++) 
            {
                var tile = GameObject.Instantiate(
                    layout.CellPrefab, hexMap.parentGameObject.transform);
                tile.Setup(q, r, GetRandomCellLayout(layout.CellLayouts), onCellClicked);
                hexMap.CellMap.Add(tile.Coords.Pos,tile);
            }
        }

        return hexMap;
    }

    private static CellLayout GetRandomCellLayout(CellLayout[] cellLayouts)
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        for (int i = 0; i < cellLayouts.Length; i++)
        {
            cumulativeProbability += (cellLayouts[i].probability * .01f);
            if (randomValue <= cumulativeProbability)
            {
                return cellLayouts[i];
            }
        }

        // Default to the first prefab if the probability values weren't set correctly
        return cellLayouts[0];
    }

    public static void ClearField(HexagonalMap hexMap)
    {
        foreach (var kvp in hexMap.CellMap)
        {
            if (kvp.Value != null)
            {
                kvp.Value.CellGameObject<HexagonalCell>().Destroy();
            }
        }

        GameObject.Destroy(hexMap.parentGameObject);
        hexMap.CellMap.Clear();
    }
}