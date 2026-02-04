using System;
using System.Collections.Generic;
using UnityEngine;

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