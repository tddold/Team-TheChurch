namespace JesusBot
{
    using System;
    using TexasHoldem.Logic.Players;

    public class Jesus : BasePlayer
    {
        public override string Name
        {
            get
            {
                return "Jesus Christ";
            }
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return PlayerAction.CheckOrCall();
        }
    }
}
