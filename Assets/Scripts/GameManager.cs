using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int indexAircraft; // 0 = helicopter | 1 = Airplane
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {

        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
