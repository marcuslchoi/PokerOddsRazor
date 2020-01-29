using System;
namespace PokerOddsRazor.Models
{
    public enum Rounds { isPreDeal, isPreFlop, isFlop, isTurn, isRiver, isShowdown }

    public static class Constants
    {
        public static string[] POKER_HANDS = new string[]
        { "HIGH_CARD", "PAIR", "TWO_PAIR", "THREE_OF_A_KIND",
          "STRAIGHT", "FLUSH", "FULL_HOUSE", "FOUR_OF_A_KIND", "STRAIGHT_FLUSH" };

        public static int HOLDEM_POCKETSIZE = 2;
        public static int HOLDEM_FLOPSIZE = 3;
        public static int HOLDEM_TURNSIZE = 1;
        public static int HOLDEM_RIVERSIZE = 1;
        public static int DECK_SIZE = 52;

        public static double ConvertToPercent(double probability)
        {
            var percent = probability * 100;
            percent = Math.Round(percent, 2);
            return percent;
        }
    }
}
