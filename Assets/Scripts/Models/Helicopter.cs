using UnityEngine;

public class Helicopter : Aircraft
{
    private Rigidbody rb;

    [Header("Configuración del Helicóptero")]
    public float engineForce = 15f;      // Fuerza para subir
    public float moveSpeed = 5f;         // Velocidad de desplazamiento
    public float turnSpeed = 3f;         // Velocidad de giro (Cola)
    public float tiltAngle = 20f;        // Cuánto se inclina visualmente
    public float stabilizationSpeed = 2f;// Qué tan rápido se endereza

    [Header("Visuales (Opcional)")]
    public Transform mainRotor;
    public Transform tailRotor;
    public float rotorSpeed = 500f;

    // Inputs
    private float inputVertical;   // Shift/Espacio
    private float inputMoveX;      // A/D
    private float inputMoveZ;      // W/S
    private float inputYaw;        // Q/E

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // CONFIGURACIÓN FÍSICA CRUCIAL PARA HELICÓPTEROS
        // Necesitan 'frenarse' en el aire, si no se sienten como en el hielo.
        rb.linearDamping = 1.5f;
        rb.angularDamping = 2.5f;
    }

    void Update()
    {
        HandleInputs();
        HandleVisuals();
    }

    void FixedUpdate()
    {
        if (IsEngineOn)
        {
            Move();
        }
    }

    // --- IMPLEMENTACIÓN DE MÉTODOS ABSTRACTOS ---

    public override void StartEngine()
    {
        IsEngineOn = true;
        Debug.Log("Helicopter Engine Started...");
    }

    public override void StopEngine()
    {
        IsEngineOn = false;
        Debug.Log("Helicopter Engine Stopped.");
    }

    public override void Move()
    {
        // 1. GASTO DE COMBUSTIBLE (Ya que lo tienes en la clase base)
        if (FuelLevel > 0)
        {
            FuelLevel -= 0.01f; // Gasta un poco cada frame de física
        }
        else
        {
            StopEngine(); // Se acabó la gasolina
            return;
        }

        // 2. SUSTENTACIÓN (Levitar)
        // Calculamos la fuerza para contrarrestar la gravedad exacta
        float gravityCompensation = 9.81f * rb.mass;
        // Sumamos la fuerza del motor según el input (Subir/Bajar)
        float lift = gravityCompensation + (inputVertical * engineForce);

        rb.AddRelativeForce(Vector3.up * lift);

        // 3. MOVIMIENTO (Adelante/Atrás/Lados)
        // Aplicamos fuerza según hacia donde nos inclinamos
        Vector3 movement = (transform.right * inputMoveX) + (transform.forward * inputMoveZ);
        rb.AddForce(movement * moveSpeed, ForceMode.Acceleration);

        // 4. ROTACIÓN (Yaw/Cola)
        rb.AddRelativeTorque(Vector3.up * inputYaw * turnSpeed, ForceMode.Acceleration);

        // 5. ESTABILIZACIÓN VISUAL (Tilt)
        // Esto inclina el modelo para que se vea realista, pero mantiene el control estable
        ApplyStabilization();
    }

    // --- MÉTODOS PRIVADOS DE AYUDA ---

    private void HandleInputs()
    {
        // Encendido/Apagado
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsEngineOn) StopEngine();
            else StartEngine();
        }

        if (!IsEngineOn) return;

        inputMoveX = Input.GetAxis("Vertical");
        inputMoveZ = Input.GetAxis("Horizontal") * -1;

        // Control de altura (Espacio / Shift)
        inputVertical = 0f;
        if (Input.GetKey(KeyCode.Space)) inputVertical = 1f;
        else if (Input.GetKey(KeyCode.LeftShift)) inputVertical = -1f;

        // Control de giro (Q / E)
        inputYaw = 0f;
        if (Input.GetKey(KeyCode.Q)) inputYaw = -1f;
        else if (Input.GetKey(KeyCode.E)) inputYaw = 1f;
    }

    private void ApplyStabilization()
    {
        // Calculamos la inclinación objetivo basada en el movimiento
        float targetRoll = -inputMoveX * tiltAngle;
        float targetPitch = inputMoveZ * tiltAngle;

        // Mantenemos la rotación Y actual (hacia donde mira la nariz), pero ajustamos X y Z
        Quaternion currentYaw = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Quaternion targetRotation = currentYaw * Quaternion.Euler(targetPitch, 0, targetRoll);

        // Suavizamos la rotación para que no sea brusca
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, stabilizationSpeed * Time.fixedDeltaTime);
    }

    private void HandleVisuals()
    {
        // Rotar aspas si el motor está encendido
        if (IsEngineOn)
        {
            if (mainRotor) mainRotor.Rotate(0, rotorSpeed * Time.deltaTime, 0);
            if (tailRotor) tailRotor.Rotate(0, rotorSpeed * Time.deltaTime, 0);
        }
    }
}