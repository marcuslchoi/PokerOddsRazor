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
