using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Budtender.Genealogy
{
    public class Genealogy
    {
        public static List<BreedingStep> DecodeGenealogy(string genealogyData)
        {
            var steps = new List<BreedingStep>();
            var entries = genealogyData.Split(';');
            foreach (var entry in entries)
            {
                var parts = entry.Split('|');
                if (parts.Length != 5) continue;
                var step = new BreedingStep
                {
                    ParentA = new ParentInfo
                    {
                        Name = parts[0],
                        Description = parts[1],
                        Traits = parts[2]
                    },
                    ParentB = new ParentInfo
                    {
                        Name = parts[3],
                        Description = parts[4],
                        Traits = parts[5]
                    },
                    Generation = steps.Count + 1
                };
                steps.Add(step);
            }
            return steps;
        }

        public static string EncodeGenealogy(List<BreedingStep> steps)
        {
            var entries = new List<string>();
            foreach (var step in steps)
            {
                var entry = $"{step.ParentA.Name}|{step.ParentA.Description}|{step.ParentA.Traits}|{step.ParentB.Name}|{step.ParentB.Description}";
                entries.Add(entry);
            }
            return string.Join(";", entries);
        }
    }

    public class BreedingStep
    {
        public ParentInfo ParentA;
        public ParentInfo ParentB;
        public int Generation;
    }

    public class ParentInfo
    {
        public string Name;
        public string Description;
        //stats, flavors, effects, etc encoded as a string
        public string Traits;
    }
}