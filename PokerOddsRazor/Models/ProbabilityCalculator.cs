//using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using UnityEngine.UI;

namespace PokerOddsRazor.Models
{
    public class ProbabilityCalculator
    {
        //public static Rounds CurrentRound;
        public static Rounds CurrentRound { get { return TableGameMediator.CurrentRound; } }

        //public Text roundText;
        public static string strNumberColor = "0271D9FF";//"00ffff";

        //public List<Text> chanceTexts = new List<Text>();

        //number of cards left in deck at any round
        static int cardsLeft;

        #region Singleton
        private static ProbabilityCalculator instance;
        private ProbabilityCalculator() { }
        public static ProbabilityCalculator Instance { get { return ProbabilityCalculator.instance; } }
        #endregion

        #region Init
        public static void Initialize()
        {
            ProbabilityCalculator.instance = new ProbabilityCalculator();
        }
        #endregion

        //called when cards are returned to deck
        public static void ClearAllTexts()
        {
            //ProbabilityCalculator pc = GetInstance();

            //pc.roundText.text = "WAITING...";
            //for (int i = 0; i < pc.chanceTexts.Count; i++) {

            //	pc.chanceTexts [i].enabled = false;
            //}
        }

        public static bool IsMyPokerHand(MyHand myHand, string pokerHand)
        {
            return (Math.Floor((float)myHand.GetRank()) ==
                System.Array.IndexOf(Constants.POKER_HANDS, pokerHand));
        }

