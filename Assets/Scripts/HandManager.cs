using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handContainer;

    private Hand playerHand = new();
    private Dictionary<Card, GameObject> cardObjectMap = new();

    private void Update()
    {
        RenderHand();
    }

    public void Draw(int count)
    {
        playerHand.Draw(count);
    }

    public HandType Submit()
    {
        Debug.Log("submit!");
        return HandType.FULL_HOUSE;
        return HandType.NONE;
    }

    public void Discard()
    {
        Debug.Log("discard,,,");
    }

    public void RenderHand()
    {
        List<Card> currentDataCards = playerHand.GetCards();
        List<Card> currentDataSelected = playerHand.GetSelected();
        List<Card> cardsToDestroy = cardObjectMap.Keys.Except(currentDataCards).ToList();

        foreach (Card card in cardsToDestroy)
        {
            if (cardObjectMap.TryGetValue(card, out GameObject uiObject))
            {
                Destroy(uiObject);
                cardObjectMap.Remove(card);
            }
        }

        foreach (Card card in currentDataCards)
        {
            if (!cardObjectMap.ContainsKey(card))
            {
                GameObject cardObj = Instantiate(cardPrefab, handContainer);

                CardDisplay display = cardObj.GetComponent<CardDisplay>();
                display.Setup(card);
                display.hand = playerHand;
                display.clicked += () => RenderHand();

                cardObjectMap.Add(card, cardObj);
            }
        }

        int i = 0;
        foreach (Card card in currentDataCards)
        {
            if (cardObjectMap.TryGetValue(card, out GameObject cardObj))
            {
                float xOffset = (i - (currentDataCards.Count - 1) / 2f) * 150;
                float yOffset = -300;

                if (currentDataSelected.Contains(card))
                    yOffset += 150;

                RectTransform cardRect = cardObj.GetComponent<RectTransform>();
                Vector2 targetPosition = new Vector2(xOffset, yOffset);

                // Smooth lerp to target position
                cardRect.anchoredPosition = Vector2.Lerp(
                    cardRect.anchoredPosition,
                    targetPosition,
                    0.25f
                );
            }
            i++;
        }
    }
}
