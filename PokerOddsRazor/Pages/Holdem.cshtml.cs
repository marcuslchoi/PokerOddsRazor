//this is a Razor page's page model, like a viewmodel
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokerOddsRazor.Models;
using Newtonsoft.Json;

namespace PokerOddsRazor.Pages
{
    public class HoldemModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public MyHand Hand { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Card0 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Card1 { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Card0display { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Card1display { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Flop0 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Flop1 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Flop2 { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Turn { get; set; }
        [BindProperty(SupportsGet = true)]
        public string River { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ChanceHighCard { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChancePair { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceTwoPair { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceThreeKind { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceStraight { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceFlush { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceFullHouse { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceFourKind { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceStraightFlush { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChanceRoyalFlush { get; set; }

        [BindProperty(SupportsGet = true)]
        public double HandRank { get; set; }

        [BindProperty(SupportsGet = true)]
        public Rounds CurrentRound { get; set; }

        public void OnGet()
        {
            //todo should this happen every time?
            ProbabilityCalculator.Initialize();

            if (string.IsNullOrEmpty(Card0))
            {
                Card0 = "";
            }
            if (string.IsNullOrEmpty(Card1))
            {
                Card1 = "";
            }

            if (string.IsNullOrEmpty(Flop0))
            {
                Flop0 = "";
            }
            if (string.IsNullOrEmpty(Flop1))
            {
                Flop1 = "";
            }
            if (string.IsNullOrEmpty(Flop2))
            {
                Flop2 = "";
            }

            if (string.IsNullOrEmpty(Turn))
            {
                Turn = "";
            }
            if (string.IsNullOrEmpty(River))
            {
                River = "";
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            this.PossiblySetPreviousCardValues();

            //todo null check?
            TableGameMediator.SetCurrentRound(Hand);
            var rank = Hand.GetRank(); //todo make sure this isn't called too many times
            rank = Math.Round(rank, 10);

            //get probabilities
            var pc = ProbabilityCalculator.Instance;
            var vm = pc.FindChancesOfPokerHands(Hand);
            var highCardPercent = Constants.ConvertToPercent(vm.HighCard);
            var pairPercent = Constants.ConvertToPercent(vm.Pair);
            var twoPairPercent = Constants.ConvertToPercent(vm.TwoPair);
            var threeKindPercent = Constants.ConvertToPercent(vm.ThreeOfAKind);
            var straightPercent = Constants.ConvertToPercent(vm.Straight);
            var flushPercent = Constants.ConvertToPercent(vm.Flush);
            var fullhousePercent = Constants.ConvertToPercent(vm.FullHouse);
            var fourKindPercent = Constants.ConvertToPercent(vm.FourOfAKind);
            var straightFlushPercent = Constants.ConvertToPercent(vm.StraightFlush);
            var royalFlushPercent = Constants.ConvertToPercent(vm.RoyalFlush);

            //pass anonymous object with hand property
            return RedirectToPage(new
            {
                ChancePair = pairPercent.ToString(),
                ChanceHighCard = highCardPercent.ToString(),
                ChanceTwoPair = twoPairPercent.ToString(),
                ChanceThreeKind = threeKindPercent.ToString(),
                ChanceStraight = straightPercent.ToString(),
                ChanceFlush = flushPercent.ToString(),
                ChanceFullHouse = fullhousePercent.ToString(),
                ChanceFourKind = fourKindPercent.ToString(),
                ChanceStraightFlush = straightFlushPercent.ToString(),
                ChanceRoyalFlush = royalFlushPercent.ToString(),

                Card0 = Hand.MyCard0Id,
                Card0display = this.GetDisplayName(Hand.MyCard0Id),
                Card1 = Hand.MyCard1Id,
                Card1display = this.GetDisplayName(Hand.MyCard1Id),
                Flop0 = Hand.Flop0,
                Flop1 = Hand.Flop1,
                Flop2 = Hand.Flop2,
                Turn = Hand.Turn,
                River = Hand.River,
                CurrentRound = TableGameMediator.CurrentRound,
                HandRank = rank
            }); 
        }

        //convert to image file name
        private string GetDisplayName(string cardId)
        {
            string display = "";
            if (cardId.Contains('S'))
            {
                display = cardId.Remove(cardId.Length - 1);
                display += "spade";
            }
            else if(cardId.Contains('H'))
            {
                display = cardId.Remove(cardId.Length - 1);
                display += "heart";
            }
            else if (cardId.Contains('C'))
            {
                display = cardId.Remove(cardId.Length - 1);
                display += "club";
            }
            else if (cardId.Contains('D'))
            {
                display = cardId.Remove(cardId.Length - 1);
                display += "diamond";
            }
            return display;
        }

        private void PossiblySetPreviousCardValues()
        {
            if (this.Card0 != null && Hand.MyCard0Id == null)
            {
                Hand.MyCard0Id = this.Card0;
            }
            if (this.Card1 != null && Hand.MyCard1Id == null)
            {
                Hand.MyCard1Id = this.Card1;
            }
            if (this.Flop0 != null && Hand.Flop0 == null)
            {
                Hand.Flop0 = this.Flop0;
            }
            if (this.Flop1 != null && Hand.Flop1 == null)
            {
                Hand.Flop1 = this.Flop1;
            }
            if (this.Flop2 != null && Hand.Flop2 == null)
            {
                Hand.Flop2 = this.Flop2;
            }
            if (this.Turn != null && Hand.Turn == null)
            {
                Hand.Turn = this.Turn;
            }
            if (this.River != null && Hand.River == null)
            {
                Hand.River = this.River;
            }
        }
    }
}
