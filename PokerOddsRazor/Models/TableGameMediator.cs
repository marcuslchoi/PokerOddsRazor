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
        private static Rounds currentRound;
        public static Rounds CurrentRound
        {
            get
            {
                return currentRound;
            }
            set
            {
                currentRound = value;
                SetCardsLeftInDeck(value);
            }
        }
        public static string myCard0Id { get; set; }
        public static string myCard1Id { get; set; }
        public static List<string> tableCardIds { get; set; }
        public static int CardsLeftInDeck { get; private set; }
        public static int NumberOfPlayers = 1;

        private static void SetCardsLeftInDeck(Rounds round)
        {
            if (round == Rounds.isPreFlop)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - 2 * NumberOfPlayers;
            }
            else if (round == Rounds.isFlop)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - 2 * NumberOfPlayers - Constants.HOLDEM_FLOPSIZE;
            }
            else if (round == Rounds.isTurn)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - 2 * NumberOfPlayers - Constants.HOLDEM_FLOPSIZE - 1;
            }
        }

        public static void GetWinningChance()
        {
            Debug.WriteLine("TODO: GET WINNING CHANCE?");
            //throw new NotImplementedException();
        }
    }
}