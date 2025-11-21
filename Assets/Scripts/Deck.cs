using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public enum Suit
{
    DIAMONDS,
    SPADES,
    CLUBS,
    HEARTS,
}

public enum HandType
{
    ROYAL_FLUSH,
    STRAIGHT_FLUSH,
    FOUR_OF_A_KIND,
    FULL_HOUSE,
    FLUSH,
    STRAIGHT,
    THREE_OF_A_KIND,
    TWO_PAIR,
    ONE_PAIR,
    HIGH_CARD,
    NONE,
}

public class Card
{
    public int rank;
    public Suit suit;

    public Card(int myRank, Suit mySuit)
    {
        rank = myRank;
        suit = mySuit;
    }
}

public class Deck
{
    public List<Card> cards = new();

    private static readonly Random random = new();

    public void Populate()
    {
        for (int i = 1; i <= 13; i++)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                Card card = new(i, suit);
                cards.Add(card);
            }
        }
    }

    public Card Draw()
    {
        if (cards.Count <= 0)
            Populate();
        int index = random.Next(0, cards.Count);
        Card card = cards[index];
        cards.RemoveAt(index);
        return card;
    }
}

public class Hand
{
    private Deck deck = new();
    private List<Card> cards = new();
    private List<Card> selected = new();

    public void Draw(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            cards.Add(deck.Draw());
        }
    }

    public List<Card> GetCards()
    {
        return cards;
    }

    public List<Card> GetSelected()
    {
        return selected;
    }

    public void Select(Card card)
    {
        if (selected.Find(c => c == card) == null)
        {
            if (selected.Count < 5)
                selected.Add(card);
        }
        else
        {
            selected.Remove(card);
        }
    }

    public void Deselect(Card card)
    {
        selected.Remove(card);
    }
}
