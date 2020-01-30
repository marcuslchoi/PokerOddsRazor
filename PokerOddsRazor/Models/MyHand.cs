using System.Web;
//using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System;

namespace PokerOddsRazor.Models
{
    public class MyHand
    {
        private string myCard0Id;
        public string MyCard0Id
        {
            get { return myCard0Id; }
            set
            {
                myCard0Id = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }
        private string myCard1Id;
        public string MyCard1Id
        {
            get { return myCard1Id; }
            set
            {
                myCard1Id = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }
        private string flop0;
        public string Flop0
        {
            get { return flop0; }
            set
            {
                flop0 = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }
        private string flop1;
        public string Flop1
        {
            get { return flop1; }
            set
            {
                flop1 = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }
        private string flop2;
        public string Flop2
        {
            get { return flop2; }
            set
            {
                flop2 = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }

        private string turn;
        public string Turn
        {
            get { return this.turn; }
            set
            {
                this.turn = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }
        private string river;
        public string River
        {
            get { return this.river; }
            set
            {
                this.river = value;
                this.PossiblyAddToMyCardIds(value);
            }
        }

        public List<string> myPocketCardIds = new List<string>();
        public List<string> tableCardIds = new List<string>();

        //my pocket hand and table cards combined
        private List<string> myCardIds = new List<string>();
        public List<string> MyCardIds { get { return this.myCardIds; } }

        //has 4 of 5 cards to get a straight
        public bool isAlmostStraight = false;

        //used for royal flush possibility
        public bool isAlmostHighStraight = false;

        //CHANGED
        //3 cards present in a possible straight
        public bool is3AlmostStraight = false;
        public bool is3AlmostHighStraight = false;

        public string fourFlushSuit = null;

        //CHANGED
        public string threeFlushSuit = null;

        //initializer
        public MyHand(List<string> cardList)
        {
            myCardIds = cardList;

        }

        public MyHand()
        {
        }

        private bool PossiblyAddToMyCardIds(string card)
        {
            if (card != null && !this.myCardIds.Contains(card))
            {
                this.myCardIds.Add(card);
                return true;
            }
            else
            {
                Debug.WriteLine("Card not added: " + card);
                return false;
            }
        }

        public List<string> GetPocketSuits()
        {
            var pocketSuits = new List<string>();
            pocketSuits.Add(this.MyCard0Id.Substring(this.MyCard0Id.Length - 1));
            pocketSuits.Add(this.MyCard1Id.Substring(this.MyCard1Id.Length - 1));
            return pocketSuits;
        }

        public bool PocketCardsAreSuited()
        {
            var pocketSuits = this.GetPocketSuits();
            return pocketSuits[0] == pocketSuits[1];
        }

        public List<int> GetPocketCardRanksHighToLow()
        {
            var pocketCards = new List<string> { this.MyCard0Id, this.MyCard1Id };
            return this.GetCardRanksHighToLow(pocketCards);
        }

        //todo call this once!
        //returns the list of cards as just int ranks
        private List<int> GetCardRanksHighToLow(List<string> cardsToBeIntRanked)
        {
            List<int> myCardRanks = new List<int>();

            for (int i = 0; i < cardsToBeIntRanked.Count; i++)
            {
                string cardSuitless = cardsToBeIntRanked[i];

                //remove last character from string (the suit)
                cardSuitless = cardSuitless.Substring(0, cardSuitless.Length - 1);

                //changing suitless face cards to a number for ranking. Used to check if numbers are in order
                if (cardSuitless == "J")
                {
                    cardSuitless = "11";
                }
                else if (cardSuitless == "Q")
                {
                    cardSuitless = "12";
                }
                else if (cardSuitless == "K")
                {
                    cardSuitless = "13";

                }
                else if (cardSuitless == "A")
                {

                    cardSuitless = "14";
                }

                int cardRankInt = int.Parse(cardSuitless);
                myCardRanks.Add(cardRankInt);
            }

            myCardRanks.Sort();
            myCardRanks.Reverse();

            return myCardRanks;
        }

        //get the flush suit. If not flush, returns null
        string getFlushSuit()
        {
            //dictionary showing how many of each card suit there is		
            Dictionary<string, int> suitCounts = new Dictionary<string, int>();
            List<string> cardsSuit = new List<string>();

            //create a list of the cards suit only
            for (var i = 0; i < myCardIds.Count; i++)
            {
                string cardSuit = myCardIds[i];

                cardSuit = cardSuit.Substring(cardSuit.Length - 1);

                cardsSuit.Add(cardSuit);
            }

            //sort the suits alphabetically
            cardsSuit.Sort();

            //hashset of the suits present in hand (unique)
            HashSet<string> suitsUnique = new HashSet<string>(cardsSuit);

            string flushSuit = null;

            //create dictionary of suit:number in hand
            //for each suit present, loop through array of playerCardsSuit and count how many of each suit 		
            foreach (string suit in suitsUnique)
            {
                int count = 0;

                for (int j = 0; j < cardsSuit.Count; j++)
                {
                    if (cardsSuit[j] == suit)
                    {
                        count++;
                    }
                }

                suitCounts.Add(suit, count);

                //flush exists, get the flush suit
                if (count >= 5)
                {
                    //isFlush = true;
                    flushSuit = suit;

                    //need this for straight flush possibilities
                    fourFlushSuit = suit;

                    //CHANGED
                    threeFlushSuit = suit;

                }
                //if there are exactly 4 flush cards
                else if (count == 4)
                {

                    fourFlushSuit = suit;

                    //CHANGED
                    threeFlushSuit = suit;

                }
                //CHANGED
                //if there are exactly 3 flush cards
                else if (count == 3)
                {

                    threeFlushSuit = suit;

                }

                //reset count when going to a different suit
                count = 0;
            }

            return flushSuit;
        }


        double checkForMultiples()
        {

            //FIRST CHECKS FOR QUAD, THEN FULL HOUSE, THEN TRIPLE, THEN 2 PAIR, THEN 1 PAIR (THEN HIGH CARD IF EVERYTHING FALSE)

            //put cards into array of suitless int values
            List<int> myCardRanks = GetCardRanksHighToLow(myCardIds);

            //sort rank values in high to low order
            myCardRanks.Sort();
            myCardRanks.Reverse();

            //get unique values of ranks and put in HashSet
            var intRanksUnique = new HashSet<int>(myCardRanks);

            bool isTwoOfAKind = false;
            bool isThreeOfAKind = false;
            bool isFourOfAKind = false;
            bool isFullHouse = false;
            bool isTwoPair = false;

            int numberOfPairs = 0;
            int numberOfTriples = 0;

            //the ranks (independent of suit) of cards in pairs, triples and quads as Int
            List<int> pairValues = new List<int>();
            List<int> tripleValues = new List<int>();
            int quadValue = new int();

            //create dictionary of int rank:number in hand
            Dictionary<int, int> counts = new Dictionary<int, int>();

            //for each rank present, loop through list of myCardRanks and count how many of each rank 		
            foreach (int rank in intRanksUnique)
            {
                int count = 0;

                for (int j = 0; j < myCardRanks.Count; j++)
                {
                    //only works with sorted array
                    if (myCardRanks[j] == rank)
                    {
                        count++;
                    }
                }

                //assign key:value pair to dictionary
                counts.Add(rank, count);

                if (count == 2)
                {
                    isTwoOfAKind = true;

                    pairValues.Add(rank);
                    //alert("Pair Values: "+pairValues);

                    numberOfPairs++;
                    //alert("Number of pairs: "+numberOfPairs)

                    //IF NUMBER OF PAIRS >= 2, two pair is true
                    if (numberOfPairs >= 2)
                    {
                        isTwoPair = true;
                    }
                }
                else if (count == 3)
                {
                    isThreeOfAKind = true;
                    numberOfTriples++;
                    tripleValues.Add(rank);

                    //alert("Number of 3 of a kinds: "+numberOfTriples);

                    //IF NUMBER OF TRIPLES (3 OF A KIND) IS 2, FULL HOUSE
                    if (numberOfTriples == 2)
                    {
                        isFullHouse = true;
                        //alert("Full House! 2 triples");
                    }
                }
                else if (count == 4)
                {
                    isFourOfAKind = true;
                    quadValue = rank;
                    //alert("Four of a kind!");
                }

                //reset count when going to a different rank
                count = 0;
            }   //END FOR LOOP FOR EACH RANK

            //FULL HOUSE
            if (isTwoOfAKind && isThreeOfAKind)
            {
                isFullHouse = true;
                //Debug.WriteLine("full house");

            }

            //============CHECKING DIFFERENT CASES AND ASSIGNING HANDRANK======================

            //need this declaration inside of function to use it
            //var pokerHandsArray = ["HIGH_CARD","PAIR","TWO_PAIR","THREE_OF_A_KIND","STRAIGHT","FLUSH","FULL_HOUSE","FOUR_OF_A_KIND","STRAIGHT_FLUSH"];

            double handRank = -1;

            double divisor = 100f;
            if (isFourOfAKind)
            {

                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "FOUR_OF_A_KIND");
                handRank += quadValue / divisor;

                divisor *= 100;

                //int counter = 0;
                //add the next highest card after the four-of-a-kind value to handRank
                foreach (int rank in intRanksUnique)
                {
                    if (rank != quadValue)
                    {
                        handRank += rank / divisor;
                        //counter++;
                        break;
                    }
                }

                //handRank = handRank.toFixed(2 + 2*counter);

                Debug.WriteLine("4kind hand rank: " + handRank);

                //alert("four of a kind " + handRank);

            }
            else if (isFullHouse)
            {
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "FULL_HOUSE");

                //CASE 2 TRIPLES
                if (numberOfTriples == 2)
                {
                    //use higher triple for 3 of a kind, other triple becomes the pair
                    foreach (int tripleValue in tripleValues)
                    {
                        handRank += tripleValue / divisor;

                        divisor *= 100;
                    }
                }
                else //triple and double
                {
                    //append the three of a kind value, then the highest pair value
                    handRank += tripleValues[0] / divisor;
                    divisor *= 100;
                    handRank += pairValues[0] / divisor;
                }

                //handRank = handRank.toFixed(4);

                //alert("full house: "+handRank);
            }
            else if (isThreeOfAKind)
            {
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "THREE_OF_A_KIND");
                handRank += tripleValues[0] / divisor;

                divisor *= 100;

                int counter = 0;
                //add the next 2 highest cards after the three-of-a-kind value
                foreach (int rank in intRanksUnique)
                {
                    if (rank != tripleValues[0])
                    {
                        handRank += rank / divisor;
                        divisor *= 100;
                        counter++;
                    }

                    //counter represents the number of cards added to handRank after the triple
                    if (counter == 2)
                    {
                        break;
                    }
                }

                //handRank = handRank.toFixed(2 + 2*counter);

                //alert("Three of a kind: "+handRank);
            }
            else if (isTwoPair)
            {
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "TWO_PAIR");

                //if there are 3 pair values, remove lowest pair
                if (pairValues.Count == 3)
                {

                    pairValues.RemoveAt(pairValues.Count - 1);

                }

                //add the 2 pair values to handRank
                foreach (int pairValue in pairValues)
                {
                    handRank += pairValue / divisor;
                    divisor *= 100;

                }

                //find the highest 5th card in hand that is not one of the pairs in pairValues, add to handRank
                foreach (int rank in intRanksUnique)
                {
                    if (!pairValues.Contains(rank))
                    {
                        handRank += rank / divisor;
                        //counter++;
                        break;
                    }
                }

                //handRank = handRank.toFixed(4 + 2*counter);

                //alert("2 pair: "+handRank);
            }
            else if (isTwoOfAKind)
            {
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "PAIR");

                //add pair value to handRank
                handRank += pairValues[0] / divisor;

                divisor *= 100;

                int counter = 0;

                //add values of 3 highest non-pair cards (in diminishing order) to handRank
                foreach (int rank in intRanksUnique)
                {
                    if (!pairValues.Contains(rank))
                    {
                        handRank += rank / divisor;
                        divisor *= 100;
                        counter++;
                    }

                    //counter represents the number of cards added after the only pair
                    if (counter == 3)
                    {
                        break;
                    }
                }

                //handRank = handRank.toFixed(2 + 2*counter);

                //alert("pair: "+handRank);
            }
            else //HIGH_CARD
            {
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "HIGH_CARD");

                int counter = 0;
                //add values of up to 5 highest cards from highest to lowest to handRank
                foreach (int rank in intRanksUnique)
                {
                    handRank += rank / divisor;
                    divisor *= 100;
                    counter++;

                    if (counter == 5)
                    {
                        break;
                    }
                }

                //handRank = handRank.toFixed(2*counter);
                //alert("high card: "+handRank);
            }

            return handRank;

        }//CLOSE CHECKFORMULTIPLES FUNCTION

