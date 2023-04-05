using System.Collections.Generic;

public interface GeneticFitnessFunction
{
    public static readonly GeneticFitnessFunction Evaluation = EvaluationFitnessFunction.Instance;
    public static readonly GeneticFitnessFunction Default = Evaluation;

    /// <summary>
    ///     Calculates the Fitness of the genotypes in a generation.
    /// </summary>
    /// <param name="gen">The generation</param>
    /// <returns></returns>
    void Fitness(List<Genotype> gen);

    private class EvaluationFitnessFunction : GeneticFitnessFunction
    {
        public static readonly GeneticFitnessFunction Instance = new EvaluationFitnessFunction();

        public void Fitness(List<Genotype> gen)
        {
            var sum = 0F;
            foreach (var g in gen) sum += g.Evaluation;

            var average = sum / gen.Count;

            foreach (var genotype in gen)
            {
                var fitness = genotype.Evaluation / average;
                genotype.Fitness = fitness;
            }
        }
    }
}