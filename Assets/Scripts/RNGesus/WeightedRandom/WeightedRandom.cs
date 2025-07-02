using System.Collections.Generic;
using System.Linq;
using System;
//using Sirenix.Serialization;

namespace RNGesus
{
    [Serializable]
    public class WeightedRandom<T>
    {
        public Dictionary<T, float> probabiltyTable = new Dictionary<T, float>();
        private List<KeyValuePair<T, float>> sortedPair;
        private WeightAlgorithm algorithm;

        #region Constructors
        public WeightedRandom() : this(WeightAlgorithm.FairBiased) { }
        public WeightedRandom(WeightAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }
        #endregion

        public void Add(T val, float probability)
        {
            probabiltyTable.Add(val, probability);
            sortedPair = probabiltyTable.OrderBy(i => i.Value).ToList();
        }

        public void SetTable(Dictionary<T, float> table)
        {
            foreach(KeyValuePair<T, float> pair in table)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public T Next()
        {
            if (sortedPair == null || sortedPair.Count == 0)
                throw new Exception("No elements added to probability calculation!");

            float totalProbability = sortedPair.Sum(x => x.Value);
            double randomValue = RNGesus.GetRandom() * totalProbability;
            for (var i = 0; i < sortedPair.Count; i++)
            {
                //TO-DO:Implement an algorithm factory and decouple it from here.
                if (algorithm == WeightAlgorithm.FairBiased)
                {

                    randomValue -= sortedPair[i].Value;
                    if (randomValue <= 0)
                        return sortedPair[i].Key;
                }
                else
                {
                    if (randomValue < sortedPair[i].Value)
                        return sortedPair[i].Key;

                    randomValue -= sortedPair[i].Value;
                }

            }
            return default; //Replace with random pick;
        }

    }
}
