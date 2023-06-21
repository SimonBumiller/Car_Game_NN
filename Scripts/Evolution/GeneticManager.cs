using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GeneticManager : MonoBehaviour
{
    public static GeneticManager Instance;

    [SerializeField] private int[] layers;
    [SerializeField] private int CarNumber;

    private readonly List<Agent> Agents = new();

    public GeneticAlgorithm GeneticAlgorithm;
    public int LivingAgents;
    public int Iteration;

    private void Awake()
    {
        Instance = this;
    }

    private event Action AllCarsDead;

    public void StartEvolution()
    {
        Assert.IsTrue(layers.Length != 5, "layers.Length != 5");

        var net = new NeuralNetwork(layers); //For weights num
        GeneticAlgorithm = new GeneticAlgorithm(CarNumber, net.WeightCount)
        {
            Select = GeneticSelection.Default.Select,
            Crossover = GeneticCrossover.Default.Crossover,
            Mutate = GeneticMutation.Default.Mutate
        };
        Iteration = 1;
        GeneticAlgorithm.Race += StartRace;

        AllCarsDead += GeneticAlgorithm.Evolution;

        GeneticAlgorithm.Start();
    }

    private void StartRace(List<Genotype> gen)
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
        Track.Instance.Restart();
    }

    private void AgentDie(Agent agent)
    {
        LivingAgents--;

        if (LivingAgents == 0) GeneticAlgorithm.Evolution();
    }

    public void Reset()
    {
        Iteration = Iteration + 1;
        StartEvolution();
    }
}