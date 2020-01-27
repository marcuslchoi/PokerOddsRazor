using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;

//todo does this class belong here?
namespace PokerOddsRazor.Models
{
    public class TableGameMediator
    {
        public static bool newGameStarted { get; set; }
        public static Rounds CurrentRound { get; internal set; }
        public static string myCard0Id { get; internal set; }
        public static string myCard1Id { get; internal set; }
        public static List<string> tableCardIds { get; internal set; }

        public static void GetWinningChance()
        {
            Debug.WriteLine("TODO: GET WINNING CHANCE?");
            //throw new NotImplementedException();
        }
    }
}