using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Track : MonoBehaviour
{
    public static Track Instance;

    [SerializeField] private CarController StartCar;
    [SerializeField] private Sprite BestCar;
    [SerializeField] private Sprite NormalCar;
    public readonly List<TrackCar> Cars = new();

    private TrackCar best;

    private Checkpoint[] Checkpoints;
    private Vector2 startPos;
    private Quaternion startRot;
    private float TotalTrackLenght;

    private TrackCar Best
    {
        get => best;
        set
        {
            if (best == value) return; //Same car is still best
            if (value == null)
            {
                best = null;
                return;
            }

            if (best != null && best.Controller.enabled) //Janky but all we can do!
                best.Controller.Renderer.material = best.Controller.NormalMaterial;

            best = value;
            best.Controller.Renderer.material = best.Controller.BestMaterial;
            if (OnBestCarChange != null) OnBestCarChange(value.Controller);
        }
    }


    private void Awake()
    {
        Checkpoints = GetComponentsInChildren<Checkpoint>();
        var startTransform = StartCar.transform;
        startPos = startTransform.position;
        startRot = startTransform.rotation;

        Instance = this;
    }

    private void Start()
    {
        StartCar.enabled = false;
        SetupCheckpointsValues();
    }

    private void Update()
    {
        var enabledCars = Cars.FindAll(car => car.Controller.enabled); //Filter out already dead cars
        if (enabledCars.Count == 0)
        {
            if (OnLastCarDie != null) OnLastCarDie();

            return;
        }

        foreach (var trackCar in enabledCars)
        {
            Assert.IsNotNull(trackCar);

            var controller = trackCar.Controller;

            trackCar.Controller.Progress = CarTrackPercentage(controller, ref trackCar.Checkpoint);
            var progress = controller.Progress;

            Best ??= trackCar;
            var bestController = best.Controller;
            var bestProgress = bestController.Progress;

            if (bestProgress < progress) Best = trackCar; //Set new best car if car is better than best.
        }
    }

    public event Action<CarController> OnBestCarChange;
    public event Action OnLastCarDie;

    public void Restart()
    {
        StartCar.enabled = false;
        best = null;

        foreach (var trackCar in Cars)
        {
            trackCar.Controller.Restart();
            trackCar.Controller.transform.position = startPos;
            trackCar.Controller.transform.rotation = startRot;
            trackCar.Checkpoint = 0;
        }
    }

    public void CreateCars(int amount)
    {
        if (amount == Cars.Count || amount < 0) return;

        if (amount > Cars.Count)
        {
            var toAdd = amount - Cars.Count;

            for (var i = 0; i < toAdd; i++)
            {
                var newCar = Instantiate(StartCar.gameObject);
                newCar.transform.position = startPos;
                newCar.transform.rotation = startRot;
                var newController = newCar.GetComponent<CarController>();
                newCar.SetActive(true);
                newController.OnDie += OnCarDie;
                Cars.Add(new TrackCar(newController, 0));
            }
        }
        else if (amount < Cars.Count)
        {
            //Remove existing cars
            for (var toBeRemoved = Cars.Count - amount; toBeRemoved > 0; toBeRemoved--)
            {
                var last = Cars[^1];
                Cars.RemoveAt(Cars.Count - 1);

                Destroy(last.Controller.gameObject);
            }
        }
    }

    private void OnCarDie(CarController car)
    {
        if (Best == null || Best.Controller == car) Best = null;
    }

    /// <summary>
    ///     Sets up the values of the checkpoints, such as the rewards and the distances.
    /// </summary>
    private void SetupCheckpointsValues()
    {
        if (Checkpoints.Length < 1) return;

        Checkpoints[0].TotalDistance = 0; //First Checkpoint is start of game.

        for (var i = 1; i < Checkpoints.Length; i++)
        {
            Checkpoints[i].DistanceToLast = Vector2.Distance(Checkpoints[i].Position, Checkpoints[i - 1].Position);
            Checkpoints[i].TotalDistance = Checkpoints[i].DistanceToLast + Checkpoints[i - 1].TotalDistance;
        }

        TotalTrackLenght = Checkpoints[^1].TotalDistance;

        for (var i = 1; i < Checkpoints.Length; i++)
        {
            Checkpoints[i].Reward =
                Checkpoints[i].TotalDistance / TotalTrackLenght; /*-
                Checkpoints[i - 1].TotalDistance; //Percentage of total minus total reward of earlier*/
            Checkpoints[i].TotalReward = Checkpoints[i - 1].TotalReward + Checkpoints[i].Reward;
        }

        var values = new float[Checkpoints.Length];
        for (var i = 0; i < Checkpoints.Length; i++) values[i] = Checkpoints[i].Reward;

        Debug.Log("Checkpoint values: " + string.Join(", ", values));
    }

    private float CarTrackPercentage(CarController car, ref int checkpoint)
    {
        if (checkpoint >= Checkpoints.Length) return 1; //Car is finished

        //Calculate distance to next checkpoint
        var checkPointDistance =
            Vector2.Distance(car.transform.position, Checkpoints[checkpoint + 1].transform.position);

        //Check if checkpoint can be captured
        if (checkPointDistance <= Checkpoints[checkpoint + 1].Radius)
        {
            checkpoint++;
            car.OnCheckpoint();
            return CarTrackPercentage(car, ref checkpoint); //Recursively check next checkpoint
        }

        //Return accumulated reward of last checkpoint + reward of distance to next checkpoint
        return Checkpoints[checkpoint].TotalReward +
               Checkpoints[checkpoint + 1].GetPartOfReward(checkPointDistance);
    }

    /// <summary>
    ///     Describes a Car that is an entity on a track, and its progress relative to the track.
    /// </summary>
    public class TrackCar
    {
        public int Checkpoint;

        public CarController Controller;

        public TrackCar(CarController controller, int checkpoint)
        {
            Controller = controller;
            Checkpoint = checkpoint;
        }

        public override string ToString()
        {
            return "TrackCar{ Controller = " + Controller + ", Checkpoint = " + Checkpoint;
        }
    }
}