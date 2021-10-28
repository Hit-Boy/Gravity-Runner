using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // private Quaternion cameraRotation = PlayerMovement.targetRotation;
    [SerializeField] private float yOffset = 3f;

    [SerializeField] private float zOffset = -6f;

    private readonly Quaternion baseRotation = Quaternion.Euler(12f, 0f, 0f);
    private Vector3 beforeCheckTransform;
    private Transform player;

    private void Start()
    {
        player = GameObject.Find("Player Body").transform;
    }

    private void LateUpdate()
    {
        var offsetPosition = new Vector3(0f, yOffset, zOffset);
        beforeCheckTransform = player.transform.rotation * offsetPosition + player.position;
        CheckBordersAndChangePosition();
        transform.position = beforeCheckTransform;
        transform.rotation = player.transform.rotation * baseRotation;
    }

    private void CheckBordersAndChangePosition()
    {
        if (beforeCheckTransform.y >= 15)
            beforeCheckTransform.y = 15 - Constants.Epsilon;
        if (beforeCheckTransform.y <= -15)
            beforeCheckTransform.y = -15 + Constants.Epsilon;
        if (beforeCheckTransform.x >= 15)
            beforeCheckTransform.x = 15 - Constants.Epsilon;
        if (beforeCheckTransform.x <= -15)
            beforeCheckTransform.x = -15 + Constants.Epsilon;
    }
}