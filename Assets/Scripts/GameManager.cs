using UnityEngine;
using UnityCommunity.UnitySingleton;
using NUnit.Framework;
using System.Collections.Generic;
using Sirenix.Serialization;

public class GameManager : MonoSingleton<GameManager>
{
    //do not keep these public; I'm just being lazy for now
    public int Money = 0;
    public int Reputation = 0; //unused for now. It will be used to determine customer spawn rates and barter chances
    public int Experience = 0;

    //This will be replaceed later. I am thinking different lists for different products; flowers in one, edibles in another, etc.
    public List<FlowerSlot> FlowerInventory = new List<FlowerSlot>();

    [OdinSerialize] public Flower testFlower;
}
