namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.DummyPlayer;
    using JesusPlayer;
    using TexasHoldem.Logic.Players;

    internal class SmartVsAlwaysCallPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new JesusPlayer();

        private readonly IPlayer secondPlayer = new AlwaysCallDummyPlayer();

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
