using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Io;
using UnityEngine;
using UnityEngine.Assertions;

public class GeneticManager : MonoBehaviour
{
    public static GeneticManager Instance;

    [SerializeField] private int[] layers;
    [SerializeField] private int CarNumber;
    [SerializeField] private int[] saveAtGenerations;

    private readonly List<Agent> Agents = new();

    public GeneticAlgorithm GeneticAlgorithm;
    public int LivingAgents;
    public int Iteration;

    public List<GenerationInfo> generations;

    private void Awake()
    {
        Instance = this;
    }

    private event Action AllCarsDead;

    public void StartEvolution()
    {
        Assert.IsTrue(layers.Length != 5, "layers.Length != 5");

        generations = new List<GenerationInfo>();
        var net = new NeuralNetwork(layers); //For calculating weights num
        GeneticAlgorithm = new GeneticAlgorithm(CarNumber, net.WeightCount)
        {
            Select = GeneticSelection.Default.Select,
            Crossover = GeneticCrossover.Default.Crossover,
            Mutate = GeneticMutation.Default.Mutate
        };
        Iteration = 1;
        GeneticAlgorithm.Race += StartRace;

        AllCarsDead += GeneticAlgorithm.Evolution;

        GeneticAlgorithm.PreEvolution += (genotypes) =>
        {
            var evaluations = new List<float>(generations.Count);
            for (int i = 0; i < genotypes.Count; i++)
            {
                evaluations.Add(genotypes[i].Evaluation);
            }

            var gen = new GenerationInfo(GeneticAlgorithm.Generation, genotypes.Count, evaluations);
            generations.Add(gen);
        };

        GeneticAlgorithm.Start();
    }

    private void StartRace(List<Genotype> gen, int generation)
    {
        Agents.Clear();
        LivingAgents = 0;

        foreach (var genotype in gen) Agents.Add(new Agent(genotype, layers, ActivationFunction.SoftSign));

        Track.Instance.CreateCars(Agents.Count);

        Assert.AreEqual(Track.Instance.Cars.Count, Agents.Count, "Track.Cars.Count == Agents.Count");

        for (var i = 0; i < Track.Instance.Cars.Count; i++)
        {
            Track.Instance.Cars[i].Controller.Agent = Agents[i];
            LivingAgents++;
            Agents[i].OnAgentDie += AgentDie;
        }

        if (saveAtGenerations.Contains(generation))
        {
            SaveToFile();
        }
        Track.Instance.Restart();
    }

    private void AgentDie(Agent agent)
    {
        LivingAgents--;

        if (LivingAgents == 0)
        {
            GeneticAlgorithm.Evolution();
        }
    }

    public void Reset()
    {
        SaveToFile();
        Iteration = Iteration + 1;
        StartEvolution();
    }

    public void SaveToFile()
    {
        IterationManager.Instance.SaveIteration(new IterationInfo(generations));
    }
}