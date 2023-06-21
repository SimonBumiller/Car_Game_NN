using UnityEngine;
using UnityEngine.UI;

public class CarInfoController : MonoBehaviour
{
    public static CarInfoController Instance;

    public CarController Car;

    [SerializeField] private Text AccelerationTest, SpeedText;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (Car is null) return;
        var acceleration = Movement.AccelerationHelper.from((float)Car.Movement.horizontalInput);
        var speed = Movement.Speedhelper.from(Car.Movement.Velocity, CarMovement.MaxVel, -CarMovement.MaxVel);
        var aText = Movement.nameFor(acceleration);
        var sText = Movement.nameFor(speed);

        AccelerationTest.text = aText;
        SpeedText.text = sText;
    }
}