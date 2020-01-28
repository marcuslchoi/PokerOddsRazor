using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;

//todo does this class belong here?
namespace PokerOddsRazor.Models
{
    //todo change this to GameState?
    public class TableGameMediator
    {
        public static bool newGameStarted { get; set; }
        private static Rounds currentRound;
        public static Rounds CurrentRound { get { return currentRound; } }
        public static string myCard0Id { get; set; }
        public static string myCard1Id { get; set; }
        public static List<string> tableCardIds { get; set; }
        public static int CardsLeftInDeck { get; private set; }
        public static int NumberOfPlayers = 1;

        public static void SetCurrentRound(MyHand myHand)
        {
            int myCardsCount = myHand.MyCardIds.Count;
            if (myCardsCount == 0)
            {
                currentRound = Rounds.isPreDeal;
            }
            else if (myCardsCount == Constants.HOLDEM_POCKETSIZE)
            {
                currentRound = Rounds.isPreFlop;
            }
            else if (myCardsCount == Constants.HOLDEM_POCKETSIZE + Constants.HOLDEM_FLOPSIZE)
            {
                currentRound = Rounds.isFlop;
            }
            else if (myCardsCount == Constants.HOLDEM_POCKETSIZE + Constants.HOLDEM_FLOPSIZE +
                Constants.HOLDEM_TURNSIZE)
            {
                currentRound = Rounds.isTurn;
            }
            else if (myCardsCount == Constants.HOLDEM_POCKETSIZE + Constants.HOLDEM_FLOPSIZE +
                Constants.HOLDEM_TURNSIZE + Constants.HOLDEM_RIVERSIZE)
            {
                currentRound = Rounds.isRiver;
            }
            else
            {
                Debug.WriteLine("Error: wrong number of cards");
                return;
            }
            SetCardsLeftInDeck();
        }

        private static void SetCardsLeftInDeck()
        {
            if (currentRound == Rounds.isPreDeal)
            {
                CardsLeftInDeck = Constants.DECK_SIZE;
            }
            if (currentRound == Rounds.isPreFlop)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - Constants.HOLDEM_POCKETSIZE * NumberOfPlayers;
            }
            else if (currentRound == Rounds.isFlop)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - Constants.HOLDEM_POCKETSIZE * NumberOfPlayers -
                    Constants.HOLDEM_FLOPSIZE;
            }
            else if (currentRound == Rounds.isTurn)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - Constants.HOLDEM_POCKETSIZE * NumberOfPlayers -
                    Constants.HOLDEM_FLOPSIZE - Constants.HOLDEM_TURNSIZE;
            }
            else if (currentRound == Rounds.isRiver)
            {
                CardsLeftInDeck = Constants.DECK_SIZE - Constants.HOLDEM_POCKETSIZE * NumberOfPlayers -
                    Constants.HOLDEM_FLOPSIZE - Constants.HOLDEM_TURNSIZE - Constants.HOLDEM_RIVERSIZE;
            }
        }

        public static void GetWinningChance()
        {
            Debug.WriteLine("TODO: GET WINNING CHANCE?");
            //throw new NotImplementedException();
        }
    }
}