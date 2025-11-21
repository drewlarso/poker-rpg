using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public CameraController cameraController;

    public Transform player;
    public Transform enemy1;
    public Transform enemy2;
    public Transform enemy3;
    public Transform playerCameraPivot;
    public Transform enemyCameraPivot;

    public Button submitButton;
    public Button discardButton;

    private HandManager handManager;
    private HandType handType = HandType.NONE;

    void Start()
    {
        handManager = GetComponent<HandManager>();

        submitButton.onClick.AddListener(() => handType = handManager.Submit());
        discardButton.onClick.AddListener(() => handManager.Discard());

        StartCoroutine(InitCombat());
    }

    private IEnumerator InitCombat()
    {
        cameraController.lookAt = player;
        cameraController.goTo = playerCameraPivot;

        for (int i = 0; i < 7; i++)
        {
            handManager.Draw(1);
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(StartPlayerTurn());
    }

    private IEnumerator StartPlayerTurn()
    {
        cameraController.lookAt = player;
        cameraController.goTo = playerCameraPivot;

        yield return new WaitUntil(() => handType != HandType.NONE);
        Debug.Log(handType);

        // yield return new WaitUntil();
        yield return new WaitForSeconds(1);
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        Debug.Log("Player Attack");
        yield return new WaitForSeconds(1);
        StartCoroutine(EndPlayerTurn());
    }

    private IEnumerator EndPlayerTurn()
    {
        Debug.Log("End Player Turn");
        yield return new WaitForSeconds(1);
        StartCoroutine(StartEnemyTurn());
    }

    private IEnumerator StartEnemyTurn()
    {
        cameraController.lookAt = enemy1;
        cameraController.goTo = enemyCameraPivot;
        Debug.Log("Start Enemy Turn");
        yield return new WaitForSeconds(1);
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        Debug.Log("Enemy Attack");
        yield return new WaitForSeconds(1);
        StartCoroutine(EndEnemyTurn());
    }

    private IEnumerator EndEnemyTurn()
    {
        Debug.Log("End Enemy Turn");
        yield return new WaitForSeconds(1);
        StartCoroutine(StartPlayerTurn());
    }

    // Init Combat
    // Start Player Turn
    // Player Attack
    // End Player Turn
    // Start Enemy Turn
    // Enemy Attack
    // End Enemy Turn
}
