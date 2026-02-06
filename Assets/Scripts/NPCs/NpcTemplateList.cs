using System;
using System.Collections.Generic;
using UnityEngine;

/* this class was made based on Potion Craft, but I'm not sure if it's really needed. 
 * NpcTemplate can already be serialized in a list, so having a dedicated object may not be necessary.
 */
[Serializable]
public class NpcTemplateList
{
    public NpcTemplateList()
    {
    }

    public NpcTemplateList(List<NpcTemplate> npc)
    {
        this.templates = npc;
    }

    public List<NpcTemplate> templates = new List<NpcTemplate>();
}