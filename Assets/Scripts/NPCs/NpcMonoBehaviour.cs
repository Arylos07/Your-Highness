using Sirenix.OdinInspector.Editor.GettingStarted;
using UnityEngine;

public class NpcMonoBehaviour : MonoBehaviour
{
    public NpcMood Mood { get; private set; } = NpcMood.Good;
    public int GetMoodIndex()
    {
        return (int)this.Mood;
    }

    private NpcState currentState;
    public NpcState CurrentState
    {
        get
        {
            return this.currentState;
        }
        set
        {
            this.UpdateCurrentState(value, true);
        }
    }

    //add some stuff for quests and pooling in the sell screen.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMood(int moodToAdd, NpcMood minMood = NpcMood.VeryBad, NpcMood maxMood = NpcMood.VeryGood)
    {
        NpcMood newMood = (NpcMood)Mathf.Clamp((int)(this.Mood + moodToAdd), (int)minMood, (int)maxMood);
        this.SetMood(newMood, minMood, maxMood);
    }

     //unlike AddMood, this function will have other calculations and state corrections to do. AddMood is just a flat modifier.
    public void SetMood(NpcMood newMood, NpcMood minMood = NpcMood.VeryBad, NpcMood maxMood = NpcMood.VeryGood)
    {
        newMood = (NpcMood)Mathf.Clamp((int)newMood, (int)minMood, (int)maxMood);
        this.Mood = newMood;
    }

    public void UpdateCurrentState(NpcState newState, bool callOnStateChanged = true)
    {
        this.currentState = newState;
        if (!callOnStateChanged)
        {
            //Do not animate or update character: just change state.
            return;
        }
    }
}
