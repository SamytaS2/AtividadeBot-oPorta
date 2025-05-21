using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorID;
    private Collider2D doorCollider;

    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        DoorEventChannel.OnButtonPressed += OpenDoorIfMatch;
    }

    private void OnDisable()
    {
        DoorEventChannel.OnButtonPressed -= OpenDoorIfMatch;
    }

    private void OpenDoorIfMatch(string triggeredID)
    {
        if (triggeredID == doorID)
        {
            Debug.Log($"Door {doorID} opened");
            doorCollider.enabled = false; // Porta se "abre"
        }
    }
}