using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public RawImage image;
    public Hand hand;

    public Action clicked;
    public Card CardData { get; private set; }

    public void Setup(Card card)
    {
        CardData = card;
        string rank = card.rank.ToString();
        if (rank == "1")
            rank = "A";
        if (rank == "11")
            rank = "J";
        if (rank == "12")
            rank = "Q";
        if (rank == "13")
            rank = "K";
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        string cardName = "card" + textInfo.ToTitleCase(card.suit.ToString().ToLower()) + rank;
        Texture2D loadedTexture = Resources.Load<Texture2D>(cardName);
        if (loadedTexture != null)
        {
            image.texture = loadedTexture;
        }
        else
        {
            Debug.LogError(
                $"Card asset not found in Resources: {cardName}. Check spelling and folder location."
            );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hand.Select(CardData);
        clicked.Invoke();
    }
}
