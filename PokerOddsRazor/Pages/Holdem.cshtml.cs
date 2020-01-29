//this is a Razor page's page model, like a viewmodel
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokerOddsRazor.Models;

namespace PokerOddsRazor.Pages
{
    public class HoldemModel : PageModel
    {
        [BindProperty]
        public MyHand Hand { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Card0 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Card1 { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Flop0 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Flop1 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Flop2 { get; set; }

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
                Card0 = "card0";
            }
            if (string.IsNullOrEmpty(Card1))
            {
                Card1 = "card1";
            }

            if (string.IsNullOrEmpty(Flop0))
            {
                Flop0 = "flop0";
            }
            if (string.IsNullOrEmpty(Flop1))
            {
                Flop1 = "flop1";
            }
            if (string.IsNullOrEmpty(Flop2))
            {
                Flop2 = "flop2";
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            //todo null check?
            TableGameMediator.SetCurrentRound(Hand);

            var pc = ProbabilityCalculator.Instance;
            var vm = pc.FindChancesOfPokerHands(Hand);
            var rank = Hand.GetRank(); //todo make sure this isn't called too many times

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
                Card1 = Hand.MyCard1Id,
                Flop0 = Hand.Flop[0],
                Flop1 = Hand.Flop[1],
                Flop2 = Hand.Flop[2],
                CurrentRound = TableGameMediator.CurrentRound,
                HandRank = rank
            }); //pass anonymous object with hand property
        }
    }
}
