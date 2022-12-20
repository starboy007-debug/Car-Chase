using UnityEngine;

public class ResetCar : MonoBehaviour
{
    // The speed at which the car should rotate when resetting its position
    public float resetRotationSpeed = 50.0f;

    // The threshold at which the car is considered flipped
    public float flipThreshold = 30.0f;

    // The position that the car should be reset to
    public Vector3 resetPosition;

    // The rotation that the car should be reset to
    public Quaternion resetRotation;

    // The input button that should be used to reset the car's position and rotation
    public string resetButton = "Reset";

   // private GameObject car;

    // Update is called once per frame
    private void Start()
    {
        
    }
    void Update()
    {
        // Check if the reset button is pressed
        if (Input.GetButtonDown(resetButton))
        {
            // Reset the car's position and rotation
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
        else
        {
            // Check if the car is flipped over
            if (transform.eulerAngles.z > flipThreshold && transform.eulerAngles.z < 180.0f)
            {
                // Rotate the car back to an upright position
                transform.Rotate(Vector3.forward * -resetRotationSpeed * Time.deltaTime);
            }
            else if (transform.eulerAngles.z < 360.0f - flipThreshold && transform.eulerAngles.z > 180.0f)
            {
                // Rotate the car back to an upright position
                transform.Rotate(Vector3.forward * resetRotationSpeed * Time.deltaTime);
            }
        }
    }
}
