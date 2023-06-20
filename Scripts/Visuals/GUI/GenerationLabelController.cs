using UnityEngine;
using UnityEngine.UI;

public class GenerationLabelController : MonoBehaviour
{
    [SerializeField] private Text GenerationText;

    void Update()
    {
        GenerationText.text = GeneticManager.Instance != null
            ? GeneticManager.Instance.GeneticAlgorithm.Generation.ToString()
            : "N/A";
    }
}