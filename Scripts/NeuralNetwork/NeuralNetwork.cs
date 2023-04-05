public class NeuralNetwork
{
    public readonly int[] NeuronCounts;

    public NeuralLayer[] Layers;

    public int WeightCount;

    public NeuralNetwork(int[] neuronCounts)
    {
        NeuronCounts = neuronCounts;

        for (var i = 1; i < neuronCounts.Length; i++)
            WeightCount += neuronCounts[i] * (neuronCounts[i - 1] + 1); // + 1 Because bias

        Layers = new NeuralLayer[neuronCounts.Length - 1];
        for (var i = 0; i < Layers.Length; i++) Layers[i] = new NeuralLayer(neuronCounts[i], neuronCounts[i + 1]);
    }

    public int InputNeurons
    {
        get
        {
            if (NeuronCounts != null) return NeuronCounts[0];
            return -1;
        }
        private set => NeuronCounts[0] = value;
    }

    public int OutputNeurons
    {
        get
        {
            if (NeuronCounts != null) return NeuronCounts[^1];
            return -1;
        }
        private set => NeuronCounts[^1] = value;
    }

    public void SetRandomWeights(float from, float to)
    {
        if (Layers != null)
            foreach (var neuralLayer in Layers)
                neuralLayer.SetRandomWeights(from, to);
    }

    public NeuralNetwork Copy()
    {
        var copy = new NeuralNetwork(NeuronCounts);
        for (var i = 0; i < Layers.Length; i++) copy.Layers[i] = Layers[i].Copy();

        return copy;
    }

    public NeuralNetwork CopyWithoutWeights()
    {
        var copy = new NeuralNetwork(NeuronCounts);
        for (var i = 0; i < Layers.Length; i++)
            copy.Layers[i].ActivationFunction = Layers[i].ActivationFunction;

        return copy;
    }
}