        private ProbabilityViewModel GetFlopOrTurnChances(MyHand myHand)
        {
            var chanceHighCard = new double();
            var chancePair = new double();
            var chanceTwoPair = new double();
            var chanceThreeKind = new double();
            var chanceStraight = new double();
            var chanceFlush = new double();
            var chanceFullHouse = new double();
            var chanceFourKind = new double();
            var chanceStraightFlush = new double();
            var chanceRoyalFlush = new double();

            var cardsForPair = new double();
            var cardsForTriple = new double();

            if (CurrentRound == Rounds.isFlop)
            {
                //pc.roundText.text = "ON THE TURN AND RIVER";
            }
            else
            {
                //pc.roundText.text = "ON THE RIVER";
            }

            //high card
            if (IsMyPokerHand(myHand, "HIGH_CARD"))
            {
                if (CurrentRound == Rounds.isFlop)
                {

                    //5 cards dealt (including my pocket hand) * 3 left of each rank
                    cardsForPair = 15;

                    // 15/47 * 32/46 * 2
                    chancePair = cardsForPair / cardsLeft * (cardsLeft - cardsForPair) / (cardsLeft - 1) * 2;

                    // 15/47 * 12/46
                    chanceTwoPair = cardsForPair / cardsLeft * (cardsForPair - 3) / (cardsLeft - 1);

                    // 15/47 * 2/46 (there are 2 cards left after pair that could make a triple)
                    chanceThreeKind = cardsForPair / cardsLeft * 2 / (cardsLeft - 1);
                }
                else if (CurrentRound == Rounds.isTurn)
                {

                    cardsForPair = 18;

                    chancePair = cardsForPair / cardsLeft;
                    chanceTwoPair = 0;
                    chanceThreeKind = 0;
                }

                chanceFullHouse = 0;
                chanceFourKind = 0;

                //todo this should be 1 - all other chances
                chanceHighCard = 1;

            }

            else if (IsMyPokerHand(myHand, "PAIR"))
            {

                //HIDE HIGH CARD CHANCE TEXT IF ALREADY HAVE A PAIR
                //pc.chanceTexts [0].enabled = false;

                if (CurrentRound == Rounds.isFlop)
                {

                    // 9/47 * 38/46 * 2 = 31.6%
                    chanceTwoPair = 9d / cardsLeft * (cardsLeft - 9d) / (cardsLeft - 1) * 2;

                    // 2/47 * 45/46 * 2 = 8%
                    chanceThreeKind = 2d / cardsLeft * (cardsLeft - 2d) / (cardsLeft - 1) * 2;

                    //triple, then pair
                    // 2/47 * 9/46 * 2 = 1.7%
                    chanceFullHouse = 2d / cardsLeft * 9d / (cardsLeft - 1) * 2;

                    //.09%
                    chanceFourKind = 2d / cardsLeft * 1d / (cardsLeft - 1);

                }
                else //if (GameState.currentRound == GameState.Rounds.isTurn) 
                {

                    //3 * 3 possible cards to pair with
                    chanceTwoPair = 9d / cardsLeft;

                    //triple with pocket cards
                    chanceThreeKind = 2d / cardsLeft;

                    chanceFullHouse = 0;
                    chanceFourKind = 0;
                }

                chancePair = 1;

            }
            else if (IsMyPokerHand(myHand, "TWO_PAIR"))
            {

                Debug.WriteLine("Case: two pair on flop/turn");

                //HIDE CHANCE TEXTS FOR HIGH CARD, PAIR, THREE KIND
                for (int i = 0; i <= 3; i++)
                {
                    //don't hide 2 pair
                    if (i != 2)
                    {
                        //pc.chanceTexts [i].enabled = false;
                    }
                }

                if (CurrentRound == Rounds.isFlop)
                {
                    // 4/47 * 43/46 * 2 = 16%
                    chanceFullHouse = 4d / cardsLeft * (cardsLeft - 4) / (cardsLeft - 1) * 2;

                    chanceFourKind = 4d / cardsLeft * 1d / (cardsLeft - 1);

                }
                else
                {

                    chanceFullHouse = 4d / cardsLeft;
                    chanceFourKind = 0;
                }
                //if 2 pair, a triple would give a full house
                chanceThreeKind = 0;


                chanceTwoPair = 1;

            }
            //3 of a kind
            else if (IsMyPokerHand(myHand, "THREE_OF_A_KIND"))
            {


                for (int i = 0; i <= 2; i++)
                {

                    //pc.chanceTexts [i].enabled = false;
                }

                if (CurrentRound == Rounds.isFlop)
                {

                    // 6/47 * 41/46 * 2
                    chanceFullHouse = 6d / cardsLeft * (cardsLeft - 6d) / (cardsLeft - 1) * 2;
                    chanceFourKind = 1d / cardsLeft * 2;

                }
                else
                {

                    chanceFullHouse = 6d / cardsLeft;
                    chanceFourKind = 1d / cardsLeft;
                }

                chanceThreeKind = 1;

            }
            //straight
            else if (IsMyPokerHand(myHand, "STRAIGHTS"))
            {

                for (int i = 0; i <= 3; i++)
                {

                    //pc.chanceTexts [i].enabled = false;
                }

                chanceFullHouse = 0;
                chanceFourKind = 0;

                chanceStraight = 1;

            }
            //flush
            else if (IsMyPokerHand(myHand, "FLUSH"))
            {

                for (int i = 0; i <= 4; i++)
                {

                    //pc.chanceTexts [i].enabled = false;
                }

                chanceFullHouse = 0;
                chanceFourKind = 0;

                chanceFlush = 1;

            }
            //full house
            else if (IsMyPokerHand(myHand, "FULL_HOUSE"))
            {

                for (int i = 0; i <= 5; i++)
                {

                    //pc.chanceTexts [i].enabled = false;
                }

                if (CurrentRound == Rounds.isFlop)
                {
                    chanceFourKind = 1d / cardsLeft * 2;
                }
                else
                {
                    chanceFourKind = 1d / cardsLeft;
                }

                chanceFullHouse = 1;

            }
            //four of a kind
            else if (IsMyPokerHand(myHand, "FOUR_OF_A_KIND"))
            {

                for (int i = 0; i <= 6; i++)
                {

                    //pc.chanceTexts [i].enabled = false;
                }
                //already checked below
                //chanceStraightFlush = 0;

                chanceFourKind = 1;
            }
            //straight flush
            else if (IsMyPokerHand(myHand, "STRAIGHT_FLUSH"))
            {

                for (int i = 0; i <= 7; i++)
                {

                    //pc.chanceTexts [i].enabled = false;
                }

                //if my hand is a king high straight flush
                if (myHand.GetRank() == System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT_FLUSH") + 0.13)
                {

                    if (CurrentRound == Rounds.isFlop)
                    {
                        chanceRoyalFlush = 1d / cardsLeft + (cardsLeft - 1d) / cardsLeft * 1d / (cardsLeft - 1);
                    }
                    else
                    {
                        chanceRoyalFlush = 1d / cardsLeft;
                    }
                }
                //if my hand is a royal flush
                else if (myHand.GetRank() == System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT_FLUSH") + 0.14)
                {

                    chanceRoyalFlush = 1;
                    //hide all chance texts because already a royal flush
                    //for (int i = 0; i < pc.chanceTexts.Count-1; i++) 
                    //{
                    //	pc.chanceTexts [i].enabled = false;
                    //}
                }

                chanceStraightFlush = 1;
            }

            //CHANGED
            //FLUSH, STRAIGHT, AND STRAIGHT FLUSH POSSIBILITY----------------------------

            //getRank already called checkForStraight

            //equals "N" if there is no four card flush
            string myFourFlushSuit = myHand.fourFlushSuit;

            //IF THERE IS A 4 FLUSH SUIT, THERE IS A 3 FLUSH SUIT
            string myThreeFlushSuit = myHand.threeFlushSuit;

            Debug.WriteLine("four flush suit: " + myFourFlushSuit);
            Debug.WriteLine("three flush suit: " + myThreeFlushSuit);

            MyHand my4FlushHand = new MyHand();
            MyHand my3FlushHand = new MyHand();

            //chance for flush
            if (CurrentRound == Rounds.isFlop && Math.Floor((float)myHand.GetRank()) < System.Array.IndexOf(Constants.POKER_HANDS, "FLUSH"))
            {
                //if there is exactly 3 card flush (also for checking for straight flush)
                //CHANGED
                if (myThreeFlushSuit != "N" && myFourFlushSuit == "N")
                {

                    //if there are exactly 3 flush cards
                    //if (myFourFlushSuit == "N") {

                    Debug.WriteLine("exactly 3 flush cards");
                    chanceFlush = 9d / cardsLeft * 8d / (cardsLeft - 1);
                    //}


                    List<string> myThreeFlushCards = myHand.getFlushCards(myThreeFlushSuit);

                    //					Debug.WriteLine ("1INSIDE FLOP CHANCE FLUSH: " + TableGameMediator.currentRound + " " + MyHand.myCardIds.Count);

                    //create new hand with just 3 flush cards, check if 3 cards present in straight
                    my3FlushHand = new MyHand(myThreeFlushCards);
                    my3FlushHand.checkForStraight();

                    //					Debug.WriteLine ("2INSIDE FLOP CHANCE FLUSH: " + TableGameMediator.currentRound + " " + MyHand.myCardIds.Count);

                }
                //if there is a 4+ card flush (also for checking for straight flush)
                else if (myFourFlushSuit != "N")
                {

                    Debug.WriteLine("four flush");
                    //9 cards left in deck of same flush suit
                    //chance of both new cards flushing + chance of exactly 1 card flushing
                    // 9/47 * 8/46 + (9/47 * 38/46)*2 = 35%
                    chanceFlush = 9d / cardsLeft * 8d / (cardsLeft - 1) + 2 * (9d / cardsLeft * (cardsLeft - 9d) / (cardsLeft - 1));


                    List<string> myFourFlushCards = myHand.getFlushCards(myFourFlushSuit);

                    //create new hand with just 4 flush cards, check if 4 cards present in straight
                    my4FlushHand = new MyHand(myFourFlushCards);
                    my4FlushHand.checkForStraight();

                }

                else
                {
                    chanceFlush = 0;
                }

            }
            else if (CurrentRound == Rounds.isTurn && Math.Floor((float)myHand.GetRank()) < System.Array.IndexOf(Constants.POKER_HANDS, "FLUSH"))
            {
                //must have 4 flush cards for possible flush
                //Hand my4FlushHand = new Hand ();

                //if there is a 4 card flush (also for checking for straight flush)
                if (myFourFlushSuit != "N")
                {

                    Debug.WriteLine("four flush");
                    //9 cards left in deck of same flush suit
                    chanceFlush = 9d / cardsLeft;

                    List<string> myFourFlushCards = myHand.getFlushCards(myFourFlushSuit);

                    //create new hand with just 4 flush cards, check if 4 cards present in straight
                    my4FlushHand = new MyHand(myFourFlushCards);
                    my4FlushHand.checkForStraight();

                }
                else
                {

                    chanceFlush = 0;
                }
            }

            //CHANCE FOR STRAIGHT if have 4 of 5 straight cards already
            if (myHand.isAlmostStraight && Math.Floor((float)myHand.GetRank()) < System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT"))
            {

                //STRAIGHT IS POSSIBLE

                //CHANGED
                //chance with 8 possible cards to finish straight
                if (myHand.numberOfPossStraightsWithMy4Cards == 2)
                {

                    if (CurrentRound == Rounds.isFlop)
                    {
                        //first card straights and second card either does or doesn't + only 2nd card straights
                        // 8/47 * 1 + 39/47 * 8/46 = 31.5%
                        chanceStraight = 8d / cardsLeft + (cardsLeft - 8d) / cardsLeft * 8d / (cardsLeft - 1);
                    }
                    else //turn round
                    {
                        chanceStraight = 8d / cardsLeft;
                    }
                }
                //chance with 4 possible cards to finish straight
                else //only 1 possible straight
                {
                    if (CurrentRound == Rounds.isFlop)
                    {
                        //flop round with 1 4straight and is3almostStraight
                        if (myHand.is3AlmostStraight)
                        {
                            //chance of completing the 4 straight + chance of completing the 3 straight
                            chanceStraight = (4d / cardsLeft + (cardsLeft - 4d) / cardsLeft * 4d / (cardsLeft - 1)) + 8d / cardsLeft * 4d / (cardsLeft - 1);

                        }
                        else
                        {
                            // 4/47 * 1 + 43/47 * 4/46
                            chanceStraight = 4d / cardsLeft + (cardsLeft - 4d) / cardsLeft * 4d / (cardsLeft - 1);
                        }
                    }
                    else  //turn round
                    {
                        chanceStraight = 4d / cardsLeft;
                    }
                }

            }

            //chance for straight if have 3 of 5 straight cards already (only in flop round)
            else if (myHand.is3AlmostStraight && CurrentRound == Rounds.isFlop && Math.Floor((float)myHand.GetRank()) < System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT"))
            {
                Debug.WriteLine("INSIDE IS3ALMOSTSTRAIGHT " + myHand.numberOfPossStraightsWithMy3Cards);
                //STRAIGHT IS POSSIBLE
                if (myHand.numberOfPossStraightsWithMy3Cards >= 3)
                {

                    Debug.WriteLine("3 or more diff poss straights");
                    //both cards must straight.
                    //first card has 2 poss ranks to make 4 in a row, then 2 poss ranks for 2nd card +
                    //first card has 2 poss ranks to make 4 in poss straight (with gap), then 1 poss rank (gap card)
                    // 8/47 * 8/46 + 8/47 * 4/46
                    chanceStraight = 8d / cardsLeft * 8d / (cardsLeft - 1) + 8d / cardsLeft * 4d / (cardsLeft - 1);

                }
                else if (myHand.numberOfPossStraightsWithMy3Cards == 2)
                {
                    //case 2,3,4 (first card 5, then first card 6, then first card Ace)
                    // 4/47 * 8/46 + 4/47 * 4/46 + 4/47 * 4/46 is the same answer. Also works
                    //for case of low straight and high straight possibility (Ace included)

                    Debug.WriteLine("2 diff poss straights");
                    //1 poss rank for 4 in a row (the gap), then 2 poss ranks (2 ways this can happen) 
                    // 4/47 * 8/46 * 2
                    chanceStraight = 4d / cardsLeft * 8d / (cardsLeft - 1) * 2;

                }
                else if (myHand.numberOfPossStraightsWithMy3Cards == 1)
                {
                    Debug.WriteLine("1 poss straight");
                    //2 poss ranks then 1 poss rank
                    // 8/47 * 4/46
                    chanceStraight = 8d / cardsLeft * 4d / (cardsLeft - 1);

                }

            }
            //there are not 4 of 5 straight cards present, or 3 straight in the flop round
            else if (Math.Floor((float)myHand.GetRank()) < System.Array.IndexOf(Constants.POKER_HANDS, "STRAIGHT"))
            {
                chanceStraight = 0;
            }

            //CHANCE FOR STRAIGHT FLUSH
            //works for if I have a flush already too (my4FlushHand can have 4+ cards in it)
            if (my4FlushHand.isAlmostStraight)
            {

                //STRAIGHT FLUSH IS POSSIBLE

                Debug.WriteLine("poss straight flush");

                //ROYAL FLUSH POSSIBILITY
                if (my4FlushHand.isAlmostHighStraight)
                {

                    Debug.WriteLine("poss royal flush");

                    if (CurrentRound == Rounds.isFlop)
                    {
                        chanceRoyalFlush = 1d / cardsLeft * 2;
                    }
                    else
                    {
                        chanceRoyalFlush = 1d / cardsLeft;
                    }
                }
                //check if there is possible Royal Flush w 3 of the 5 poss cards
                else if (CurrentRound == Rounds.isFlop && my4FlushHand.is3AlmostHighStraight)
                {

                    Debug.WriteLine("is3almosthighstraight with a 4+ card flush (also is almost straight flush)");
                    chanceRoyalFlush = 2d / cardsLeft * 1d / (cardsLeft - 1);
                }

                //STRAIGHT FLUSH POSSIBILITY: chance with 2 possible cards to finish straight flush
                if (my4FlushHand.numberOfPossStraightsWithMy4Cards == 2)
                {

                    if (CurrentRound == Rounds.isFlop)
                    {
                        chanceStraightFlush = 2d / cardsLeft + (cardsLeft - 2d) / cardsLeft * 2d / (cardsLeft - 1);
                    }
                    else
                    {
                        chanceStraightFlush = 2d / cardsLeft;
                    }

                }
                //chance with 1 possible card to finish straight flush
                else
                { //only 1 possible 4straight

                    if (CurrentRound == Rounds.isFlop)
                    {
                        if (my4FlushHand.is3AlmostStraight)
                        {

                            //chance of completing the 4 straight + chance of completing the 3 straight
                            chanceStraightFlush = (1d / cardsLeft * 2) + 2d / cardsLeft * 1d / (cardsLeft - 1);
                        }
                        else
                        {
                            chanceStraightFlush = 1d / cardsLeft + (cardsLeft - 1d) / cardsLeft * 1d / (cardsLeft - 1);
                        }

                    }
                    else
                    {
                        chanceStraightFlush = 1d / cardsLeft;
                    }

                }

            }

            //3+ flush hand is 3almost straight during flop round
            else if (my3FlushHand.is3AlmostStraight && CurrentRound == Rounds.isFlop)
            {
                Debug.WriteLine("is3almosthighstraight " + my3FlushHand.is3AlmostHighStraight);

                if (my3FlushHand.numberOfPossStraightsWithMy3Cards >= 3)
                {

                    Debug.WriteLine("3 or more diff poss straight flushes");
                    //both cards must straight.
                    //first card has 2 poss ranks to make 4 in a row, then 2 poss ranks for 2nd card +
                    //first card has 2 poss ranks to make 4 in poss straight (with gap), then 1 poss rank (gap card)
                    // 2/47 * 2/46 + 2/47 * 1/46
                    chanceStraightFlush = 2d / cardsLeft * 2d / (cardsLeft - 1) + 2d / cardsLeft * 1d / (cardsLeft - 1);

                }
                else if (my3FlushHand.numberOfPossStraightsWithMy3Cards == 2)
                {

                    Debug.WriteLine("2 diff poss straight flushes");
                    //1 poss rank for 4 in a row (the gap), then 2 poss ranks (2 ways this can happen) 
                    // 1/47 * 2/46 * 2
                    chanceStraightFlush = 1d / cardsLeft * 2d / (cardsLeft - 1) * 2;

                }
                else if (my3FlushHand.numberOfPossStraightsWithMy3Cards == 1)
                {
                    Debug.WriteLine("1 poss straight flush");
                    //2 poss ranks then 1 poss rank
                    // 2/47 * 1/46
                    chanceStraightFlush = 2d / cardsLeft * 1d / (cardsLeft - 1);

                }

                if (my3FlushHand.is3AlmostHighStraight)
                {

                    Debug.WriteLine("is3almosthighstraight!!!!!!!!");
                    chanceRoyalFlush = 2d / cardsLeft * 1d / (cardsLeft - 1);
                }
            }
            else
            { //there are not 4 of 5 straight flush cards present

                chanceStraightFlush = 0;
                chanceRoyalFlush = 0;
            }

            var vm = new ProbabilityViewModel
                (chanceHighCard, chancePair, chanceTwoPair, chanceThreeKind,
                chanceStraight, chanceFlush, chanceFullHouse,
                chanceFourKind, chanceStraightFlush, chanceRoyalFlush);

            return vm;
        }

        private ProbabilityViewModel GetPreFlopChances(MyHand myHand)
        {
            //pc.roundText.text = "CHANCES ON THE FLOP";

            List<int> myPocketRanks = myHand.GetPocketCardRanksHighToLow();

            //just the suits of the pocket hand
            List<string> myPocketSuits = myHand.GetPocketSuits();
            bool cardsAreSuited = myHand.PocketCardsAreSuited();

            var cardsForPair = new double();
            var cardsForTriple = new double();

            var chanceHighCard = new double();
            var chancePair = new double();
            var chanceTwoPair = new double();
            var chanceThreeKind = new double();
            var chanceStraight = new double();
            var chanceFlush = new double();
            var chanceFullHouse = new double();
            var chanceFourKind = new double();
            var chanceStraightFlush = new double();
            var chanceRoyalFlush = new double();


            //IF I HAVE A HIGH CARD WITH MY POCKET CARDS
            if (IsMyPokerHand(myHand, "HIGH_CARD"))
            {
                //3 cards could pair with first pocket card and 3 with the second
                cardsForPair = 6;

                //				chanceHighCard = 1;

                //flop0 pairs, flop1 doesnt, flop2 doesnt
                //6/50 * 44/49 * (43-3)/48. The 3 represents the number of ranks left matching flop1
                chancePair = cardsForPair / cardsLeft * ((cardsLeft - 1) - (cardsForPair - 1)) / (cardsLeft - 1) * ((cardsLeft - 2) - (cardsForPair - 1) - 3) / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE;


                //TWO PAIR SECTION --------------------------------------------------------

                //chance of first card pairing with a pocket card, 2nd card not pairing, 3rd card pairing with 2nd card
                //6/50 * 44/49 * 3/48
                double chanceTwoPairComm = 6d / cardsLeft * 44d / (cardsLeft - 1) * 3d / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE;

                //chance of first card pairing with a pocket card, 2nd card pairing with other pocket card, 3rd card not pairing
                //6/50 * 3/49 * 44/48
                chanceTwoPair = cardsForPair / cardsLeft * 3d / (cardsLeft - 1) * ((cardsLeft - 2) - (cardsForPair - 2)) / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE + chanceTwoPairComm;


                //chance of all 3 comm cards same rank (not same as either pocket card)
                //(50-6)/50 * 3/49 * 2/48
                double chanceThreeKindComm = (cardsLeft - cardsForPair) / cardsLeft * 3d / (cardsLeft - 1) * 2d / (cardsLeft - 2);

                //chance flop0 pairing, flop1 pairing with flop0, flop3 not pairing
                //6/50 * 2/49 * 44/48 * Constants.HOLDEM_FLOPSIZE + chanceThreeKindComm
                chanceThreeKind = cardsForPair / cardsLeft * 2d / (cardsLeft - 1) * ((cardsLeft - 2) - (cardsForPair - 2)) / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE + chanceThreeKindComm;


                //STRAIGHT AND STRAIGHT FLUSH POSSIBILITY --------------------------------------------

                //for possible straight on flop, 
                //cards must have an Ace and 2,3,4 or 5 (Ace low straight), or cards must be less than 5 ranks different
                bool hasAce = myPocketRanks[0] == 14;
                bool possAceLowStraight = hasAce && myPocketRanks[1] <= 5;
                bool possNormalStraight = myPocketRanks[0] - myPocketRanks[1] < 5;
                if (possAceLowStraight || possNormalStraight)
                {
                    //STRAIGHT IS POSSIBLE ON FLOP

                    //number of card ranks per each suit
                    double ranksPerSuit = 4d;

                    //IF RANKS ARE < 4 APART AND THERE IS NO ACE, the first card has 8 possibilities 
                    //(ie if pocket cards are 2 and 5, first card can be Ace or 6)
                    if (myPocketRanks[0] - myPocketRanks[1] < 4 && !hasAce)
                    {
                        //8d / 50d * 4d / 49d * 4d / 48d * 3d*2d;
                        chanceStraight = 2d * ranksPerSuit / cardsLeft * ranksPerSuit / (cardsLeft - 1) * ranksPerSuit / (cardsLeft - 2) * 3d * 2d;
                    }
                    else
                    {
                        //ranks are at the ends of the straight so there is only one possible straight
                        //since they are all different ranks, there are 3! (3*2) different orders for them to be dealt
                        //4d / 50d * 4d / 49d * 4d / 48d * 3d*2d;
                        chanceStraight = ranksPerSuit / cardsLeft * ranksPerSuit / (cardsLeft - 1) * ranksPerSuit / (cardsLeft - 2) * 3d * 2d;
                    }

                    //if cards are suited, there's a chance of straight flush on flop
                    if (cardsAreSuited)
                    {
                        //straight flush is also possible on flop
                        //1d / 50d * 1d / 49d * 1d / 48d * 3d * 2d;
                        chanceStraightFlush = 1d / cardsLeft * 1d / (cardsLeft - 1) * 1d / (cardsLeft - 2) * 3d * 2d;

                        //Royal flush chance
                        bool bothRankedAtLeastTen = myPocketRanks[0] >= 10 && myPocketRanks[1] >= 10;
                        if (bothRankedAtLeastTen)
                        {
                            chanceRoyalFlush = chanceStraightFlush;
                        }
                    }
                }
                else
                { //cards are not within 5 ranks so no possibility of straight
                    Debug.WriteLine("straight is not possible on flop");
                    chanceStraight = 0;
                    chanceStraightFlush = 0;
                }

                //FLUSH POSSIBILITY--------------------------------------------

                if (cardsAreSuited)
                {
                    //flush is possible on flop
                    var flushCardsLeft = 11d;
                    //flop0 is flushcard, flop1 is flushcard, flop2 is flushcard (only one way to order this) 
                    //11d / 50d * 10d / 49d * 9d / 48d;
                    chanceFlush = flushCardsLeft / cardsLeft * (flushCardsLeft - 1) / (cardsLeft - 1) * (flushCardsLeft - 2) / (cardsLeft - 2);
                }
                else
                {
                    Debug.WriteLine("flush not possible on flop");
                    chanceFlush = 0;
                }

                //FULL HOUSE POSSIBILITY--------------------------------------------

                //chance of pairing on flop0, triple on flop1, pairing with other pocket card on flop2
                //6d / 50d * 2d / 49d * 3d / 48d * Constants.HOLDEM_FLOPSIZE;
                chanceFullHouse = cardsForPair / cardsLeft * 2d / (cardsLeft - 1) * 3d / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE;

                //FOUR OF A KIND POSSIBILITY--------------------------------------------

                //first card has 6 possibilities for pairing, then only 2 for triple, then 1 for quad
                //6 / 50 * 2 / 49 * 1 / 48
                chanceFourKind = cardsForPair / cardsLeft * 2d / (cardsLeft - 1) * 1d / (cardsLeft - 2);

                //starts from 1 (pair) through straight flush (count-1) because royal flush is a type of straight flush
                chanceHighCard = 1;

            } //CLOSE HIGH CARD
            else if (IsMyPokerHand(myHand, "PAIR"))
            {

                //HIDE HIGH CARD CHANCE TEXT
                //pc.chanceTexts [0].enabled = false;

                //TWO PAIR SECTION --------------------------------------------------------

                //chance of first card not tripling with pocket cards, 2nd card not tripling or pairing with 1st, 3rd card pairing with 2nd card
                //48/50 * (47-3)/49 * 3/48 * 3
                chanceTwoPair = 48d / cardsLeft * 44d / (cardsLeft - 1) * 3d / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE;

                //THREE KIND SECTION

                cardsForTriple = 2;

                //chance flop0 tripling, flop1 no multiple, flop2 no multiple
                //2/50 * 48/49 * (47-3)/48 * Constants.HOLDEM_FLOPSIZE
                chanceThreeKind = cardsForTriple / cardsLeft * ((cardsLeft - 1) - (cardsForTriple - 1)) / (cardsLeft - 1) * ((cardsLeft - 2) - (cardsForTriple - 1) - 3d) / (cardsLeft - 2) * Constants.HOLDEM_FLOPSIZE;

                //STRAIGHT, FLUSH, AND STRAIGHT FLUSH NOT POSSIBLE --------------------------------------------

                chanceStraight = 0;
                chanceFlush = 0;
                chanceStraightFlush = 0;

                //FULL HOUSE POSSIBILITY--------------------------------------------

                //chance of all 3 comm cards same rank (not same as either pocket card)
                //(50-2)/50 * 3/49 * 2/48
                double chanceThreeKindComm = (cardsLeft - cardsForTriple) / cardsLeft * 3d / (cardsLeft - 1) * 2d / (cardsLeft - 2);

                //TODO: RECODE
                //flop0 triples, flop1 other rank, flop2 pairs with flop1
                //2/50 * 48/49 * 3/48 * Constants.HOLDEM_FLOPSIZE
                chanceFullHouse = 2d / 50d * 48d / 49d * 3d / 48d * Constants.HOLDEM_FLOPSIZE + chanceThreeKindComm;

                //flop0 and flop1 make quad, flop2 any card
                //2d / 50d * 1d / 49d * 1d * Constants.HOLDEM_FLOPSIZE
                chanceFourKind = 2d / 50d * 1d / 49d * 1d * Constants.HOLDEM_FLOPSIZE;

                chancePair = 1;

            } //CLOSE PAIR

            var vm = new ProbabilityViewModel
                (chanceHighCard, chancePair, chanceTwoPair, chanceThreeKind,
                chanceStraight, chanceFlush, chanceFullHouse,
                chanceFourKind, chanceStraightFlush, chanceRoyalFlush);

            return vm;
        }

        public void FindChancesOfPokerHands(MyHand myHand)
        {
            //TODO: POSSIBLE BUGS SEEN ON STRAIGHT POSSIBILITY
            //TODO: INDICATOR OF PLAYING BOARD CARDS (IE IF 2 BOARD CARDS ARE PAIR, CHANCE 3KIND ON BOARD IS
            //ONLY GOOD IF YOU HAVE ANOTHER HIGH CARD FOR POSSIBLE KICKER)
            //HAND STRENGTH INDICATOR: DEPENDS ON WHETHER YOUR CARDS COME INTO PLAY, HOW HIGH IN PLAY 
            //(IE HIGHEST CARD IN STRAIGHT, LOW OR HIGH COMPLETION OF FLUSH, ETC)

            //		MyHand myHand = new MyHand (MyHand.myCardIds);

            //ProbabilityCalculator pc = GameObject.Find ("ProbabilityCalculator").GetComponent<ProbabilityCalculator> ();

            //enable the round text and chance texts
            //pc.roundText.enabled = true;
            //for (int i = 0; i < pc.chanceTexts.Count; i++) {

            //	pc.chanceTexts [i].enabled = true;
            //}

            //just the ranks of the pocket hand, sorted from high to low
            List<int> myPocketRanks = myHand.GetPocketCardRanksHighToLow();

            //just the suits of the pocket hand
            List<string> myPocketSuits = myHand.GetPocketSuits();
            //bool cardsAreSuited = myPocketSuits[0] == myPocketSuits[1];

            //var deckSize = 52;

            //var flopSize = 3;

            //number of cards still in deck that can be used to pair with a pocket card
            //var cardsForPair = new double();
            //var cardsForTriple = new double();

            //var chanceHighCard = new double();
            //var chancePair = new double();
            //var chanceTwoPair = new double();
            //var chanceThreeKind = new double();
            //var chanceStraight = new double();
            //var chanceFlush = new double();
            //var chanceFullHouse = new double();
            //var chanceFourKind = new double();
            //var chanceStraightFlush = new double();
            //var chanceRoyalFlush = new double();

            var numberPlayers = 1;  //CHANGE THIS TO PLAYERLIST.COUNT? NO NEED

            //number of cards left in the deck in each round
            if (CurrentRound == Rounds.isPreFlop)
            {
                cardsLeft = Constants.DECK_SIZE - 2 * numberPlayers;
            }
            else if (CurrentRound == Rounds.isFlop)
            {
                cardsLeft = Constants.DECK_SIZE - 2 * numberPlayers - Constants.HOLDEM_FLOPSIZE;
            }
            else if (CurrentRound == Rounds.isTurn)
            {
                cardsLeft = Constants.DECK_SIZE - 2 * numberPlayers - Constants.HOLDEM_FLOPSIZE - 1;
            }

            ProbabilityViewModel vm;

            //http://poker.stackexchange.com/questions/1474/formula-for-making-a-single-pair-on-the-flop
            if (CurrentRound == Rounds.isPreFlop)
            {
                vm = GetPreFlopChances(myHand);

            }  
            else if (CurrentRound == Rounds.isFlop || CurrentRound == Rounds.isTurn)
            {
                vm = GetFlopOrTurnChances(myHand);
            }  
            //river was just dealt, hide all chance texts except my hand
            else if (CurrentRound == Rounds.isRiver)
            {

                var indexOfPokerHand = (int)Math.Floor((float)myHand.GetRank());

                //pc.roundText.text = "FINAL ROUND";

                //for (int i = 0; i < pc.chanceTexts.Count; i++) {

                //	if( i == 10)
                //		continue;
                //	if (i != indexOfPokerHand)
                //	{
                //		pc.chanceTexts [i].enabled = false;
                //	}
                //}

            }

            //below is UI stuff

            //List<float> chancesList = new List<float> { (float)chanceHighCard, (float)chancePair, (float)chanceTwoPair, (float)chanceThreeKind, (float)chanceStraight, (float)chanceFlush, (float)chanceFullHouse, (float)chanceFourKind, (float)chanceStraightFlush, (float)chanceRoyalFlush };
            //List<string> chancesListStr = new List<string>();

            //for (int i = 0; i < chancesList.Count; i++)
            //{

            //    chancesList[i] *= 100;
            //    //chancesList[i] = Math.Round (chancesList[i] * 1000) / 1000;
            //    chancesListStr.Add(chancesList[i].ToString("N1"));

            //    if (chancesListStr[i] == "100.0")
            //    {

            //        chancesListStr[i] = "CURRENT HAND";
            //    }
            //}

        //    List<string> pokerHandsList = new List<string> {
        //    "HIGH CARD",
        //    "PAIR",
        //    "TWO PAIR",
        //    "THREE OF A KIND",
        //    "STRAIGHT",
        //    "FLUSH",
        //    "FULL HOUSE",
        //    "FOUR OF A KIND",
        //    "STRAIGHT FLUSH",
        //    "ROYAL FLUSH"
        //};

         //   string lastCharacter;
            //for (int i = 0; i < pc.chanceTexts.Count-1; i++) 
            //{

            //	if (chancesListStr [i] == "CURRENT HAND") {

            //		lastCharacter = "";

            //	} else {

            //		lastCharacter = "%";
            //	}

            //	pc.chanceTexts [i].text = pokerHandsList [i] + ": "+ "<color=#" + strNumberColor + ">" + chancesListStr [i] + lastCharacter + "</color>";

            //}

            //if (CurrentRound == Rounds.isRiver)
            //{

                //for (int i = 0; i < pc.chanceTexts.Count-1; i++) 
                //{
                //	pc.chanceTexts [i].text = pokerHandsList [i];
                //}

            //}

        }

        static public void SetHandStrength(float fStr)
        {
            //ProbabilityCalculator pc = GetInstance();
            //if(pc.chanceTexts.Count > 9)
            //{
            //	pc.chanceTexts[10].text = "HAND STRENGTH: " + "<color=#" + strNumberColor + ">" + fStr.ToString("N1") + "/100</color>";
            //}
        }
    }
}
