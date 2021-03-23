using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;

    [SerializeField]
    private float yOffset = 3f;
    [SerializeField]
    private float zOffset = -6f;
    void Start()
    {
        player = GameObject.Find("Player Body").transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + yOffset, player.position.z + zOffset);
    }
}
