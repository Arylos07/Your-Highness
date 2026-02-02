using System.Collections.Generic;
using UnityEngine;
using static Traits;

namespace Budtender
{
    public static class BreedingEngine
    {
        private const int MinTraitCount = 3;
        private const int MaxTraitCountDefault = 6;

        /// <summary>
        /// Breeds two FlowerTemplates, simulating a punnett square for realism.
        /// Picks one of four children and returns the resulting Flower. Optional: print all children and the result.
        /// </summary>
        public static Flower Breed(FlowerTemplate a, FlowerTemplate b, bool printPunnett = false)
        {
            var children = new List<Flower>();
            for (int i = 0; i < 4; i++)
            {
                // For punnett square, "cross" with slight variation each time.
                var child = MixTraits(
                    GetRandomName(a.names, b.names),
                    Random.Range(0, 2) == 0 ? a.category : b.category,
                    Random.Range(a.thcMin, a.thcMax + 1),
                    Random.Range(a.cbdMin, a.cbdMax + 1),
                    a.effects,
                    a.flavors,
                    a.FlavorText,

                    GetRandomName(b.names, a.names),
                    Random.Range(0, 2) == 0 ? b.category : a.category,
                    Random.Range(b.thcMin, b.thcMax + 1),
                    Random.Range(b.cbdMin, b.cbdMax + 1),
                    b.effects,
                    b.flavors,
                    b.FlavorText
                );
                children.Add(child);
            }

            if (printPunnett)
            {
                for (int i = 0; i < 4; i++)
                    Debug.Log($"Punnett Child #{i + 1}:\n{children[i].Tooltip()}");
            }

            var chosen = children[Random.Range(0, 4)];
            Debug.Log($"Final Bred Flower (from templates):\n{chosen.Tooltip(true)}");
            return chosen;
        }

        /// <summary>
        /// Breeds two Flowers. Returns the resulting Flower. Optional: print the result.
        /// </summary>
        public static Flower Breed(Flower a, Flower b, bool print = false)
        {
            var child = MixTraits(
                a.Name, a.Category, a.Thc, a.Cbd, a.Effects, a.Flavors, a.Tooltip(),
                b.Name, b.Category, b.Thc, b.Cbd, b.Effects, b.Flavors, b.Tooltip()
            );
            if (print)
                Debug.Log($"Bred Flower from two Flowers:\nA:\n{a.Tooltip()}\nB:\n{b.Tooltip()}\n\nResult:\n{child.Tooltip(true)}");
            return child;
        }

        /// <summary>
        /// Breeds a Flower and a FlowerTemplate. Returns the resulting Flower. Optional: print the result.
        /// </summary>
        public static Flower Breed(Flower flower, FlowerTemplate template, bool print = false)
        {
            // Make a "virtual" Flower from the template with random values within range
            var temp = new Flower(template);
            var child = Breed(flower, temp, print);
            return child;
        }
        /// <summary>
        /// Breeds a FlowerTemplate and a Flower. Returns the resulting Flower. Optional: print the result.
        /// </summary>
        public static Flower Breed(FlowerTemplate template, Flower flower, bool print = false)
        {
            return Breed(flower, template, print);
        }

        // --- CORE MIXING LOGIC ---

        private static Flower MixTraits(
            string nameA, Category catA, int thcA, int cbdA, List<Effects> effA, List<Flavors> flavA, string flavorTextA,
            string nameB, Category catB, int thcB, int cbdB, List<Effects> effB, List<Flavors> flavB, string flavorTextB)
        {
            // Name: Combine parent names, e.g., "ParentA x ParentB"
            string childName = $"{nameA} x {nameB}";

            // Category: Pick randomly from either parent
            var childCategory = Random.value < 0.5f ? catA : catB;

            // THC & CBD: pick one parent's value, add slight random mutation
            int thc = (Random.value < 0.5f ? thcA : thcB) + Random.Range(-2, 3);
            int cbd = (Random.value < 0.5f ? cbdA : cbdB) + Random.Range(-2, 3);
            thc = Mathf.Clamp(thc, 0, 100);
            cbd = Mathf.Clamp(cbd, 0, 100);

            // Effects and Flavors: Up to 3, favor overlap, fill with random from both
            var effects = CrossTraits(effA, effB, MaxTraitCountDefault);
            var flavors = CrossTraits(flavA, flavB, MaxTraitCountDefault);

            // FlavorText: Prefer non-empty, or auto-generate
            string flavorText = !string.IsNullOrWhiteSpace(flavorTextA) ? flavorTextA
                                 : !string.IsNullOrWhiteSpace(flavorTextB) ? flavorTextB
                                 : $"{nameA} crossed with {nameB}.";

            // Use direct constructor (assumes you added this to Flower)
            return new Flower(childName, childCategory, thc, cbd, effects, flavors, flavorText);
        }

        // --- TRAIT CROSS HELPERS ---

        private static List<T> CrossTraits<T>(List<T> listA, List<T> listB, int? maxCountOverride = null)
        {
            var shared = new List<T>();
            var unique = new HashSet<T>(listA);
            unique.UnionWith(listB);

            // Find shared
            foreach (var trait in listA)
                if (listB.Contains(trait) && !shared.Contains(trait))
                    shared.Add(trait);

            // Decide trait count (random between 3 and max, or max of parents if you want)
            int maxParent = Mathf.Max(listA.Count, listB.Count, MinTraitCount);
            int maxCount = maxCountOverride ?? Mathf.Min(MaxTraitCountDefault, maxParent);
            int traitCount = Random.Range(MinTraitCount, maxCount + 1);

            var result = new List<T>();

            // 1. Add a random subset of shared traits (between 1 and traitCount/2)
            int numShared = Mathf.Clamp(Random.Range(1, traitCount), 0, shared.Count);
            var sharedPool = new List<T>(shared);
            sharedPool.Shuffle();
            result.AddRange(sharedPool.GetRange(0, numShared));

            // 2. Add random traits from the combined pool until traitCount reached
            var pool = new List<T>(unique);
            pool.RemoveAll(trait => result.Contains(trait));
            pool.Shuffle();

            foreach (var trait in pool)
            {
                if (result.Count >= traitCount) break;
                result.Add(trait);
            }

            // 3. Shuffle final result for fun
            result.Shuffle();

            return result;
        }

        // Random name picker for punnett square children (or fallback to "Unnamed Flower")
        private static string GetRandomName(List<string> a, List<string> b)
        {
            var pool = new List<string>();
            if (a != null) pool.AddRange(a);
            if (b != null) pool.AddRange(b);
            if (pool.Count == 0) return "Unnamed Flower";
            return pool[Random.Range(0, pool.Count)];
        }

        // Fisher-Yates shuffle (list extension)
        private static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}