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
        public void OnGet()
        {
            if (string.IsNullOrEmpty(Card0))
            {
                Card0 = "card0";
            }
            if (string.IsNullOrEmpty(Card1))
            {
                Card1 = "card1";
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            TableGameMediator.CurrentRound = Rounds.isPreFlop;
            ProbabilityCalculator.FindChancesOfPokerHands(Hand);

            return RedirectToPage(new {Card0 = Hand.MyCard0Id, Card1 = Hand.MyCard1Id }); // (new { IndexCity = Address.City });//pass anonymous object with city property
        }
    }
}
