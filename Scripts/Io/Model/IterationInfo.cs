using System;
using System.Collections.Generic;

[Serializable]
public class IterationInfo
{
    public IterationInfo(List<GenerationInfo> generations)
    {
        this.generations = generations;
    }

    public List<GenerationInfo> generations;

}