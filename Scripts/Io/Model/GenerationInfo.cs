using System;
using System.Collections.Generic;

[Serializable]
public class GenerationInfo
{
    public GenerationInfo(int generationNum, int agentNum, List<float> fitnesses)
    {
        this.generationNum = generationNum;
        this.agentNum = agentNum;
        this.fitnesses = fitnesses;
    }

    public int generationNum;
    
    public int agentNum;

    public List<float> fitnesses;

}