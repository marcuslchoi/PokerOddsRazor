//this is a Razor page's page model, like a viewmodel
using System;
using System.Collections.Generic;
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
        public string ChanceHighCard { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ChancePair { get; set; }
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

            if (string.IsNullOrEmpty(ChancePair))
            {
                ChancePair = string.Empty;
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            TableGameMediator.CurrentRound = Rounds.isPreFlop;
            var pc = ProbabilityCalculator.Instance;
            var vm = pc.FindChancesOfPokerHands(Hand);

            var highCardPercent = vm.HighCard * 100;
            var pairPercent = vm.Pair * 100;

            return RedirectToPage(new
            {ChancePair = pairPercent.ToString(),ChanceHighCard = highCardPercent.ToString(),
                Card0 = Hand.MyCard0Id, Card1 = Hand.MyCard1Id }); //pass anonymous object with hand property
        }
    }
}
