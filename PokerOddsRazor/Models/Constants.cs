using System;
namespace PokerOddsRazor.Models
{
    public enum Rounds { isPreDeal, isPreFlop, isFlop, isTurn, isRiver, isShowdown }
    public enum PokerHand { HIGH_CARD, PAIR, TWO_PAIR, THREE_OF_A_KIND , STRAIGHT , FLUSH , FULL_HOUSE , FOUR_OF_A_KIND , STRAIGHT_FLUSH }

    public static class Constants
    {
        public static string[] POKER_HANDS = new string[]
        { "HIGH_CARD", "PAIR", "TWO_PAIR", "THREE_OF_A_KIND",
          "STRAIGHT", "FLUSH", "FULL_HOUSE", "FOUR_OF_A_KIND", "STRAIGHT_FLUSH" };

        public static int[] LOWEST_STRAIGHT = new int[] { 14, 2, 3, 4, 5 };
        public static int[] HIGHEST_STRAIGHT = new int[] { 10,11,12,13,14 };
        public static int[][] STRAIGHTS = new int[][]
        {
            HIGHEST_STRAIGHT,
            new int[] {9, 10, 11, 12, 13},
            new int[] {8, 9, 10, 11, 12},
            new int[] {7, 8, 9, 10, 11},
            new int[] {6, 7, 8, 9, 10},
            new int[] {5, 6, 7, 8, 9 },
            new int[] {4, 5, 6, 7, 8  },
            new int[] {3, 4, 5, 6, 7  },
            new int[] {2,3,4,5,6 },
            LOWEST_STRAIGHT,
        };

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
