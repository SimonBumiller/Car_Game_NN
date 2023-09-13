using System;
using System.Collections.Generic;

public class GeneticAlgorithm
{
    public delegate List<Genotype> CrossoverAlgo(List<Genotype> gen, int generationNum);

    public delegate List<Genotype> MutateAlgo(List<Genotype> gen);

    public delegate List<Genotype> SelectAlgo(List<Genotype> gen);

    private static readonly float DefaultParamMax = 1F;
    private static readonly float DefaultParamMin = -1F;

    public CrossoverAlgo Crossover;

    public int Generation;

    public MutateAlgo Mutate;

    public List<Genotype> Population;

    public SelectAlgo Select;

    public int Size;

    public GeneticAlgorithm(int size, int genotypeParams)
    {
        Size = size;

        Population = new List<Genotype>(Size);
        for (var i = 0; i < Size; i++)
            Population.Add(new Genotype
            {
                Weights = new float[genotypeParams]
            });

        Generation = 1;
    }

    public event Action<List<Genotype>, int> Race;
    
    /// <summary>
    /// Callback to be called shortly before the evolutionary process is started with the population.
    /// </summary>
    public event Action<List<Genotype>> PreEvolution; 

    public void Start()
    {
        FillGenotypeWeights();

        Race(Population, Generation);
    }

    /// <summary>
    ///     To be invoked after evaluation, eg the race, has stopped.
    /// </summary>
    public void Evolution()
    {
        
        GeneticFitnessFunction.Default.Fitness(Population); //Calculate Fitness
        if (PreEvolution != null) PreEvolution(Population);

        Population.Sort();

        var baseGen = Select(Population);
        var crossedGen = Crossover(baseGen, Size);
        var mutatedGen = Mutate(crossedGen);

        Population = mutatedGen;
        ++Generation;
        if (Generation == 5)
        {
            
        }

        if (Race != null) Race(Population, Generation);
    }

    private void FillGenotypeWeights()
    {
        foreach (var genotype in Population) genotype.SetRandomWeights(DefaultParamMin, DefaultParamMax);
    }
}