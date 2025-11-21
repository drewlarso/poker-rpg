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

    public bool selectingEnemies = false;
    public List<GameObject> selectedEnemies = new();
    public int selectAmount = 1;
    private bool doneSelectingEnemies = false;

    private HandManager handManager;
    private HandType handType = HandType.NONE;
    private bool discardedAlready = false;

    void Start()
    {
        handManager = GetComponent<HandManager>();

        submitButton.onClick.AddListener(() =>
        {
            if (selectingEnemies && selectedEnemies.Count == selectAmount)
            {
                doneSelectingEnemies = true;
            }
            else
            {
                handType = handManager.Submit();
            }
        });
        discardButton.onClick.AddListener(() =>
        {
            if (!discardedAlready)
            {
                handManager.DiscardSelected();
                handManager.ClearSelected();
                discardedAlready = true;
            }
        });

        StartCoroutine(InitCombat());
    }

    private IEnumerator InitCombat()
    {
        cameraController.lookAt = player;
        cameraController.goTo = playerCameraPivot;
        cameraController.offset = new(0, -2, 0);

        for (int i = 0; i < 10; i++)
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
        cameraController.offset = new(0, -2, 0);

        discardButton.gameObject.SetActive(true);
        discardedAlready = false;

        handManager.DrawUntil(10);

        yield return new WaitForSeconds(1);
        StartCoroutine(PlayerPickHand());
    }

    private IEnumerator PlayerPickHand()
    {
        yield return new WaitUntil(() => handType != HandType.NONE);
        Debug.Log(handType);
        selectAmount = 1;

        discardButton.gameObject.SetActive(false);

        StartCoroutine(PlayerSelectEnemy());
    }

    private IEnumerator PlayerSelectEnemy()
    {
        cameraController.lookAt = enemy1;
        cameraController.goTo = enemyCameraPivot;
        cameraController.offset = new(0, -2, 0);

        selectingEnemies = true;

        yield return new WaitUntil(() => doneSelectingEnemies);

        doneSelectingEnemies = false;
        selectingEnemies = false;
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        handManager.DiscardSelected();
        handManager.ClearSelected();

        float damage = 0f;

        switch (handType)
        {
            case HandType.ROYAL_FLUSH:
                damage = 50;
                break;
            case HandType.STRAIGHT_FLUSH:
                damage = 40;
                break;
            case HandType.FOUR_OF_A_KIND:
                damage = 40;
                break;
            case HandType.FULL_HOUSE:
                damage = 30;
                break;
            case HandType.FLUSH:
                damage = 30;
                break;
            case HandType.STRAIGHT:
                damage = 30;
                break;
            case HandType.THREE_OF_A_KIND:
                damage = 40;
                break;
            case HandType.TWO_PAIR:
                damage = 30;
                break;
            case HandType.ONE_PAIR:
                damage = 20;
                break;
            case HandType.HIGH_CARD:
                damage = 10;
                break;
            default:
                break;
        }

        foreach (GameObject fighter in selectedEnemies)
        {
            fighter.GetComponent<Fighter>().TakeDamage(damage);
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(EndPlayerTurn());
    }

    private IEnumerator EndPlayerTurn()
    {
        handType = HandType.NONE;
        selectedEnemies.Clear();
        yield return new WaitForSeconds(1);
        StartCoroutine(StartEnemyTurn());
    }

    private IEnumerator StartEnemyTurn()
    {
        cameraController.lookAt = enemy1;
        cameraController.goTo = enemyCameraPivot;
        cameraController.offset = new(0, -2, 0);
        yield return new WaitForSeconds(1);
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        player.GetComponent<Fighter>().TakeDamage(10);
        yield return new WaitForSeconds(1);
        StartCoroutine(EndEnemyTurn());
    }

    private IEnumerator EndEnemyTurn()
    {
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
