using UnityEngine;
using UnityCommunity.UnitySingleton;
using NUnit.Framework;
using System.Collections.Generic;
using Sirenix.Serialization;
using System;
using Sirenix.OdinInspector;

public class GameManager : MonoSingleton<GameManager>
{
    //do not keep these public; I'm just being lazy for now
    public int Money = 0;
    [Obsolete("This is not implemented yet.")]
    public int Reputation = 0; //unused for now. It will be used to determine customer spawn rates and barter chances
    [Obsolete("This is not implemented yet.")]
    public int Experience = 0;

    //This will be replaceed later. I am thinking different lists for different products; flowers in one, edibles in another, etc.
    public List<FlowerSlot> FlowerInventory = new List<FlowerSlot>();

    [OdinSerialize] public Flower testFlower;

    public DateTime Calendar;
    [Tooltip("mm-dd format , e.g. 01-01 for January 1st"),
        Header("These act as a range since a random start date is selected")]
    public Vector2 StartDate;
    [Tooltip("mm-dd format , e.g. 01-01 for January 1st")]
    public Vector2 EndDate;

    // The year to start the game in, can be changed later
    // One thing to look at is that I'm not a fan of games like these starting from year 0/1.
    // However, I know some would like that.
    // As a future setting, I will make a setting so that players can choose the start year; either start from year 0/1 or enter a year of their choice and increment that value as needed.
    public int StartYear = 2023;

    [ShowInInspector, ReadOnly]
    public string CurrentDate => Calendar.ToString("MM-dd-yyyy");

    private void Start()
    {
        DateTime _date = new DateTime(StartYear, 
            (int)UnityEngine.Random.Range(StartDate.x, EndDate.x + 1), 
            (int)UnityEngine.Random.Range(StartDate.y, EndDate.y + 1));
        Calendar = _date;
    }

    // Event that is triggered when the day advances
    public event Action<DateTime> OnDayAdvanced;

    //This should only ever advance one day at a time. It will pass the new date to the event
    // ignore the temptation to pass the number of days that passed; it will only ever be one with this function
    [Button("Advance Day")]
    public void AdvanceDay()
    {
        Calendar = Calendar.AddDays(1);

        int listenerCount = OnDayAdvanced?.GetInvocationList().Length ?? 0;
        Debug.Log($"Day advanced to {CurrentDate}; Notified {listenerCount} listeners.");

        OnDayAdvanced?.Invoke(Calendar);
    }
}
