using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    public string doorID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DoorEventChannel.RaiseEvent(doorID);
            Debug.Log($"Button pressed: {doorID}");
        }
    }
}
