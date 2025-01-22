using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	public enum RANK
	{
		TWO = 2,
		THREE,
		FOUR,
		FIVE,
		SIX,
		SEVEN,
		EIGHT,
		NINE,
		TEN,
		JACK,
		QUEEN,
		KING,
		ACE
	}

	public enum SUIT
	{
		DIAMONDS = 1,
		CLUBS,
		HEARTS,
		SPADES
	}

	public enum SIMBOL
	{
		D = 1,
		C,
		H,
		S
	}

	public class Card
	{
		private int rank, suit;
		private bool faceUp;

		public bool FaceUp
		{
			get { return faceUp; }
			set { faceUp = value; }
		}

		//* 디폴트 값인 다이아 2
		public Card()
		{
			rank = (int)RANK.TWO;
			suit = (int)SUIT.DIAMONDS;
			faceUp = false;
		}

		public Card(RANK rank, SUIT suit)
		{
			this.rank = (int)rank;
			this.suit = (int)suit;
			faceUp = false;
		}

		public Card(int rank, int suit)
		{
			if (rank < 1 || rank > 14 || suit < 1 || suit > 4)
				throw new ArgumentOutOfRangeException();
			this.rank = rank;
			this.suit = suit;
			faceUp = false;
		}

		public Card(RANK rank, SUIT suit, bool faceUp)
		{
			this.rank = (int)rank;
			this.suit = (int)suit;
			this.faceUp = faceUp;
		}

		public Card(int rank, int suit, bool faceUp)
		{
			if (rank < 1 || rank > 14 || suit < 1 || suit > 4)
				throw new ArgumentOutOfRangeException();
			this.rank = rank;
			this.suit = suit;
			this.faceUp = faceUp;
		}

		public Card(Card card)
		{
			this.rank = card.rank;
			this.suit = card.suit;
			this.faceUp = card.faceUp;
		}

		public static string rankToString(int rank)
		{
			switch (rank)
			{
				case 11:
					return "Jack";
				case 12:
					return "Queen";
				case 13:
					return "King";
				case 14:
					return "Ace";
				default:
					return rank.ToString();
			}
		}

		public static string suitToString(int suit)
		{
			switch (suit)
			{
				case 1:
					return "Diamonds";
				case 2:
					return "Clubs";
				case 3:
					return "Hearts";
				default:
					return "Spades";
			}
		}

		public int getRank()
		{
			return rank;
		}

		public int getSuit()
		{
			return suit;
		}

		public void setRank(RANK rank)
		{
			this.rank = (int)rank;
		}

		public void setCard(RANK rank, SUIT suit)
		{
			this.rank = (int)rank;
			this.suit = (int)suit;
		}

		public void setCard(int rank, int suit)
		{
			if (rank < 1 || rank > 14 || suit < 1 || suit > 4)
				throw new ArgumentOutOfRangeException();
			this.rank = rank;
			this.suit = suit;
		}

		//* 카드 랭크 비교
		public static bool operator ==(Card a, Card b)
		{
			if (a.rank == b.rank)
				return true;
			else
				return false;
		}

		public static bool operator !=(Card a, Card b)
		{
			if (a.rank != b.rank)
				return true;
			else
				return false;
		}

		public static bool operator <(Card a, Card b)
		{
			if (a.rank < b.rank)
				return true;
			else
				return false;
		}

		public static bool operator >(Card a, Card b)
		{
			if (a.rank > b.rank)
				return true;
			else
				return false;
		}

		public static bool operator <=(Card a, Card b)
		{
			if (a.rank <= b.rank)
				return true;
			else
				return false;
		}

		public static bool operator >=(Card a, Card b)
		{
			if (a.rank >= b.rank)
				return true;
			else
				return false;
		}
	}
}
