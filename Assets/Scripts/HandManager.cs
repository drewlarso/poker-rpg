using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public float cardSpacing = 100f;
    public float fanAngle = 10f;
    public float fanDepth = 5f;
    public float lerpSpeed = 0.05f;
    public float selectedCardHeight = 100f;

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

    public void DrawUntil(int count)
    {
        int currentCardCount = playerHand.GetCards().Count;
        if (count <= currentCardCount)
            return;

        playerHand.Draw(count - playerHand.GetCards().Count);
    }

    public void ClearSelected()
    {
        playerHand.ClearSelected();
    }

    public void DiscardSelected()
    {
        playerHand.DiscardSelected();
    }

    private static bool IsStraight(List<int> ranks)
    {
        var sorted = ranks.Distinct().OrderBy(r => r).ToList();
        if (sorted.Count < 3)
            return false;

        if (
            sorted.Count == 5
            && sorted[0] == 1
            && sorted[1] == 10
            && sorted[2] == 11
            && sorted[3] == 12
            && sorted[4] == 13
        )
            return true;

        for (int i = 0; i < sorted.Count - 1; i++)
        {
            if (sorted[i + 1] != sorted[i] + 1)
                return false;
        }
        return true;
    }

    private static bool IsRoyalFlush(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;
        if (!cards.All(c => c.suit == cards[0].suit))
            return false;
        var ranks = cards.Select(c => c.rank).OrderBy(r => r).ToList();
        return ranks.SequenceEqual(new List<int> { 1, 10, 11, 12, 13 });
    }

    public HandType Submit()
    {
        List<Card> cards = playerHand.GetSelected();
        if (cards == null || cards.Count == 0)
            return HandType.NONE;

        var rankCounts = cards.GroupBy(c => c.rank).ToDictionary(g => g.Key, g => g.Count());
        var counts = rankCounts.Values.OrderByDescending(c => c).ToList();

        bool isFlush = cards.Count >= 3 && cards.All(c => c.suit == cards[0].suit);
        bool isStraight = cards.Count >= 3 && IsStraight(cards.Select(c => c.rank).ToList());

        if (IsRoyalFlush(cards))
            return HandType.ROYAL_FLUSH;

        if (isStraight && isFlush)
            return HandType.STRAIGHT_FLUSH;

        if (counts[0] == 4)
            return HandType.FOUR_OF_A_KIND;

        if (counts[0] == 3 && counts.Count > 1 && counts[1] == 2)
            return HandType.FULL_HOUSE;

        if (isFlush)
            return HandType.FLUSH;

        if (isStraight)
            return HandType.STRAIGHT;

        if (counts[0] == 3)
            return HandType.THREE_OF_A_KIND;

        if (counts[0] == 2 && counts.Count > 1 && counts[1] == 2)
            return HandType.TWO_PAIR;

        if (counts[0] == 2)
            return HandType.ONE_PAIR;

        return HandType.HIGH_CARD;
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

        int numCards = currentDataCards.Count;
        int i = 0;
        foreach (Card card in currentDataCards)
        {
            if (cardObjectMap.TryGetValue(card, out GameObject cardObj))
            {
                float normalizedPos = i - (numCards - 1) / 2f;
                float xOffset = normalizedPos * cardSpacing;
                float targetRotation = -normalizedPos * fanAngle;
                float arcYOffset = Mathf.Pow(normalizedPos, 2) * fanDepth;
                float baseYOffset = -300f;
                float yOffset = baseYOffset - arcYOffset;

                if (currentDataSelected.Contains(card))
                    yOffset += selectedCardHeight;

                RectTransform cardRect = cardObj.GetComponent<RectTransform>();

                Vector2 targetPosition = new Vector2(xOffset, yOffset);
                Quaternion targetRotationQuaternion = Quaternion.Euler(0, 0, targetRotation);

                cardRect.anchoredPosition = Vector2.Lerp(
                    cardRect.anchoredPosition,
                    targetPosition,
                    lerpSpeed
                );
                cardRect.localRotation = Quaternion.Lerp(
                    cardRect.localRotation,
                    targetRotationQuaternion,
                    lerpSpeed
                );
            }
            i++;
        }
    }
}
