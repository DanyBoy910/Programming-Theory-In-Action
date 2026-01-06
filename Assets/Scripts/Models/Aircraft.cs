using UnityEngine;

public abstract class Aircraft : MonoBehaviour
{
    //ENCAPSULATION
    [SerializeField] private string modelName;
    [SerializeField] private float fuelLevel = 100f;
    [SerializeField] private bool isEngineOn = false; // Engine is off by default
    [SerializeField] protected Vector3 idealCameraOffset = new Vector3(0, 5, -10);

    //GETS AND SETTERS
    public float FuelLevel
    {
        get { return fuelLevel; }
        protected set { fuelLevel = value; }
    }

    public Vector3 IdealCameraOffset
    {
        get{return idealCameraOffset; }
    }

    public bool IsEngineOn
    {
        get { return  isEngineOn; }
        protected set { isEngineOn = value; }
    }

    // ABSTRACTION
    // Each aircraft will decide how to move.
    public abstract void Move();
    public abstract void StartEngine();
    public abstract void StopEngine();

    // Un método común que todos heredan igual.
    public void DisplayInfo()
    {
        Debug.Log("Aircraft: " + modelName + " - Fuel Level: " + fuelLevel + "%");
    }

}
