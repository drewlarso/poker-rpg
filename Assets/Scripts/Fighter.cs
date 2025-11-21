using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fighter : MonoBehaviour
{
    public Canvas healthBarCanvas;
    public UnityEngine.UI.Image healthBar;
    public CombatManager combatManager;

    public Action Die;

    public bool isEnemy = true;
    public bool selected = false;
    public bool hoveringOver = false;
    public float maxHealth = 100f;
    public float health;
    private Renderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        health = maxHealth;
    }

    private void OnMouseOver()
    {
        hoveringOver = true;
    }

    private void OnMouseExit()
    {
        hoveringOver = false;
    }

    private void Update()
    {
        myRenderer.material.color = selected ? Color.red : Color.white;

        if (healthBarCanvas)
            healthBarCanvas.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 10));

        if (!combatManager.selectedEnemies.Contains(gameObject))
            selected = false;

        if (!isEnemy)
            return;

        if (!combatManager.selectingEnemies)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (hoveringOver)
                    {
                        if (selected)
                        {
                            myRenderer.material.color = Color.white;
                            combatManager.selectedEnemies.Remove(gameObject);
                            selected = false;
                        }
                        else
                        {
                            if (combatManager.selectedEnemies.Count < combatManager.selectAmount)
                            {
                                myRenderer.material.color = Color.red;
                                combatManager.selectedEnemies.Add(gameObject);
                                selected = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health = Math.Max(health - amount, 0);
        healthBar.fillAmount = health / maxHealth;
    }
}
