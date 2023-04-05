using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public interface GeneticCrossover
{
    private const float CrossoverChance = 0.65F;
    public static readonly GeneticCrossover Total = TotalCrossover.Instance;
    public static GeneticCrossover Default = Total;

    public static Random rand = new();

    /// <summary>
    ///     Combines a base generation from selection into the new, full-sized generation.
    /// </summary>
    /// <param name="basegen">The generation that was created by the GeneticSelection.</param>
    /// <param name="generationSize">The desired size of the new generation.</param>
    /// <returns></returns>
    public List<Genotype> Crossover(List<Genotype> basegen, int generationSize);

    private static void CrossoverTwo(Genotype p1, Genotype p2, out Genotype c1, out Genotype c2)
    {
        var paramNum = p1.Weights.Length;
        float[] c1Params = new float[paramNum],
            c2Params = new float[paramNum],
            p1Params = p1.Weights,
            p2Params = p2.Weights;

        for (var i = 0; i < paramNum - 1; i++)
        {
            var crossover = rand.NextDouble() < CrossoverChance;
            if (crossover)
            {
                c1Params[i] = p2Params[i];
                c2Params[i] = p1Params[i];
            }
            else
            {
                c1Params[i] = p1Params[i];
                c2Params[i] = p2Params[i];
            }
        }

        c1 = new Genotype
        {
            Weights = c1Params
        };

        c2 = new Genotype
        {
            Weights = c2Params
        };
    }

    private class TotalCrossover : GeneticCrossover
    {
        public static readonly GeneticCrossover Instance = new TotalCrossover();

        public List<Genotype> Crossover(List<Genotype> basegen, int generationSize)
        {
            Assert.IsTrue(generationSize > 0, "generationSize > 1");

            if (generationSize < basegen.Count) return basegen.GetRange(0, generationSize);
            if (generationSize == basegen.Count) return basegen;

            var gen = new List<Genotype>();
            foreach (var genotype in basegen) gen.Add(genotype);

            while (gen.Count < generationSize)
            {
                int randI1, randI2;
                do
                {
                    randI1 = rand.Next(basegen.Count);
                    randI2 = rand.Next(basegen.Count);
                } while (randI1 == randI2);

                Genotype p1 = basegen[randI1], p2 = basegen[randI2], c1, c2;

                CrossoverTwo(p1, p2, out c1, out c2);

                gen.Add(c1);
                if (gen.Count < generationSize) gen.Add(c2);
            }

            return gen;
        }
    }
}