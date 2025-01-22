using System;
using UnityEngine;

namespace CardGame.logic.Prepabs.Holdem
{
    public class MyCard : MonoBehaviour
    {
        private int suit;
        private int rank;
        public void setsuit(int Suit)
        {
            suit = Suit;
        }

        public void setrank(int Rank)
        {
            rank = Rank;
        }

        public int getsuit()
        {
            return suit;
        }

        public int getrank()
        {
            return rank;
        }
        
    }
}