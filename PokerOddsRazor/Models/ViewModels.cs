using System;
namespace PokerOddsRazor.Models
{
    public class ProbabilityViewModel
    {
        public double HighCard { get; set; }
        public double Pair { get; set; }
        public double TwoPair { get; set; }
        public double ThreeOfAKind { get; set; }
        public double Straight { get; set; }
        public double Flush { get; set; }
        public double FullHouse { get; set; }
        public double FourOfAKind { get; set; }
        public double StraightFlush { get; set; }
        public double RoyalFlush { get; set; }

        public ProbabilityViewModel()
        {
        }

        public ProbabilityViewModel(PokerHand pokerHand, bool isRoyalFlush = false)
        {
            if (isRoyalFlush)
            {
                this.RoyalFlush = 1;
                return;
            }

            switch (pokerHand)
            {
                case PokerHand.HIGH_CARD:
                    this.HighCard = 1;
                    break;
                case PokerHand.PAIR:
                    this.Pair = 1;
                    break;
                case PokerHand.TWO_PAIR:
                    this.TwoPair = 1;
                    break;
                case PokerHand.THREE_OF_A_KIND:
                    this.ThreeOfAKind = 1;
                    break;
                case PokerHand.STRAIGHT:
                    this.Straight = 1;
                    break;
                case PokerHand.FLUSH:
                    this.Flush = 1;
                    break;
                case PokerHand.FULL_HOUSE:
                    this.FullHouse = 1;
                    break;
                case PokerHand.FOUR_OF_A_KIND:
                    this.FourOfAKind = 1;
                    break;
                case PokerHand.STRAIGHT_FLUSH:
                    this.StraightFlush = 1;
                    break;
            }
        }

        public ProbabilityViewModel(double highCard, double pair,
            double twoPair, double threeKind, double straight, double flush,
            double fullHouse, double fourKind, double straightFlush, double royalFlush)
        {
            HighCard = highCard;
            Pair = pair;
            TwoPair = twoPair;
            ThreeOfAKind = threeKind;
            Straight = straight;
            Flush = flush;
            FullHouse = fullHouse;
            FourOfAKind = fourKind;
            StraightFlush = straightFlush;
            RoyalFlush = royalFlush;
        }
    }
}
