using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 10f;
    private PlayerMotor motor;
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");
        Vector3 horizontalMove = Vector3.right * xMove;
        Vector3 verticalMove = Vector3.forward * zMove;
        //Movement Vector
        Vector3 velocity = (horizontalMove + verticalMove).normalized * speed;
        //Apply movement
        motor.Move(velocity);
        //Player rotation with mouse
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        //Apply rotation
        motor.Rotate(rotation);
        //Player camera rotation with mouse
        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRot, 0f, 0f) * lookSensitivity;
        //Apply rotation
        motor.RotateCamera(cameraRotation);
    }
}
