using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Unit") return;

        print("bibib");
        UIManager.Instance.GameOver();
    }
}

