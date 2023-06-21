///Code taken from Samuel Arzt - Applying EANNs
/// <link> https://github.com/ArztSamuel/Applying_EANNs </link>

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that represents the outermost layer of the Game.
/// This controls and initializes the TrackManager singleton.
/// </summary>
public class CarGame : MonoBehaviour
{
    [SerializeField] private MainCamera Camera;

    [SerializeField] private string TrackName;

    void Awake()
    {
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        SceneManager.LoadScene(TrackName, LoadSceneMode.Additive);
    }

    void Start()
    {
        Track.Instance.OnBestCarChange += OnBestCarChange;
        GeneticManager.Instance.StartEvolution();
    }

    private void OnBestCarChange(CarController controller)
    {
        var controllerNull = controller is null;
        if(controllerNull) return;
        Camera.target = controller.transform;
        CarInfoController.Instance.Car = controller;
    }
}