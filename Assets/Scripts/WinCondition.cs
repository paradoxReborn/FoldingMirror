using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A very simple tag to be checked by GameStateManager.
public class WinCondition : MonoBehaviour
{
    public bool win = false;

    public void setTrue()
    {
        win = true;
    }

    public void setFalse()
    {
        win = false;
    }
}
