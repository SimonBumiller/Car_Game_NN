using System;
using System.Collections;
using System.Collections.Generic;

public class Genotype : IComparable<Genotype>, IEnumerable<float>
{
    private static readonly Random rand = new();

    /// <summary>
    ///     The Objective Strength of the Participator
    /// </summary>
    public float Evaluation;

    /// <summary>
    ///     The Subjective Strength of the Participator, relative to its' generation.
    /// </summary>
    public float Fitness;

    /// <summary>
    ///     The Genes of the Participator. This relates to the weights in the NN.
    /// </summary>
    public float[] Weights;

    public int CompareTo(Genotype other)
    {
        return other.Fitness.CompareTo(Fitness);
    }

    public IEnumerator<float> GetEnumerator()
    {
        foreach (var weight in Weights) yield return weight;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void SetRandomWeights(float from, float to)
    {
        for (var i = 0; i < Weights.Length; i++) Weights[i] = (float)(rand.NextDouble() * (to - from) + from);
    }

    public float[] CopyWeights()
    {
        var copy = new float[Weights.Length];
        Weights.CopyTo(copy, 0);

        return copy;
    }
}