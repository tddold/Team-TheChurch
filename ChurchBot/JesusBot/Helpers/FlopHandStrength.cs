using TexasHoldem.Logic;

namespace JesusBot.Helpers
{
    using System;
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;

    public static class FlopHandStrength
    {
        public static CardValuationType CalculateRisk(Card leftCard, Card rightCard, IReadOnlyCollection<Card> communityCard)
        {
            List<Card> cards = new List<Card>(communityCard);
            cards.Add(leftCard);
            cards.Add(rightCard);

            var handRankType = Helpers.GetHandRank(cards);

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
    }
}