        public List<string> getFlushCards(string flushSuit)
        {
            //initialize flush list as full player cards list, then remove non-flush cards
            List<string> flushCards = new List<string>(); //myCardIds;
            flushCards.AddRange(myCardIds);

            //remove non-flush cards from flushCards. Must go through in reverse            
            for (int i = flushCards.Count - 1; i >= 0; i--)
            {
                string cardSuit = flushCards[i];
                cardSuit = cardSuit.Substring(cardSuit.Length - 1);

                if (cardSuit != flushSuit)
                {
                    flushCards.RemoveAt(i);
                }
            }

            return flushCards;

        }

        //CHECK FOR FLUSH
        double checkForFlush()
        {

            bool isFlush = false;
            string flushSuit = getFlushSuit();

            if (!string.IsNullOrEmpty(flushSuit))
            {
                isFlush = true;
            }

            double handRank = -1;

            //eliminate non-flush cards from hand, put the rest in highest to lowest order, use top 5 in handRank
            if (isFlush)
            {
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "FLUSH");

                //list of the flush cards.
                List<string> flushCards = getFlushCards(flushSuit);

                //list of Int ranks of the flush cards. Put in high-to-low order
                List<int> flushCardRanks = GetCardRanksHighToLow(flushCards);
                flushCardRanks.Sort();
                flushCardRanks.Reverse();

                //divide the Int value by this number to add decimal amount to handRank
                double divisor = 100;

                int count = 0;
                //add values of ordered flush cards to hand rank. Only top 5 cards
                foreach (int rank in flushCardRanks)
                {
                    count++;
                    handRank += rank / divisor;
                    divisor *= 100;

                    if (count == 5)
                    {
                        break;
                    }
                }

            }

