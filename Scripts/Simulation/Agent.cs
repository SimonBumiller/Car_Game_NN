using System;

public class Agent
{
    public Genotype Genotype;

    private bool isAlive;

    public NeuralNetwork NN;

    public Agent(Genotype genotype, int[] layers, ActivationFunction activationFunction)
    {
        Genotype = genotype;
        IsAlive = true;
        NN = new NeuralNetwork(layers);
        foreach (var nl in NN.Layers) nl.ActivationFunction = activationFunction;

        //Put params 'genes' of genotype into weights of Neural network.
        var iter = Genotype.GetEnumerator();
        foreach (var layer in NN.Layers)
            for (var i = 0; i < layer.Weights.GetLength(0); i++)
            for (var j = 0; j < layer.Weights.GetLength(1); j++)
            {
                layer.Weights[i, j] = iter.Current;
                if (!iter.MoveNext())
                    throw new ArgumentException("The given genotype does not match the weight count.");
            }
    }

    public bool IsAlive
    {
        get => isAlive;
        private set
        {
            isAlive = value;

            if (!isAlive && OnAgentDie != null) OnAgentDie(this);
        }
    }

    public event Action<Agent> OnAgentDie;

    public float[] Process(float[] inputs)
    {
        var outputs = inputs;
        foreach (var layer in NN.Layers) outputs = layer.Fire(outputs);
        return outputs;
    }

    public void Reset()
    {
        Genotype.Evaluation = 0;
        Genotype.Fitness = 0;
        IsAlive = true;
    }

    /// <summary>
    ///     Kills this agent (sets IsAlive to false).
    /// </summary>
    public void Kill()
    {
        IsAlive = false;
    }
}