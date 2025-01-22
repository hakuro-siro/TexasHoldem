using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	//* 7장의 카드에서 가장 높은 족보를 추출
	//* 또한 핸드를 비교하는데 필요한 정보를 결정
	//* 모든 핸드가 정렬됨

	public static class HandCombination
	{
		public static Hand getBestHand(Hand hand)
		{
			if (hand.Count() < 5)
			{
				hand.Clear();
				return hand;
			}
			if (isRoyalFlush(hand))
				return getRoyalFlush(hand);
			if (isStraightFlush(hand))
				return getStraightFlush(hand);
			if (isQuads(hand))
				return getQuads(hand);
			if (isFullHouse(hand))
				return getFullHouse(hand);
			if (isFlush(hand))
				return getFlush(hand);
			if (isStraight(hand))
				return getStraight(hand);
			if (isTrips(hand))
				return getTrips(hand);
			if (isTwoPair(hand))
				return getTwoPair(hand);
			if (isOnePair(hand))
				return getOnePair(hand);
			return getHighCard(hand);
		}

		//* StraigthFlush가 Royal Flush를 커버하기에 isRoyalFlush를 실행하지 않고도 판별 가능
		public static Hand getBestHandEfficiently(Hand hand)
		{
			if (hand.Count() < 5)
			{
				hand.Clear();
				return hand;
			}
			if (isStraightFlush(hand))
				return getStraightFlush(hand);
			if (isQuads(hand))
				return getQuads(hand);
			if (isFullHouse(hand))
				return getFullHouse(hand);
			if (isFlush(hand))
				return getFlush(hand);
			if (isStraight(hand))
				return getStraight(hand);
			if (isTrips(hand))
				return getTrips(hand);
			if (isTwoPair(hand))
				return getTwoPair(hand);
			if (isOnePair(hand))
				return getOnePair(hand);
			return getHighCard(hand);

		}

		//* 재귀를 사용하여 Pair를 제거하고 Royal Flush 판별
		public static bool isRoyalFlush(Hand hand)
		{
			hand.sortByRank();
			//* 핸드와 동일하게 세팅
			//* 이 핸드에서 카드를 제거하여 Pair나 Trips의 간섭 없이 Straight 판별
			Hand simplifiedhand1;
			Hand simplifiedhand2;

			for (int i = 0; i <= hand.Count() - 2; i++)
			{
				if (hand[i] == hand[i + 1] && hand.Count() > 5)
				{
					simplifiedhand1 = new Hand(hand);
					simplifiedhand1.Remove(i);
					simplifiedhand2 = new Hand(hand);
					simplifiedhand2.Remove(i + 1);
					if (HandCombination.isRoyalFlush(simplifiedhand1))
						return true;
					if (HandCombination.isRoyalFlush(simplifiedhand2))
						return true;
				}
			}

			int currentSuit = hand.getCard(0).getSuit();

			int checkIf = 0;

			for (int i = 0; i < 5; i++)
				if (hand.getCard(i).getRank() == 14 - i)
					checkIf++;
			for (int i = 1; i < 5; i++)
				if (hand.getCard(i).getSuit() == currentSuit)
					checkIf++;

			// if (hand.getCard(0).getRank() == 14 && hand.getCard(1).getRank() == 13 && hand.getCard(2).getRank() == 12 && hand.getCard(3).getRank() == 11 && hand.getCard(4).getRank() == 10 && hand.getCard(1).getSuit() == currentSuit && hand.getCard(2).getSuit() == currentSuit && hand.getCard(3).getSuit() == currentSuit && hand.getCard(4).getSuit() == currentSuit) return true;

			if (checkIf == 9)
				return true;
			else
				return false;
		}

		//* 재귀를 사용하여 Pair를 제거하고 Royal Flush 판별
		public static Hand getRoyalFlush(Hand hand)
		{
			hand.sortByRank();
			Hand straightFlush = new Hand(HandCombination.getStraightFlush(hand));
			straightFlush.setValue(10, 0);

			if (straightFlush.getCard(0).getRank() == 14)
				return straightFlush;
			else
			{
				straightFlush.Clear();
				return straightFlush;
			}
		}

		//* 재귀를 사용하여 pair를 제거한 후, Straight Flush 판별
		public static bool isStraightFlush(Hand hand)
		{
			hand.sortByRank();
			//* 핸드와 동일하게 세팅
			//* 이 핸드에서 카드를 제거하여 Pair나 Trips의 간섭 없이 Straight 판별
			Hand simplifiedhand1;
			Hand simplifiedhand2;

			for (int i = 0; i <= hand.Count() - 2; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1))
				{
					simplifiedhand1 = new Hand(hand);
					simplifiedhand1.Remove(i);
					simplifiedhand2 = new Hand(hand);
					simplifiedhand2.Remove(i + 1);
					if (HandCombination.isStraightFlush(simplifiedhand1))
						return true;
					if (HandCombination.isStraightFlush(simplifiedhand2))
						return true;
				}
			}

			for (int i = 0; i <= hand.Count() - 5; i++)
			{
				int currentRank = hand.getCard(i).getRank();
				int currentSuit = hand.getCard(i).getSuit();

				int checkIf = 0;

				for (int j = 1; j <= 4; j++)
					if (currentRank == hand.getCard(i + j).getRank() + j)
						checkIf++;
				for (int j = 1; j <= 4; j++)
					if (currentSuit == hand.getCard(i + j).getSuit())
						checkIf++;

				// if (currentRank == hand.getCard(i + 1).getRank() + 1 && currentRank == hand.getCard(i + 2).getRank() + 2 && currentRank == hand.getCard(i + 3).getRank() + 3 && currentRank == hand.getCard(i + 4).getRank() + 4 && currentSuit == hand.getCard(i + 1).getSuit() && currentSuit == hand.getCard(i + 2).getSuit() && currentSuit == hand.getCard(i + 3).getSuit() && currentSuit == hand.getCard(i + 4).getSuit()) return true;

				if (checkIf == 8)
					return true;
			}

			for (int i = 0; i <= hand.Count() - 4; i++)
			{
				int currentRank = hand.getCard(i).getRank();
				int currentSuit = hand.getCard(i).getSuit();

				if (currentRank == 5 &&
					hand.getCard(i + 1).getRank() == 4 &&
					hand.getCard(i + 2).getRank() == 3 &&
					hand.getCard(i + 3).getRank() == 2 &&
					hand.getCard(0).getRank() == 14 &&
					currentSuit == hand.getCard(i + 1).getSuit() &&
					currentSuit == hand.getCard(i + 2).getSuit() &&
					currentSuit == hand.getCard(i + 3).getSuit() &&
					currentSuit == hand.getCard(0).getSuit())
					return true;
			}

			return false;
		}

		//* 두 포인터 사용으로 Straight Flush 판별 및 모든 케이스 작업
		public static Hand getStraightFlush(Hand hand)
		{
			hand.sortByRank();
			Hand straightFlush = new Hand();
			straightFlush.setValue(9);

			if (hand.getCard(0).getRank() == 14)
				hand.Add(new Card((int)RANK.ACE, hand.getCard(0).getSuit()));

			straightFlush.Add(hand.getCard(0));
			int ptr1 = 0;
			int ptr2 = 1;

			while (ptr1 < hand.Count() - 2 || ptr2 < hand.Count())
			{
				if (straightFlush.Count() >= 5)
					break;
				int rank1 = hand.getCard(ptr1).getRank();
				int rank2 = hand.getCard(ptr2).getRank();
				int suit1 = hand.getCard(ptr1).getSuit();
				int suit2 = hand.getCard(ptr2).getSuit();

				if (rank1 - rank2 == 1 &&
				   suit1 == suit2)
				{
					straightFlush.Add(hand.getCard(ptr2));
					ptr1 = ptr2;
					ptr2++;
				}
				else if (rank1 == 2 &&
						rank2 == 14 &&
						suit1 == suit2)
				{
					straightFlush.Add(hand.getCard(ptr2));
					ptr1 = ptr2;
					ptr2++;
				}
				else
				{
					if (rank1 - rank2 <= 1)
						ptr2++;
					else
					{
						straightFlush.Clear();
						straightFlush.setValue(9);
						ptr1++;
						ptr2 = ptr1 + 1;
						straightFlush.Add(hand.getCard(ptr1));
					}
				}
			}

			if (hand.getCard(0).getRank() == 14)
				hand.Remove(hand.Count() - 1);

			straightFlush.setValue(straightFlush.getCard(0).getRank());

			if (straightFlush.Count() < 5)
				straightFlush.Clear();

			return straightFlush;
		}

		//* 단순히 배열을 반복하며 쌍을 확인
		//* Trips, Full House, Two Pairs, One Pair와 같은 방식
		public static bool isQuads(Hand hand)
		{
			hand.sortByRank();

			for (int i = 0; i <= hand.Count() - 4; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i) == hand.getCard(i + 2) &&
					hand.getCard(i) == hand.getCard(i + 3))
					return true;
			}
			return false;
		}

		public static Hand getQuads(Hand hand)
		{
			hand.sortByRank();
			Hand quads = new Hand();
			quads.setValue(8);

			for (int i = 0; i <= hand.Count() - 4; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i) == hand.getCard(i + 2) &&
					hand.getCard(i) == hand.getCard(i + 3))
				{
					quads.Add(hand.getCard(i));
					quads.Add(hand.getCard(i + 1));
					quads.Add(hand.getCard(i + 2));
					quads.Add(hand.getCard(i + 3));
					quads.setValue(hand.getCard(i).getRank());
					break;
				}
			}

			return getKickers(hand, quads);
		}

		public static bool isFullHouse(Hand hand)
		{
			hand.sortByRank();
			bool trips = false;
			bool pair = false;
			int tripsRank = 0;

			for (int i = 0; i <= hand.Count() - 3; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i) == hand.getCard(i + 2))
				{
					trips = true;
					tripsRank = hand.getCard(i).getRank();
					break;
				}
			}
			for (int i = 0; i <= hand.Count() - 2; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1) &&
				   hand.getCard(i).getRank() != tripsRank)
				{
					pair = true;
					break;
				}
			}

			if (trips == true && pair == true)
				return true;
			else
				return false;
		}

		public static Hand getFullHouse(Hand hand)
		{
			hand.sortByRank();
			Hand fullHouse = new Hand();
			fullHouse.setValue(7);
			bool trips = false;
			bool pair = false;
			int tripsRank = 0;

			for (int i = 0; i <= hand.Count() - 3; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i) == hand.getCard(i + 2))
				{
					trips = true;
					tripsRank = hand.getCard(i).getRank();
					fullHouse.Add(hand.getCard(i));
					fullHouse.Add(hand.getCard(i + 1));
					fullHouse.Add(hand.getCard(i + 2));
					fullHouse.setValue(hand.getCard(i).getRank());
					break;
				}
			}
			for (int i = 0; i <= hand.Count() - 2; i++)
			{
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i).getRank() != tripsRank)
				{
					pair = true;
					fullHouse.Add(hand.getCard(i));
					fullHouse.Add(hand.getCard(i + 1));
					fullHouse.setValue(hand.getCard(i).getRank());
					break;
				}
			}

			if (trips == true && pair == true)
				return fullHouse;
			else
			{
				fullHouse.Clear();
				return fullHouse;
			}
		}

		//* suitCount를 사용하여 suitCount가 5일 경우 Flush 판별
		public static bool isFlush(Hand hand)
		{
			hand.sortByRank();
			int[] suitCount = { 0, 0, 0, 0 };

			for (int i = 0; i < hand.Count(); i++)
			{
				if ((SUIT)hand.getCard(i).getSuit() == SUIT.DIAMONDS)
					suitCount[(int)SUIT.DIAMONDS - 1]++;
				else if ((SUIT)hand.getCard(i).getSuit() == SUIT.CLUBS)
					suitCount[(int)SUIT.CLUBS - 1]++;
				else if ((SUIT)hand.getCard(i).getSuit() == SUIT.HEARTS)
					suitCount[(int)SUIT.HEARTS - 1]++;
				else if ((SUIT)hand.getCard(i).getSuit() == SUIT.SPADES)
					suitCount[(int)SUIT.SPADES - 1]++;
			}

			for (int i = 0; i < 4; i++)
				if (suitCount[i] >= 5)
					return true;

			return false;
		}

		//* suitCount를 사용하여 Flush 판별
		//* 이후, suit의 모든 카드를 가져옴
		public static Hand getFlush(Hand hand)
		{
			hand.sortByRank();
			Hand flush = new Hand();
			flush.setValue(6);
			int[] suitCount = { 0, 0, 0, 0 };

			for (int i = 0; i < hand.Count(); i++)
			{
				if ((SUIT)hand.getCard(i).getSuit() == SUIT.DIAMONDS)
					suitCount[(int)SUIT.DIAMONDS - 1]++;
				else if ((SUIT)hand.getCard(i).getSuit() == SUIT.CLUBS)
					suitCount[(int)SUIT.CLUBS - 1]++;
				else if ((SUIT)hand.getCard(i).getSuit() == SUIT.HEARTS)
					suitCount[(int)SUIT.HEARTS - 1]++;
				else if ((SUIT)hand.getCard(i).getSuit() == SUIT.SPADES)
					suitCount[(int)SUIT.SPADES - 1]++;
			}

			for (int i = 0; i < 4; i++)
			{
				if (suitCount[i] >= 5)
				{
					for (int j = 0; j < hand.Count(); j++)
					{
						if (hand.getCard(j).getSuit() == i + 1)
						{
							flush.Add(hand.getCard(j));
							flush.setValue(hand.getCard(i).getRank());
						}
						if (flush.Count() == 5)
							break;
					}
					if (flush.Count() == 5)
						break;
				}
			}

			return flush;
		}

		public static bool isStraight(Hand hand)
		{
			hand.sortByRank();

			if (hand.getCard(0).getRank() == 14)
				hand.Add(new Card((int)RANK.ACE, hand.getCard(0).getSuit()));

			int straightCount = 1;

			for (int i = 0; i <= hand.Count() - 2; i++)
			{
				//* 이미 Straight가 구성되었으면, Break
				if (straightCount == 5)
					break;

				int currentRank = hand.getCard(i).getRank();

				//* 만약 suit가 1만큼 다르면, straight를 증가
				if (currentRank - hand.getCard(i + 1).getRank() == 1)
					straightCount++;

				//* 2-A에 대한 특정 조건
				else if (currentRank == 2 && hand.getCard(i + 1).getRank() == 14)
					straightCount++;

				//* suit가 1보다 더 다르면, straight를 1로 초기화
				else if (currentRank - hand.getCard(i + 1).getRank() > 1)
					straightCount = 1;

				//* 만약 suit가 다르지 않을 시, 아무것도 하지 않음
			}

			if (hand.getCard(0).getRank() == 14)
				hand.Remove(hand.Count() - 1);

			//* straightCount에 따라 true/false return
			if (straightCount == 5)
				return true;

			return false;
		}

		public static Hand getStraight(Hand hand)
		{
			hand.sortByRank();
			Hand straight = new Hand();
			straight.setValue(5);

			if (hand.getCard(0).getRank() == 14)
				hand.Add(new Card((int)RANK.ACE, hand.getCard(0).getSuit()));

			int straightCount = 1;

			straight.Add(hand.getCard(0));

			for (int i = 0; i <= hand.Count() - 2; i++)
			{
				//* 이미 Straight가 구성되었으면, Break
				if (straightCount == 5)
					break;

				int currentRank = hand.getCard(i).getRank();

				//* 만약 suit가 1만큼 다르면, straight를 증가
				if (currentRank - hand.getCard(i + 1).getRank() == 1)
				{
					straightCount++;
					straight.Add(hand.getCard(i + 1));
				}

				//* 2-A에 대한 특정 조건
				else if (currentRank == 2 && hand.getCard(i + 1).getRank() == 14)
				{
					straightCount++;
					straight.Add(hand.getCard(i + 1));
				}

				//* suit가 1보다 더 다르면, straight를 1로 초기화
				else if (currentRank - hand.getCard(i + 1).getRank() > 1)
				{
					straightCount = 1;
					straight.Clear();
					straight.setValue(5);
					straight.Add(hand.getCard(i + 1));
				}

				//* 만약 suit가 다르지 않을 시, 아무것도 하지 않음
			}

			//* straightCount에 따라 true/false return
			if (hand.getCard(0).getRank() == 14)
				hand.Remove(hand.Count() - 1);
			if (straightCount != 5)
				straight.Clear();

			straight.setValue(straight.getCard(0).getRank());

			return straight;
		}

		public static bool isTrips(Hand hand)
		{
			hand.sortByRank();

			for (int i = 0; i <= hand.Count() - 3; i++)
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i) == hand.getCard(i + 2))
					return true;

			return false;
		}

		public static Hand getTrips(Hand hand)
		{
			hand.sortByRank();
			Hand trips = new Hand();
			trips.setValue(4);

			for (int i = 0; i <= hand.Count() - 3; i++)
				if (hand.getCard(i) == hand.getCard(i + 1) &&
					hand.getCard(i) == hand.getCard(i + 2))
				{
					trips.setValue(hand.getCard(i).getRank());
					trips.Add(hand.getCard(i));
					trips.Add(hand.getCard(i + 1));
					trips.Add(hand.getCard(i + 2));
					break;
				}

			return getKickers(hand, trips);
		}

		public static bool isTwoPair(Hand hand)
		{
			hand.sortByRank();
			int pairCount = 0;

			for (int i = 0; i <= hand.Count() - 2; i++)
				if (hand.getCard(i) == hand.getCard(i + 1))
				{
					pairCount++;
					i++;    //* Pair는 이미 체크 되었음 Two Pair를 Trips로 인식하는 문제를 막기 위하여, i를 추가적으로 증가
				}

			if (pairCount >= 2)
				return true;
			else
				return false;
		}

		public static Hand getTwoPair(Hand hand)
		{
			hand.sortByRank();
			Hand twoPair = new Hand();
			twoPair.setValue(3);
			int pairCount = 0;

			for (int i = 0; i <= hand.Count() - 2; i++)
				if (hand.getCard(i) == hand.getCard(i + 1))
				{
					twoPair.setValue(hand.getCard(i).getRank());
					twoPair.Add(hand.getCard(i));
					twoPair.Add(hand.getCard(i + 1));
					pairCount++;

					if (pairCount == 2)
						break;
					i++;    //* Pair는 이미 체크 되었음 Two Pair를 Trips로 인식하는 문제를 막기 위하여, i를 추가적으로 증가
				}

			if (pairCount == 2)
				return getKickers(hand, twoPair);
			else
			{
				twoPair.Clear();
				return twoPair;
			}
		}

		public static bool isOnePair(Hand hand)
		{
			hand.sortByRank();

			for (int i = 0; i <= hand.Count() - 2; i++)
				if (hand.getCard(i) == hand.getCard(i + 1))
					return true;

			return false;
		}

		public static Hand getOnePair(Hand hand)
		{
			hand.sortByRank();
			Hand onePair = new Hand();
			onePair.setValue(2);

			for (int i = 0; i <= hand.Count() - 2; i++)
				if (hand.getCard(i) == hand.getCard(i + 1))
				{
					onePair.setValue(hand.getCard(i).getRank());
					onePair.Add(hand.getCard(i));
					onePair.Add(hand.getCard(i + 1));
					break;
				}

			return getKickers(hand, onePair);
		}

		public static bool isHighCard(Hand hand)
		{
			return true;
		}

		//* 정렬 이후 가장 높은 카드 가져옴
		public static Hand getHighCard(Hand hand)
		{
			hand.sortByRank();
			Hand highCard = new Hand();
			highCard.setValue(1);
			highCard.Add(hand.getCard(0));
			highCard.setValue(hand.getCard(0).getRank());

			return getKickers(hand, highCard);
		}

		//* 5장의 구성에 필요한 경우, 남은 모든 카드를 가져옴
		private static Hand getKickers(Hand hand, Hand specialCards)
		{
			if (specialCards.Count() == 0)
				return specialCards;

			for (int i = 0; i < specialCards.Count(); i++)
				hand.Remove(specialCards.getCard(i));

			for (int i = 0; i < hand.Count(); i++)
			{
				if (specialCards.Count() >= 5)
					break;
				specialCards.Add(hand.getCard(i));
				specialCards.setValue(hand.getCard(i).getRank());
			}

			return specialCards;
		}
	}
}