﻿namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.DummyPlayer;
    using JesusPlayer;
    using TexasHoldem.Logic.Players;

    public class SmartVsDummyPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new JesusPlayer();
        private readonly IPlayer secondPlayer = new DummyPlayer();

        protected override IPlayer GetFirstPlayer()
        {
            return this.firstPlayer;
        }

        protected override IPlayer GetSecondPlayer()
        {
            return this.secondPlayer;
        }
    }
}