            return handRank;
        }

        //CHECK FOR STRAIGHT	
        //CHANGED
        public int numberOfPossStraightsWithMy3Cards = 0;
        public int numberOfPossStraightsWithMy4Cards = 0;
        bool checkForStraightCalledOnFlop = false;
        bool checkForStraightCalledOnTurn = false;
        public double checkForStraight()
        {

            List<int> myCardRanks = GetCardRanksHighToLow(myCardIds);

            //sort rank values in high to low order
            myCardRanks.Sort();
            myCardRanks.Reverse();

            //get unique values of ranks and put in HashSet
            HashSet<int> intRanksUnique = new HashSet<int>(myCardRanks);

            bool isStraight = false;

            double handRank = -1;

            //LOWEST STRAIGHT CASE: Ace becomes low card
            List<int> lowestStraight = new List<int> { 14, 2, 3, 4, 5 };

            //List<int> straight6 = new List<int> {2,3,4,5,6};

            List<List<int>> straights = new List<List<int>>();

            //all the possible straights from highest to lowest
            straights.Add(new List<int> { 10, 11, 12, 13, 14 });
            straights.Add(new List<int> { 9, 10, 11, 12, 13 });
            straights.Add(new List<int> { 8, 9, 10, 11, 12 });
            straights.Add(new List<int> { 7, 8, 9, 10, 11 });
            straights.Add(new List<int> { 6, 7, 8, 9, 10 });
            straights.Add(new List<int> { 5, 6, 7, 8, 9 });
            straights.Add(new List<int> { 4, 5, 6, 7, 8 });
            straights.Add(new List<int> { 3, 4, 5, 6, 7 });
            straights.Add(new List<int> { 2, 3, 4, 5, 6 });
            straights.Add(lowestStraight);

            //IF PAST PREFLOP ROUND, CHECK IF HAVE 4 CARDS OUT OF STRAIGHT FOR STRAIGHT POSSIBILITY
            //loop through each possible straight
            for (int j = 0; j < straights.Count; j++)
            {

                int count = 0;

                //loop through the ranks in each possible straight
                foreach (int rankInStraight in straights[j])
                {

                    //loop through my unique ranks
                    foreach (int uniqueRank in intRanksUnique)
                    {

                        if (uniqueRank == rankInStraight)
                        {

                            count++;
                        }
                    }
                }

                //Debug.WriteLine ("count: " + count);

                if (count == 4)
                {
                    if (checkForStraightCalledOnFlop == false)
                    {
                        numberOfPossStraightsWithMy4Cards++;
                    }

                    else if (myCardRanks.Count == 6 && checkForStraightCalledOnTurn == false)
                    {
                        numberOfPossStraightsWithMy4Cards++;
                    }

                    //possible high straight
                    if (j == 0)
                    {
                        isAlmostHighStraight = true;
                    }

                    isAlmostStraight = true;

                    //CHANGED
                    //break;
                }
                //CHANGED
                //3 cards present in a straight
                else if (count == 3 && checkForStraightCalledOnFlop == false)
                {
                    numberOfPossStraightsWithMy3Cards++;
                    //possible high straight
                    if (j == 0)
                    {
                        is3AlmostHighStraight = true;
                    }

                    is3AlmostStraight = true;
                    //break;

                }
            }
            //flag that check for straight was called on flop so that number of poss straights w 3 or 4 cards
            //does not increment everytime it is called
            int numberOfPossStraightsWithMy4Cards_fromFlop = 0;
            if (myCardRanks.Count == 5)
            {

                checkForStraightCalledOnFlop = true;
                numberOfPossStraightsWithMy4Cards_fromFlop = numberOfPossStraightsWithMy4Cards;

            }
            //on turn
            else if (myCardRanks.Count == 6)
            {

                checkForStraightCalledOnTurn = true;

                //makes sure this increments from 0 instead of from number of poss straights on flop
                numberOfPossStraightsWithMy4Cards -= numberOfPossStraightsWithMy4Cards_fromFlop;
            }

            //check if intRanksUnique is a superset of lowestStraight
            bool isLowStraight = intRanksUnique.IsSupersetOf(lowestStraight);

            //if intRanksUnique contains all elements in lowestStraight, change Ace value (14) to 1
            if (isLowStraight)
            {
                intRanksUnique.Remove(14);
                intRanksUnique.Add(1);

            }
            //LOWEST STRAIGHT CASE END

            int numberOfCardsInARow = 1;

            //put values in low to high order using a list
            List<int> intRanksUniqueList = intRanksUnique.ToList();
            intRanksUniqueList.Sort();

            //the saved number of cards in a row in the case of straight
            int actualNumberOfCardsInARow = new int();// = Int()

            //the index of the high card in the case of straight
            int indexOfStraightHighCard = new int(); // = Int()

            int i = 0;
            //increment through entire ranked list to see if 5 in a row
            while (i < intRanksUniqueList.Count - 1)
            {
                if (intRanksUniqueList[i + 1] - intRanksUniqueList[i] == 1)
                {
                    numberOfCardsInARow++;

                    //case straight
                    if (numberOfCardsInARow >= 5)
                    {

                        actualNumberOfCardsInARow = numberOfCardsInARow;
                        indexOfStraightHighCard = i + 1;
                    }

                }
                else    //if cards not in a row, restart at 1
                {
                    numberOfCardsInARow = 1;
                }

                i++;
            }

            if (actualNumberOfCardsInARow >= 5)
            {
                double divisor = 100;

                isStraight = true;
                handRank = System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT");
                handRank += intRanksUniqueList[indexOfStraightHighCard] / divisor;

                //limit to 2 decimals
                //handRank = handRank.toFixed(2);
            }

            return handRank;
        }   //END CHECK FOR STRAIGHT

