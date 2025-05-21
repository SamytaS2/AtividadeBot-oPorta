using System;
using UnityEngine;

public static class DoorEventChannel
{
    public static Action<string> OnButtonPressed;

    public static void RaiseEvent(string doorID)
    {
        OnButtonPressed?.Invoke(doorID);
    }
}
