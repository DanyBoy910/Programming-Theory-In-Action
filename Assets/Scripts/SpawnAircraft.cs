using UnityEngine;

public class SpawnAircraft : MonoBehaviour
{
    public GameObject[] Aircrafts;
    public Camera mainCamera;
    private Vector3 offset_spawn = new Vector3(0, 26, -60);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int indexAircraft = GameManager.Instance.indexAircraft;
        GameObject target = Instantiate(Aircrafts[indexAircraft], transform.position + offset_spawn, Aircrafts[indexAircraft].transform.rotation);

        mainCamera.GetComponent<CameraFollow>().target = target.transform;
        mainCamera.GetComponent<CameraFollow>().SetOffset_Camera();
    }
}
