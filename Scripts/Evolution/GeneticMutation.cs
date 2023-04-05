using System;
using System.Collections.Generic;

public interface GeneticMutation
{
    /// <summary>
    ///     Describes how many genotypes of a generation get mutated, in %.
    /// </summary>
    private const float MutationChanceGenotype = 1F;

    /// <summary>
    ///     Describes how many genes of a to be mutated genome are actually mutated, in %.
    /// </summary>
    private const float MutationChanceGene = 1F / 3F;

    /// <summary>
    ///     Describes by how much a gene that is mutated can change, in %.
    /// </summary>
    private const float MutationAmountGene = 2F;

    public static readonly GeneticMutation All = MutateAllMutation.Instance;
    public static readonly GeneticMutation Default = All;

    public static Random rand = new();

    /// <summary>
    ///     Mutates a generation of Genotypes.
    /// </summary>
    /// <param name="gen">The current Generation to mutate.</param>
    /// <returns>The Generation, with mutated genes.</returns>
    public List<Genotype> Mutate(List<Genotype> gen);

    private static void MutateGenotype(Genotype g)
    {
        for (var i = 0; i < g.Weights.Length; i++)
            if (rand.NextDouble() < MutationChanceGene)
            {
                var sign = rand.NextDouble() > 0.5F ? 1 : -1; //50:50 Whether increase or decrease
                var change =
                    (float)(g.Weights[i] * MutationAmountGene * rand.NextDouble() *
                            sign); //Change in gene, randomized percentage of maximum mutation amount.
                g.Weights[i] += change;
            }
    }

    private class MutateAllMutation : GeneticMutation
    {
        public static readonly GeneticMutation Instance = new MutateAllMutation();

        public List<Genotype> Mutate(List<Genotype> gen)
        {
            foreach (var genotype in gen)
                if (rand.NextDouble() < MutationChanceGenotype)
                    MutateGenotype(genotype);

            return gen; //Technically not needed, but for consistency with crossover and selection.
        }
    }
}