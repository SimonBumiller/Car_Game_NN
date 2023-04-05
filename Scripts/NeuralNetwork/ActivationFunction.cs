using System;

public interface ActivationFunction
{
    public static readonly ActivationFunction Sigmoid = SigmoidActivationFunction.Instance;
    public static readonly ActivationFunction SoftSign = SoftsignActivationFUnction.Instance;
    public static readonly ActivationFunction Default = Sigmoid;

    /// <summary>
    ///     Triggers this activation function, and returns the y-Value
    /// </summary>
    /// <param name="x"></param>
    /// <returns>The corresponding y Value.</returns>
    float Activate(float x);

    private class SigmoidActivationFunction : ActivationFunction
    {
        public static readonly ActivationFunction Instance = new SigmoidActivationFunction();

        public float Activate(float x)
        {
            return (float)(1 / (1 + Math.Pow(Math.E, -x)));
        }
    }

    private class SoftsignActivationFUnction : ActivationFunction
    {
        public static readonly ActivationFunction Instance = new SoftsignActivationFUnction();

        public float Activate(float x)
        {
            return x / (1 + Math.Abs(x));
        }
    }
}