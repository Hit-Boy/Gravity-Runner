using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;
    

   // private Quaternion cameraRotation = PlayerMovement.targetRotation;
    [SerializeField]
    private float yOffset = 3f;
    [SerializeField]
    private float zOffset = -6f;
    private Vector3 beforeCheckTransform;
    private Quaternion baseRotation = Quaternion.Euler(12f, 0f, 0f);

    void Start()
    {
        player = GameObject.Find("Player Body").transform;
    }

    void LateUpdate()
    {
        Vector3 offsetPosition = new Vector3(0f, yOffset, zOffset);
        beforeCheckTransform = (player.transform.rotation * offsetPosition) + player.position;
        CheckBordersAndChangePosition();
        transform.position = beforeCheckTransform;
        transform.rotation = player.transform.rotation * baseRotation;
    }
    void CheckBordersAndChangePosition()
    {
        if (beforeCheckTransform.y >= 15)
            beforeCheckTransform.y = 15 - Constants.epsilon;
        if (beforeCheckTransform.y <= -15)
            beforeCheckTransform.y = -15 + Constants.epsilon;
        if (beforeCheckTransform.x >= 15)
            beforeCheckTransform.x = 15 - Constants.epsilon;
        if (beforeCheckTransform.x <= -15)
            beforeCheckTransform.x = -15 + Constants.epsilon;
    }
}
