using System;
namespace PokerOddsRazor.Models
{
    public enum Rounds { isPreDeal, isPreFlop, isFlop, isTurn, isRiver, isShowdown }

    public static class Constants
    {
        public static string[] POKER_HANDS = new string[]
        { "HIGH_CARD", "PAIR", "TWO_PAIR", "THREE_OF_A_KIND",
          "STRAIGHT", "FLUSH", "FULL_HOUSE", "FOUR_OF_A_KIND", "STRAIGHT_FLUSH" };

        public static int HOLDEM_FLOPSIZE = 3;
    }
}
