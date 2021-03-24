using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;
    

   // private Quaternion cameraRotation = PlayerMovement.targetRotation;
    [SerializeField]
    private float yOffset = 3f;
    [SerializeField]
    private float zOffset = -6f;
    private Quaternion baseRotation = Quaternion.Euler(12f, 0f, 0f);
    void Start()
    {
        player = GameObject.Find("Player Body").transform;
    }

    void LateUpdate()
    {
        Vector3 offsetPosition = new Vector3(0f, yOffset, zOffset);
        transform.position = (player.transform.rotation * offsetPosition) + player.position;
        transform.rotation = player.transform.rotation * baseRotation;
    }
}
