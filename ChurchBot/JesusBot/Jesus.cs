namespace JesusBot
{
    using Helpers;
    using System;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class Jesus : BasePlayer
    {
        public override string Name
        {
            get
            {
                return "Jesus Christ" + Guid.NewGuid();
            }
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
                if (playHand == CardValuationType.Unplayable)
                {
                    return GetUnplayableAction(context);
                }

                if (playHand == CardValuationType.Risky)
                {
                    var smallBlindsTimes = RandomProvider.Next(1, 8);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationType.Recommended)
                {
                    var smallBlindsTimes = RandomProvider.Next(6, 14);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }

            if (context.RoundType == GameRoundType.Flop)
            {
                return this.GetFlopAction(context);
            }

            if (context.RoundType == GameRoundType.Turn)
            {
                return this.GetTurnAction(context);
            }

            if (context.RoundType == GameRoundType.River)
            {
                return this.GetRiverAction(context);
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction GetFlopAction(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Flop(this.FirstCard, this.SecondCard, this.CommunityCards);

            return GetActionByCardValuationType(playHand, context);
        }

        private PlayerAction GetTurnAction(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.Turn(this.FirstCard, this.SecondCard, this.CommunityCards);

            return GetActionByCardValuationType(playHand, context);
        }

        private PlayerAction GetRiverAction(GetTurnContext context)
        {
            var playHand = HandStrengthValuation.River(this.FirstCard, this.SecondCard, this.CommunityCards);

            return GetActionByCardValuationType(playHand, context);
        }

        private PlayerAction GetActionByCardValuationType(CardValuationType playHand, GetTurnContext context)
        {
            if (playHand == CardValuationType.Unplayable)
            {
                return GetUnplayableAction(context);
            }

            if (playHand == CardValuationType.Risky)
            {
                return ChooseAction(context.MoneyLeft, 0.1f);
            }

            if (playHand == CardValuationType.Recommended)
            {
                return ChooseAction(context.MoneyLeft, 0.25f);
            }

            if (playHand == CardValuationType.VeryPowerful)
            {
                return ChooseAction(context.MoneyLeft, 0.5f);
            }

            if (playHand == CardValuationType.AllIn)
            {
                return ChooseAction(context.MoneyLeft, 1f);
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction GetUnplayableAction(GetTurnContext context)
        {
            if (context.CanCheck)
            {
                return PlayerAction.CheckOrCall();
            }
            else
            {
                return PlayerAction.Fold();
            }
        }

        private PlayerAction ChooseAction(int leftMoney, float percent)
        {
            var bet = GetBet(leftMoney, percent);

            if (bet == 0)
            {
                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.Raise(bet);
        }

        private int GetBet(int leftMoney, float percent)
        {
            var wantBet = (int) (leftMoney * percent) + 1;

            if (wantBet < leftMoney)
            {
                return wantBet;
            }

            return 0;
        }
    }
}
