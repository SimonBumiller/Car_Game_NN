using System.Collections.Generic;
using UnityEngine.Assertions;

public interface GeneticSelection
{
    public static readonly GeneticSelection Elitist = ElitistSelection.Instance;
    public static readonly GeneticSelection Default = Elitist;

    /// <summary>
    ///     Selects the new base generation out of the current Generation.
    /// </summary>
    /// <param name="generation">The current generation. It is assumed, that this generation is sorted, highest fitness first.</param>
    /// <returns>The base for the new generation</returns>
    public List<Genotype> Select(List<Genotype> generation);

    private class ElitistSelection : GeneticSelection
    {
        private const int Amount = 2;
        public static readonly GeneticSelection Instance = new ElitistSelection();

        public List<Genotype> Select(List<Genotype> generation)
        {
            Assert.IsTrue(generation.Count >= Amount,
                "The size of the generation must be equal or bigger than " + Amount);
            return generation.GetRange(0, Amount);
        }
    }
}