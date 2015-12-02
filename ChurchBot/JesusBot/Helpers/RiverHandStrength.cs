namespace JesusBot.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;

    public static class RiverHandStrength
    {
        public static CardValuationType CalculateRisk(Card leftCard, Card rightCard, IReadOnlyCollection<Card> communityCard)
        {
            var communityCards = communityCard.ToList();
            var handRankType = HandRankType.HighCard;

            var current = Get(leftCard, rightCard, communityCards[0], communityCards[1], communityCards[2]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[0], communityCards[1], communityCards[3]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[0], communityCards[2], communityCards[3]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[0], communityCards[2], communityCards[4]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[0], communityCards[3], communityCards[4]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[1], communityCards[2], communityCards[3]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[1], communityCards[2], communityCards[4]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[1], communityCards[3], communityCards[4]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            current = Get(leftCard, rightCard, communityCards[2], communityCards[3], communityCards[4]);

            if (handRankType < current)
            {
                handRankType = current;
            }

            switch (handRankType)
            {
                case HandRankType.Pair:
                    return CardValuationType.Risky;
                case HandRankType.TwoPairs:
                    return CardValuationType.Recommended;
                case HandRankType.ThreeOfAKind:
                case HandRankType.Straight:
                    return CardValuationType.VeryRecommended;
                case HandRankType.Flush:
                case HandRankType.FullHouse:
                case HandRankType.FourOfAKind:
                    return CardValuationType.VeryPowerful;
                case HandRankType.StraightFlush:
                    return CardValuationType.AllIn;
                default:
                    return CardValuationType.Unplayable;
            }
        }

        private static HandRankType Get(params Card[] cards)
        {
            return Helpers.GetHandRank(cards);
        }
    }
}
