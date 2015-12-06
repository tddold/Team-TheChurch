namespace TexasHoldem.AI.JesusPlayerNew.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;

    public class CustomHandEvaluator
    {
        private const int ComparableCards = 5;

        public static CardValuationType GetStrengthOfHand(Card leftCard, Card rightCard, IEnumerable<Card> communityCards)
        {
            var cardSuitCounts = new int[(int)CardSuit.Spade + 1];
            var cardTypeCounts = new int[(int)CardType.Ace + 1];
            foreach (var card in communityCards)
            {
                cardSuitCounts[(int)card.Suit]++;
                cardTypeCounts[(int)card.Type]++;
            }

            cardSuitCounts[(int)leftCard.Suit]++;
            cardTypeCounts[(int)leftCard.Type]++;

            cardSuitCounts[(int)rightCard.Suit]++;
            cardTypeCounts[(int)rightCard.Type]++;

            // Flushes
            if (cardSuitCounts.Any(x => x >= ComparableCards))
            {
                bool isLeftCardPart;
                bool isRightCardPart;

                // Straight flush
                var straightFlushCards = GetStraightFlushCards(cardSuitCounts, communityCards, leftCard, rightCard, out isLeftCardPart, out isRightCardPart);
                if (straightFlushCards.Count > 0)
                {
                    bool isPowerStraigtFlush = straightFlushCards.Contains(CardType.Ace);

                    if (isPowerStraigtFlush && isLeftCardPart && isRightCardPart)
                    {
                        return CardValuationType.AllIn;
                    }

                    if (isPowerStraigtFlush || (isLeftCardPart && isRightCardPart))
                    {
                        return CardValuationType.VeryPowerfulHand;
                    }

                    return CardValuationType.PowerfulHand;
                }

                // Flush - it is not possible to have Flush and either Four of a kind or Full house at the same time
                for (var i = 0; i < cardSuitCounts.Length; i++)
                {
                    var suit = (CardSuit)i;

                    if (leftCard.Suit != suit || rightCard.Suit != suit)
                    {
                        continue;
                    }

                    if (cardSuitCounts[i] >= ComparableCards)
                    {
                        var cards = new List<Card>(communityCards);
                        cards.Add(leftCard);
                        cards.Add(rightCard);

                        bool isTwoPart = leftCard.Suit == rightCard.Suit;

                        var flushCards =
                            cards.Where(x => x.Suit == suit)
                                .Select(x => x.Type)
                                .OrderByDescending(x => x)
                                .Take(ComparableCards)
                                .ToList();

                        if (isTwoPart && flushCards.Contains(CardType.Ace))
                        {
                            return CardValuationType.PowerfulHand;
                        }

                        if (isTwoPart || flushCards.Contains(CardType.Ace))
                        {
                            return CardValuationType.VeryGoodHand;
                        }

                        return CardValuationType.GoodHand;
                    }
                }
            }

            // Four of a kind
            if (cardTypeCounts[(int)leftCard.Type] == 4 || cardTypeCounts[(int)rightCard.Type] == 4)
            {
                bool isTwoPart = leftCard.Type == rightCard.Type;

                if (isTwoPart && leftCard.Type > CardType.Ten)
                {
                    return CardValuationType.VeryGoodHand;
                }

                if (isTwoPart)
                {
                    return CardValuationType.GoodHand;
                }

                if (cardTypeCounts[(int)leftCard.Type] == 4 && leftCard.Type > CardType.Ten)
                {
                    return CardValuationType.GoodHand;
                }

                if (cardTypeCounts[(int)rightCard.Type] == 4 && leftCard.Type > CardType.Ten)
                {
                    return CardValuationType.GoodHand;
                }

                return CardValuationType.VeryRecommended;
            }

            // Full
            int countOfLeftType = cardTypeCounts[(int)leftCard.Type];
            int countOfRightType = cardTypeCounts[(int)rightCard.Type];

            if (countOfLeftType == 3 && countOfRightType == 3)
            {
                return CardValuationType.GoodHand;
            }

            if (countOfLeftType == 3 && countOfRightType == 2)
            {
                return CardValuationType.VeryRecommended;
            }

            if (countOfLeftType == 2 && countOfRightType == 3)
            {
                return CardValuationType.VeryRecommended;
            }

            // Straight
            var straightCards = GetStraightCards(cardTypeCounts);
            if (straightCards != null && (straightCards.Contains(leftCard.Type) || straightCards.Contains(rightCard.Type)))
            {
                return CardValuationType.VeryRecommended;
            }

            // 3 of a kind
            if (countOfLeftType > 3)
            {
                if (leftCard.Type > CardType.Ten)
                {
                    return CardValuationType.VeryRecommended;
                }

                return CardValuationType.Recommended;
            }

            // 3 of a kind
            if (countOfRightType > 3)
            {
                if (rightCard.Type > CardType.Ten)
                {
                    return CardValuationType.VeryRecommended;
                }

                return CardValuationType.Recommended;
            }

            // Two pairs
            if (countOfLeftType == 2 && countOfRightType == 2)
            {
                return CardValuationType.Risky;
            }

            // Pair
            if (countOfLeftType == 2 || countOfRightType == 2)
            {
                return CardValuationType.NotRecommended;
            }

            return CardValuationType.Unplayable;
        }

        private static ICollection<CardType> GetStraightFlushCards(int[] cardSuitCounts, IEnumerable<Card> cards, Card leftCard, Card rightCard, out bool firstCardIsPart, out bool secondCardIsPart)
        {
            firstCardIsPart = false;
            secondCardIsPart = false;
            var straightFlushCardTypes = new List<CardType>();

            for (var i = 0; i < cardSuitCounts.Length; i++)
            {
                if (cardSuitCounts[i] < ComparableCards)
                {
                    continue;
                }

                if (leftCard.Suit != (CardSuit)i && rightCard.Suit != (CardSuit)i)
                {
                    continue;
                }

                firstCardIsPart = false;
                secondCardIsPart = false;

                var cardTypeCounts = new int[(int)CardType.Ace + 1];

                if (leftCard.Suit == (CardSuit)i)
                {
                    firstCardIsPart = true;
                    cardTypeCounts[(int)leftCard.Type]++;
                }

                if (rightCard.Suit == (CardSuit)i)
                {
                    secondCardIsPart = true;
                    cardTypeCounts[(int)rightCard.Type]++;
                }

                foreach (var card in cards)
                {
                    if (card.Suit == (CardSuit)i)
                    {
                        cardTypeCounts[(int)card.Type]++;
                    }
                }

                var bestStraight = GetStraightCards(cardTypeCounts);
                if (bestStraight != null)
                {
                    straightFlushCardTypes.AddRange(bestStraight);
                }
            }

            return straightFlushCardTypes;
        }

        private static ICollection<CardType> GetStraightCards(int[] cardTypeCounts)
        {
            var lastCardType = cardTypeCounts.Length;
            var straightLength = 0;
            for (var i = cardTypeCounts.Length - 1; i >= 1; i--)
            {
                var hasCardsOfType = cardTypeCounts[i] > 0 || (i == 1 && cardTypeCounts[(int)CardType.Ace] > 0);
                if (hasCardsOfType && i == lastCardType - 1)
                {
                    straightLength++;
                    if (straightLength == ComparableCards)
                    {
                        var bestStraight = new List<CardType>(ComparableCards);
                        for (var j = i; j <= i + ComparableCards - 1; j++)
                        {
                            if (j == 1)
                            {
                                bestStraight.Add(CardType.Ace);
                            }
                            else
                            {
                                bestStraight.Add((CardType)j);
                            }
                        }

                        return bestStraight;
                    }
                }
                else
                {
                    straightLength = 0;
                }

                lastCardType = i;
            }

            return null;
        }
    }
}
