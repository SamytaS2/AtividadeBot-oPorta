using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayer : MonoBehaviour
{
    public int moedas = 0;
    private CommandManager myCommandManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCommandManager = new CommandManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            UndoLastCommand();
        }
        
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            //transform.position += Vector3.up;
            myCommandManager.AddCommand(new MoveUp(transform));
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            transform.position += Vector3.right;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            //moedas++;
            //Destroy(other.gameObject);
            myCommandManager.AddCommand(new GetCoin(this,other.gameObject));
        }
    }

    public void UndoLastCommand()
    {
        UndoLastCommand();
    }
}
