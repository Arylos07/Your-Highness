using System;
using System.Linq;
using UnityEngine;

namespace Budtender.Traits
{
    ///
    /// DO NOT REORDER OR REMOVE VALUES - BITMASK RELIES ON FIXED INDICES
    /// ONLY RENAME IF NECESSARY AND UPDATE ANY REFERENCES
    ///
    public enum Category
    {
        Sativa,
        SativaHybrid,
        Hybrid,
        IndicaHybrid,
        Indica
    }

    ///
    /// DO NOT REORDER OR REMOVE VALUES - BITMASK RELIES ON FIXED INDICES
    /// ONLY RENAME IF NECESSARY AND UPDATE ANY REFERENCES
    ///
    public enum Effects
    {
        Happy,
        Relaxed,
        Uplifted,
        Euphoric,
        Creative,
        Energetic,
        Focused,
        Sleepy,
        Hungry,
        Talkative,
        Giggly
    }

    ///
    /// DO NOT REORDER OR REMOVE VALUES - BITMASK RELIES ON FIXED INDICES
    /// ONLY RENAME IF NECESSARY AND UPDATE ANY REFERENCES
    ///
    public enum Flavors
    {
        Citrus,
        Earthy,
        Pine,
        Spicy,
        Sweet,
        Berry,
        Herbal,
        Flowery,
        Diesel,
        Skunky,
        Pungent,
        Anise,
        Creamy,
        Vanilla
    }

    public static class TraitsManager
    {

        public static string EncodeTraits(TraitsContainer container)
        {
            // Implementation for encoding traits
            string _cat = ((int)container.Category).ToString();
            string _effects = container.Effects.Aggregate(0, (current, effect) => current | (1 << (int)effect)).ToString();
            string _flavors = container.Flavors.Aggregate(0, (current, flavor) => current | (1 << (int)flavor)).ToString();
            string _data = $"{_cat}|{_effects}|{_flavors}";
            return _data;
        }

        public static TraitsContainer DecodeTraits(string data)
        {
            TraitsContainer container = new TraitsContainer();
            try
            {
                string[] parts = data.Split('|');
                if (parts.Length != 3)
                    throw new FormatException("Invalid data format");
                container.Category = (Category)int.Parse(parts[0]);
                int effectsBitmask = int.Parse(parts[1]);
                foreach (Effects effect in Enum.GetValues(typeof(Effects)))
                {
                    if ((effectsBitmask & (1 << (int)effect)) != 0)
                    {
                        container.Effects.Add(effect);
                    }
                }
                int flavorsBitmask = int.Parse(parts[2]);
                foreach (Flavors flavor in Enum.GetValues(typeof(Flavors)))
                {
                    if ((flavorsBitmask & (1 << (int)flavor)) != 0)
                    {
                        container.Flavors.Add(flavor);
                    }
                }
                return container;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error decoding traits: {e.Message}");
            }
            return container;
        }
    }

    public class TraitsContainer
    {
        public Category Category { get; set; }
        public System.Collections.Generic.List<Effects> Effects { get; set; }
        public System.Collections.Generic.List<Flavors> Flavors { get; set; }
        public TraitsContainer()
        {
            Effects = new System.Collections.Generic.List<Effects>();
            Flavors = new System.Collections.Generic.List<Flavors>();
        }
        public TraitsContainer(Category category, Effects[] effects, Flavors[] flavors)
        {
            Category = category;
            Effects = new System.Collections.Generic.List<Effects>(effects);
            Flavors = new System.Collections.Generic.List<Flavors>(flavors);
        }
    }
}