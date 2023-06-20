using UnityEngine;
using UnityEngine.UI;

public class SizeInfoController : MonoBehaviour
{

    [SerializeField] private Text CurrentLivingText;
    [SerializeField] private Text TotalSizeText;

    void Update()
    {
        var totalText = GeneticManager.Instance != null
            ? GeneticManager.Instance.GeneticAlgorithm.Size.ToString()
            : "N/A";
        var currentLiving = GeneticManager.Instance != null
            ? GeneticManager.Instance.LivingAgents.ToString()
            : "N/A";

        CurrentLivingText.text = currentLiving;
        TotalSizeText.text = totalText;
    }
}
