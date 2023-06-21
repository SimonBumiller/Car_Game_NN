using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private static int idGenerator;

    public static readonly int MAX_CHECKPOINT_DELAY = 5;

    public Material NormalMaterial;
    public Material DisabledMaterial;
    public Material BestMaterial;

    [SerializeField] private bool UserControlled;
    [SerializeField] private bool ShowLines = true;
    public Renderer Renderer;
    public int id;
    private float lastCheckpointTime;

    public CarMovement Movement;
    private Sensor[] Sensors;
    private Vector3[] SensorRayDirections;

    /// <summary>
    ///     Returns the next unique id in the sequence.
    /// </summary>
    private static int NextID => idGenerator++;

    public Agent Agent { get; set; }

    public float Progress
    {
        get => Agent.Genotype.Evaluation;
        set => Agent.Genotype.Evaluation = value;
    }


    private void Awake()
    {
        Movement = GetComponent<CarMovement>();
        Renderer = GetComponent<MeshRenderer>();
        Sensors = GetComponentsInChildren<Sensor>();
        id = NextID;

        SensorRayDirections = new Vector3[5];
        SensorRayDirections[0] = new Vector3(-1, 0, 0); //Forward
        SensorRayDirections[1] = new Vector3(-1, 0, 1); //Forward - Left
        SensorRayDirections[2] = new Vector3(0, 0, 1); //Left
        SensorRayDirections[3] = new Vector3(-1, 0, -1); //Forward - Right
        SensorRayDirections[4] = new Vector3(0, 0, -1); //Right
    }

    private void Start()
    {
        Movement.SetIsUserControlled(UserControlled);

        Movement.OnHitWall += Die;

        for (int i = 0; i < Sensors.Length; i++)
        {
            if (i < SensorRayDirections.Length)
            {
                Sensors[i].transform.position = Renderer.bounds.center;
                Sensors[i].RayDirection = SensorRayDirections[i];
            }
        }
    }

    private void Update()
    {
        foreach (var sensor in Sensors)
        {
            sensor.ShowLine = ShowLines;
        }
    }

    private void FixedUpdate()
    {
        if (!UserControlled &
            (Agent != null)) //Agent null-check because start car doesnt have one. TODO: Fix this, disable start car.
        {
            var data = new float[Sensors.Length + 1];
            for (var i = 0; i < Sensors.Length; i++) data[i] = Sensors[i].Output;
            data[^1] = Movement.Velocity;

            var controls = Agent.Process(data);
            Movement.SetInputs(controls);
        }

        for (int i = 0; i < Sensors.Length; i++)
        {
            if (i < SensorRayDirections.Length)
            {
                var newRot = Movement.Rotation * SensorRayDirections[i];
                Sensors[i].RayDirection = newRot;
            }
        }

        if (lastCheckpointTime > MAX_CHECKPOINT_DELAY) Die();
        lastCheckpointTime += Time.fixedDeltaTime;
    }

    public event Action<CarController> OnDie;

    private void Die()
    {
        enabled = false;
        Movement.Die();
        foreach (var sensor in Sensors)
        {
            sensor.ShowLine = false;
        }

        Renderer.material = DisabledMaterial;

        if (Agent != null)
            Agent.Kill();
        if (OnDie != null) OnDie(this);
    }

    public void Restart()
    {
        foreach (var sensor in Sensors)
        {
            sensor.ShowLine = true;
        }

        Renderer.material = NormalMaterial;
        enabled = true;
        Agent.Reset();
        Movement.Reset();
        lastCheckpointTime = 0F;
    }

    public void OnCheckpoint()
    {
        lastCheckpointTime = 0F;
    }
}