        //takes flush cards, sees if they make a straight. USE THE HIGHEST CARD AS RANK DESCRIPTOR
        double checkForStraightFlush()
        {
            bool isStraightFlush = false;

            double handRank = -1;

            //if hand is a flush and a straight
            if (Math.Floor((float)checkForFlush()) == System.Array.IndexOf(Constants.POKER_HANDS, "FLUSH") && Math.Floor((float)checkForStraight()) == System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT"))
            {
                //alert("flush and straight");

                string flushSuit = getFlushSuit();
                //alert(flushSuit);

                //list of flush cards only
                List<string> flushCards = getFlushCards(flushSuit);

                //assign the flush card list as the current Hand's cards
                myCardIds = flushCards;

                //the hand rank of straight (if not straight, = -1). Need just the decimal value
                double handRankStraight = checkForStraight();

                //can only be straight flush if the flush cards are a straight
                if (handRankStraight != -1)
                {
                    isStraightFlush = true;

                    //use straight flush index .(high card of straight) for hand rank
                    handRank = handRankStraight - System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT") + System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT_FLUSH");

                    //alert("Straight flush: " +handRank)
                }
            }

            return handRank;
        }

        public double GetRank()
        {
            double rank = checkForMultiples();
            int checkForMultiplesIndex = (int)Math.Floor(rank);

            if (Math.Floor((float)checkForStraightFlush()) == Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT_FLUSH"))
            {
                Debug.WriteLine("Straight Flush Rank: " + checkForStraightFlush());
                rank = checkForStraightFlush();
            }
            else if (checkForMultiplesIndex == Array.IndexOf(Constants.POKER_HANDS, "FOUR_OF_A_KIND"))
            {
                Debug.WriteLine("4 of a Kind Rank: " + rank);
            }
            else if (checkForMultiplesIndex == Array.IndexOf(Constants.POKER_HANDS, "FULL_HOUSE"))
            {
                Debug.WriteLine("Full House Rank: " + rank);
            }
            else if (Math.Floor((float)checkForFlush()) == Array.IndexOf(Constants.POKER_HANDS, "FLUSH"))
            {
                Debug.WriteLine("Flush Rank: " + checkForFlush());
                rank = checkForFlush();
            }
            else if (Math.Floor((float)checkForStraight()) == Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT"))
            {
                Debug.WriteLine("Straight Rank: " + checkForStraight());
                rank = checkForStraight();
            }
            else if (checkForMultiplesIndex == Array.IndexOf(Constants.POKER_HANDS, "THREE_OF_A_KIND"))
            {
                Debug.WriteLine("3 of a Kind Rank: " + rank);
            }
            else if (checkForMultiplesIndex == Array.IndexOf(Constants.POKER_HANDS, "TWO_PAIR"))
            {
                Debug.WriteLine("2 Pair Rank: " + rank);
            }
            else if (checkForMultiplesIndex == Array.IndexOf(Constants.POKER_HANDS, "PAIR"))
            {
                Debug.WriteLine("1 Pair Rank: " + rank);
            }
            else 
            {
                Debug.WriteLine("High Card Rank: " + rank);
            }

            return rank;
        }

        //update my hand everytime new cards are dealt, displays new probabilities
        public void UpdateMyCards()
        {

            //if new game, empty my cards
            if (TableGameMediator.newGameStarted)
            {

                Debug.WriteLine("inside MyHand, emptying my old cards");
                myPocketCardIds = new List<string>();
                tableCardIds = new List<string>();
                myCardIds = new List<string>();
            }

            //add the pocket hand to my cards
            if (TableGameMediator.CurrentRound == Rounds.isPreFlop) //(myCardIds.Count == 0) 
            {

                myPocketCardIds.Add(TableGameMediator.myCard0Id);
                myPocketCardIds.Add(TableGameMediator.myCard1Id);

                myCardIds = myPocketCardIds;

                //myPocketCardIds.AddRange (TableGameMediator.myCardIds);

                //TableGameMediator.CurrentRound = Rounds.isPreFlop;

                Debug.WriteLine("pre flop round");
            }
            //ADD ONLY THE NEW TABLE CARDS TO MY CARDS
            else if (TableGameMediator.CurrentRound == Rounds.isFlop)
            {

                //add the flop
                myCardIds.AddRange(TableGameMediator.tableCardIds);

                //on the flop round
                //TableGameMediator.CurrentRound = Rounds.isFlop;

                Debug.WriteLine("flop round");

            }
            else if (TableGameMediator.CurrentRound == Rounds.isTurn || TableGameMediator.CurrentRound == Rounds.isRiver)
            {

                //add the turn/river
                myCardIds.Add(TableGameMediator.tableCardIds[TableGameMediator.tableCardIds.Count - 1]);

                //if number of comm cards is now 4, we are on the turn round
                if (TableGameMediator.CurrentRound == Rounds.isTurn)
                {

                    //TableGameMediator.CurrentRound = Rounds.isTurn;

                    Debug.WriteLine("turn round");

                }
                else
                {

                    //last round before showdown (on the river)
                    //TableGameMediator.CurrentRound = Rounds.isRiver;

                    Debug.WriteLine("river round");
                }

            }
            //		else if (myCardIds.Count == 7) {
            //		
            //			ProbabilityCalculator.currentRound = ProbabilityCalculator.Rounds.isShowdown;
            //
            //			Debug.WriteLine ("showdown round");
            //		
            //		}

            int i = 0;
            foreach (string CardId in myCardIds)
            {

                Debug.WriteLine(TableGameMediator.CurrentRound + " MY CARD IDS " + i + " " + CardId);

                i++;
            }

            //ProbabilityCalculator.FindChancesOfPokerHands(this);
           
            TableGameMediator.GetWinningChance();
        }
    }
}
