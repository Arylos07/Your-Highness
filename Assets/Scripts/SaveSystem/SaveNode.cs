using System;
using UnityEngine;

public class SaveNode : MonoBehaviour
{
    private void OnMouseDown()
    {
        Save(); // Save the game data; this is a placeholder for now
    }

    [Obsolete("Not implemented yet. It tells the GameManager to advance the day, but it doesn't save any data yet.")]
    public void Save()
    {
        Debug.LogWarning("Save functionality is not implemented yet; no data was saved.");
        // Placeholder: Add save logic here in the future

        GameManager.Instance.AdvanceDay(); // Tell the game manager to advance the day
    }
}
