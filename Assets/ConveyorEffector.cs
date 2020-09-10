using UnityEngine;

public class ConveyorEffector : MonoBehaviour
{
    // Make sure this has a downward direction too, so that the CharacterController stays grounded.
    public Vector3 effectDirection = Vector3.forward;

    private void OnTriggerStay(Collider other)
    {
        // For player, use character controller to move; otherwise, just use transform.position.
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().Move(effectDirection * Time.fixedDeltaTime);
        }
        else
        {
            // For everything else, use Rigidbody if it has one, otherwise just move the position of the object.
            // TODO: Probably want to change how broadly this works at some point or our conveyor belt is going to be scooting around pieces of environment.
            var otherRigidbody = other.GetComponent<Rigidbody>();
            if (otherRigidbody)
            {
                var currentPosition = other.transform.position;
                otherRigidbody.MovePosition(currentPosition + (effectDirection * Time.fixedDeltaTime));
            }
            else
            {
                other.transform.Translate(effectDirection * Time.fixedDeltaTime);
            }
        }
    }
}
