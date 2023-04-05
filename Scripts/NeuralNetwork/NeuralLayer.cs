using Unity.Mathematics;
using UnityEngine.Assertions;

public class NeuralLayer
{
    public const float BIAS = 1F;
    private static Random rand;

    public ActivationFunction ActivationFunction;

    public int NeuronNum;

    public int NextLayerNeuronNums;

    public float[,] Weights;

    public NeuralLayer(int neuronNum, int nextLayerNeuronNums)
    {
        NeuronNum = neuronNum;
        NextLayerNeuronNums = nextLayerNeuronNums;

        Weights = new float[neuronNum + 1, nextLayerNeuronNums];
    }

    public void SetWeights(float[] weights)
    {
        var k = 0;
        for (var i = 0; i < Weights.GetLength(0); i++)
        for (var j = 0; j < Weights.GetLength(1); j++)
            Weights[i, j] = weights[k++];
    }

    public void SetRandomWeights(float from, float to)
    {
        var weights = new float[NeuronNum, NextLayerNeuronNums];

        for (var i = 0; i < Weights.GetLength(0); i++)
        for (var j = 0; j < Weights.GetLength(1); j++)
            Weights[i, j] = rand.NextFloat(from, to);
    }

    public float[] Fire(float[] inputs)
    {
        Assert.IsTrue(inputs.Length == NeuronNum, "inputs.Length == NeuronNum");

        var sums = new float[NextLayerNeuronNums];

        var biasedInputs = new float[inputs.Length + 1]; //Plus one because bias
        inputs.CopyTo(biasedInputs, 0);
        biasedInputs[^1] = BIAS;

        for (var j = 0; j < Weights.GetLength(1); j++)
        for (var i = 0; i < Weights.GetLength(0); i++)
            sums[j] += Weights[i, j] * biasedInputs[i];

        if (ActivationFunction != null)
            for (var i = 0; i < sums.Length; i++)
                sums[i] = ActivationFunction.Activate(sums[i]);

        return sums;
    }

    public NeuralLayer Copy()
    {
        var copy = new NeuralLayer(NeuronNum, NextLayerNeuronNums);

        Weights.CopyTo(copy.Weights, 0);
        copy.ActivationFunction = ActivationFunction;

        return copy;
    }
}