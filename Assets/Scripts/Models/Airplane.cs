using UnityEngine;

public class Airplane : Aircraft 
{
    private Rigidbody rb;

    [Header("Configuración de Motor")]
    public float throttlePower = 20f;
    public float maxSpeed = 50f;

    [Header("Configuración de Control")]
    public float pitchSpeed = 5f; // nose
    public float rollSpeed = 5f;  // roll
    public float yawSpeed = 2f;   //

    [Header("Aerodinámica")]
    public float liftPower = 0.5f;
    public float aerodynamicStability = 0.05f; 

    // Variables de Input
    private float activeRoll, activePitch, activeYaw;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
   
        rb.angularDamping = 2f;
        rb.linearDamping = 0.5f;
    }

    void Update()
    {
     
        activePitch = Input.GetAxis("Vertical");   // W/S 
        activeRoll = Input.GetAxis("Horizontal"); // A/D 

        if (Input.GetKey(KeyCode.Q)) activeYaw = -1f;
        else if (Input.GetKey(KeyCode.E)) activeYaw = 1f;
        else activeYaw = 0f;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!IsEngineOn) StartEngine();
            else StopEngine();
        }
    }

    void FixedUpdate()
    {
        if (IsEngineOn)
        {
            Move();
        }
    }

    public override void Move()
    {
        float forwardSpeed = transform.InverseTransformDirection(rb.linearVelocity).z;
        if (forwardSpeed < maxSpeed)
        {
            rb.AddRelativeForce(Vector3.forward * throttlePower, ForceMode.Acceleration);
        }

        if (forwardSpeed > 0)
        {
            float liftFactor = Vector3.Dot(transform.up, Vector3.up);

            float effectiveLift = Mathf.Max(0, liftFactor);

            Vector3 liftForce = Vector3.up * (forwardSpeed * forwardSpeed) * liftPower * effectiveLift;
            rb.AddForce(transform.up * liftForce.magnitude * 0.05f, ForceMode.Force);
        }

        float controlAuthority = Mathf.Clamp01(forwardSpeed / 10f);
        Vector3 torque = new Vector3(activePitch * pitchSpeed, activeYaw * yawSpeed, -activeRoll * rollSpeed);
        rb.AddRelativeTorque(torque * controlAuthority, ForceMode.Force);

        if (rb.linearVelocity.magnitude > 1f)
        {
            Vector3 velocity = rb.linearVelocity;
            Vector3 desiredVelocity = transform.forward * velocity.magnitude;

            rb.linearVelocity = Vector3.Lerp(velocity, desiredVelocity, aerodynamicStability);
        }
    }

    public override void StartEngine()
    {
        IsEngineOn = true;
        Debug.Log("ENGINE START");
    }

    public override void StopEngine()
    {
        IsEngineOn = false;
        Debug.Log("ENGINE STOP");
    }
}