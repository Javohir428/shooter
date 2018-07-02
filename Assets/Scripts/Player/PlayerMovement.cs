using UnityEngine;
using GeekGame.Input;
namespace CompleteProject
{
    public class PlayerMovement : MonoBehaviour
    {
         // The speed that the player will move at.

                  // Reference to the animator component.
        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.


        void Awake ()
        {
            playerRigidbody = GetComponent <Rigidbody> ();
        }


        void Update ()
        {
            Turning ();
        }



        void Turning ()
        {
            //Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));
			Vector3 turnDir = new Vector3(JoystickRotate.instance.H , 0f , JoystickRotate.instance.V);

            if (turnDir != Vector3.zero)
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotatation);
            }

        }
    }
}