using UnityEngine;
using UnityEngine.UI;

public class InfoPanelDisplay : MonoBehaviour
{
    [SerializeField] private Text lbl;
    public void UpdateInfo(
        ICoords startCoords = null,
        ICoords endCoords = null, 
        int pathLength = 0)
    {
        HexCoords startingCoords = startCoords as HexCoords;
        HexCoords endingCoords = endCoords as HexCoords;

        lbl.text = string.Empty;
        lbl.text += $"Coords Start:  ";
        if(startCoords != null)
        {
            lbl.text +=$"[{startingCoords.Q},{startingCoords.R}]";
        }
        lbl.text += $"\nCoords End:   ";
        if(endCoords != null)
        {
            lbl.text +=$"[{endingCoords.Q},{endingCoords.R}]";
        }
        lbl.text += $"\nPath Length:   ";
        if (pathLength > 0)
        {
            lbl.text += $"{pathLength}";
        }
    }
}
