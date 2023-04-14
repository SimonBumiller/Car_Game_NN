using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private static int idGenerator;

    public static readonly int MAX_CHECKPOINT_DELAY = 5;

    [SerializeField] private bool UserControlled;
    [SerializeField] private Sprite DisabledSprite;
    [SerializeField] private Sprite NormalSprite;
    [SerializeField] private bool ShowCrosses = true;
    [SerializeField] private bool ShowLines = true;
    public SpriteRenderer Renderer;
    public int id;
    private float lastCheckpointTime;

    public CarMovement Movement;
    private Sensor[] Sensors;

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
        Renderer = GetComponent<SpriteRenderer>();
        Sensors = GetComponentsInChildren<Sensor>();
        transform.position.Normalize();
        id = NextID;
    }

    private void Start()
    {
        Movement.SetIsUserControlled(UserControlled);

        Movement.OnHitWall += Die;

        var pos = transform.position;

        Sensors[0].SetPosition(new Vector2(pos.x + 2, pos.y));
        Sensors[1].SetPosition(new Vector2(pos.x + 1.3F, pos.y + 0.85F));
        Sensors[2].SetPosition(new Vector2(pos.x + 1.3F, pos.y - 0.85F));
        Sensors[3].SetPosition(new Vector2(pos.x + 0.5F, pos.y - 1.5F));
        Sensors[4].SetPosition(new Vector2(pos.x + 0.5F, pos.y + 1.5F));
    }

    private void Update()
    {
        foreach (var sensor in Sensors)
        {
            sensor.ShowCross = ShowCrosses;
            sensor.ShowLine = ShowLines;
        }
    }

    private void FixedUpdate()
    {
        if (Agent == null) enabled = false;

        if (!UserControlled &
            (Agent != null)) //Agent null-check because start car doesnt have one. TODO: Fix this, disable start car.
        {
            var data = new float[Sensors.Length + 1];
            for (var i = 0; i < Sensors.Length; i++) data[i] = Sensors[i].Output;
            data[^1] = Movement.Velocity;

            var controls = Agent.Process(data);
            Movement.SetInputs(controls);
        }

        if (lastCheckpointTime > MAX_CHECKPOINT_DELAY) Die();
        lastCheckpointTime += Time.fixedDeltaTime;
    }

    public event Action<CarController> OnDie;

    private void Die()
    {
        Renderer.sprite = DisabledSprite;
        enabled = false;
        Movement.Die();
        foreach (var sensor in Sensors)
        {
            sensor.ShowCross = false;
            sensor.ShowLine = false;
        }

        Agent.Kill();
        if (OnDie != null) OnDie(this);
    }

    public void Restart()
    {
        foreach (var sensor in Sensors)
        {
            sensor.ShowCross = true;
            sensor.ShowLine = true;
        }

        Renderer.sprite = NormalSprite;
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