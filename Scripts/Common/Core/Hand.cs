﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	//* 플레이어의 핸드를 구성하는 카드의 리스트를 위한 클래스
	//* 핸드는 Static Class인 HandCombination을 사용하여 결정되는 handValue를 포함
	//* handValue를 가진 핸드만 비교가 가능
	//* += 연산자로 핸드에 핸드의 추가가 가능

	public class Hand
	{
		private List<Card> myHand;
		private List<int> handValue;

		public Hand()
		{
			myHand = new List<Card>();
			handValue = new List<int>();
		}

		public Hand(Hand otherHand)
		{
			myHand = new List<Card>(otherHand.myHand);
			handValue = new List<int>(otherHand.handValue);
		}

		public Card this[int index]
		{
			get { return myHand[index]; }
			set { myHand[index] = value; }
		}

		public void Clear()
		{
			myHand.Clear();
			handValue.Clear();
		}

		public void Add(Card card)
		{
			myHand.Add(card);
		}

		public void Remove(int index)
		{
			myHand.RemoveAt(index);
		}

		public void Remove(Card card)
		{
			myHand.Remove(card);
		}

		public List<int> getValue()
		{
			return this.handValue;
		}

		public void setValue(int value)
		{
			handValue.Add(value);
		}

		public void setValue(int value, int index)
		{
			handValue[index] = value;
		}

		public int Count()
		{
			return myHand.Count;
		}

		public Card getCard(int index)
		{
			if (index >= myHand.Count)
				throw new ArgumentOutOfRangeException();

			return myHand[index];
		}

		List<Card> QuickSortRank(List<Card> myCards)
		{
			Card pivot;
			Random ran = new Random();

			if (myCards.Count() <= 1)
				return myCards;

			pivot = myCards[ran.Next(myCards.Count())];
			myCards.Remove(pivot);

			var less = new List<Card>();
			var greater = new List<Card>();

			//* 더 작거나 큰 List에 할당
			foreach (Card i in myCards)
			{
				if (i > pivot)
					greater.Add(i);
				else if (i <= pivot)
					less.Add(i);
			}

			//* 더 작은 수의 List와 더 큰 수의 List의 반복
			var list = new List<Card>();

			list.AddRange(QuickSortRank(greater));
			list.Add(pivot);
			list.AddRange(QuickSortRank(less));

			return list;
		}

		List<Card> QuickSortSuit(List<Card> myCards)
		{
			Card pivot;
			Random ran = new Random();

			if (myCards.Count() <= 1)
				return myCards;

			pivot = myCards[ran.Next(myCards.Count())];
			myCards.Remove(pivot);

			var less = new List<Card>();
			var greater = new List<Card>();

			//* 더 작거나 큰 List에 할당
			for (int i = 0; i < myCards.Count(); i++)
			{
				if (myCards[i].getSuit() > pivot.getSuit())
					greater.Add(myCards[i]);
				else if (myCards[i].getSuit() <= pivot.getSuit())
					less.Add(myCards[i]);
			}

			//* 더 작은 수의 List와 더 큰 수의 List의 반복
			var list = new List<Card>();

			list.AddRange(QuickSortSuit(less));
			list.Add(pivot);
			list.AddRange(QuickSortSuit(greater));

			return list;
		}

		public void sortByRank()
		{
			myHand = QuickSortRank(myHand);
		}

		public void sortBySuit()
		{
			myHand = QuickSortSuit(myHand);
		}

		public override string ToString()
		{
			if (this.handValue.Count() == 0)
				return "No Poker Hand is Found";

			switch (this.handValue[0])
			{
				case 1:
					return Card.rankToString(handValue[1]) + " High";
				case 2:
					return "Pair of " + Card.rankToString(handValue[1]) + "s";
				case 3:
					return "Two Pair: " + Card.rankToString(handValue[1]) + "s over " + Card.rankToString(handValue[2]) + "s";
				case 4:
					return "Three " + Card.rankToString(handValue[1]) + "s";
				case 5:
					return Card.rankToString(handValue[1]) + " High Straight";
				case 6:
					return Card.rankToString(handValue[1]) + " High Flush";
				case 7:
					return Card.rankToString(handValue[1]) + "s Full of " + Card.rankToString(handValue[2]) + "s";
				case 8:
					return "Quad " + Card.rankToString(handValue[1]) + "s";
				case 9:
					return Card.rankToString(handValue[1]) + " High Straight Flush";
				default:
					return "Royal Flush";
			}
		}

		//* 핸드가 같은지 체크, Value가 아님
		public bool isEqual(Hand a)
		{
			for (int i = 0; i < a.Count(); i++)
				if (a[i] != myHand[i] || a[i].getSuit() != myHand[i].getSuit())
					return false;
			return true;
		}

		public static bool operator ==(Hand a, Hand b)
		{
			if (a.getValue().Count == 0 || b.getValue().Count == 0)
				throw new NullReferenceException();
			for (int i = 0; i < a.getValue().Count() && i < b.getValue().Count(); i++)
				if (a.getValue()[i] != b.getValue()[i])
					return false;
			return true;
		}

		public static bool operator !=(Hand a, Hand b)
		{
			if (a.getValue().Count == 0 || b.getValue().Count == 0)
				throw new NullReferenceException();
			for (int i = 0; i < a.getValue().Count() && i < b.getValue().Count(); i++)
				if (a.getValue()[i] != b.getValue()[i])
					return true;
			return false;
		}

		public static bool operator <(Hand a, Hand b)
		{
			if (a.getValue().Count == 0 || b.getValue().Count == 0)
				throw new NullReferenceException();
			for (int i = 0; i < a.getValue().Count() && i < b.getValue().Count(); i++)
			{
				if (a.getValue()[i] < b.getValue()[i])
					return true;
				if (a.getValue()[i] > b.getValue()[i])
					return false;
			}
			return false;
		}

		public static bool operator >(Hand a, Hand b)
		{
			if (a.getValue().Count == 0 || b.getValue().Count == 0)
				throw new NullReferenceException();
			for (int i = 0; i < a.getValue().Count() && i < b.getValue().Count(); i++)
			{
				if (a.getValue()[i] > b.getValue()[i])
					return true;
				if (a.getValue()[i] < b.getValue()[i])
					return false;
			}
			return false;
		}

		public static bool operator <=(Hand a, Hand b)
		{
			if (a.getValue().Count == 0 || b.getValue().Count == 0)
				throw new NullReferenceException();
			for (int i = 0; i < a.getValue().Count() && i < b.getValue().Count(); i++)
			{
				if (a.getValue()[i] < b.getValue()[i])
					return true;
				if (a.getValue()[i] > b.getValue()[i])
					return false;
			}
			return true;
		}

		public static bool operator >=(Hand a, Hand b)
		{
			if (a.getValue().Count == 0 || b.getValue().Count == 0)
				throw new NullReferenceException();
			for (int i = 0; i < a.getValue().Count() && i < b.getValue().Count(); i++)
			{
				if (a.getValue()[i] > b.getValue()[i])
					return true;
				if (a.getValue()[i] < b.getValue()[i])
					return false;
			}
			return true;
		}

		public static Hand operator +(Hand a, Hand b)
		{
			for (int i = 0; i < b.Count(); i++)
				a.Add(b[i]);
			return a;
		}
	}
}
