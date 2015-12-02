namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.Players;

    public class SmartVsSmartPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new JesusPlayer();
        private readonly IPlayer secondPlayer = new JesusPlayer();

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
