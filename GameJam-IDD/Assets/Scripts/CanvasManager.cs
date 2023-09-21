using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject canvasWin;

    public void ShowingWinCondition(Component sender, object data)
    {
        StartCoroutine(WaitWinCondition());
    }
    IEnumerator WaitWinCondition()
    {
        yield return new WaitForSeconds(2.0f);
        canvasWin.SetActive(true);
    }
}
