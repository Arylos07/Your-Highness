using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
using System;

namespace RNGesus
{
    [Serializable]
    public class IntRange
    {
        //[InfoBox("You can also assign the min max values dynamically by referring to members.")]
        //[MinMaxSlider("Range", true)]
        public Vector2Int Values = new Vector2Int(25, 75);

        public Vector2Int Range = new Vector2Int(1, 20);

        public int Min { get { return this.Values.x; } }

        public int Max { get { return this.Values.y; } }

        public int Get()
        {
            return RNGesus.GetRangeInt(Min, Max + 1);
        }
    }

    [Serializable]
    public class FloatRange
    {
        //[InfoBox("You can also assign the min max values dynamically by referring to members.")]
        //[MinMaxSlider("Range", true)]
        public Vector2 Values = new Vector2(0.25f, 0.75f);

        public Vector2 Range = new Vector2(0, 1);

        public float Min { get { return this.Values.x; } }

        public float Max { get { return this.Values.y; } }

        public float Get()
        {
            return RNGesus.GetRangeFloat(Min, Max);
        }
    }

    public class StringRange
    {
        public List<string> Values = new List<string>();

        public int Count { get { return this.Values.Count; } }

        public string Get()
        {
            return Values[RNGesus.GetRangeInt(0, Count)];
        }
    }
}