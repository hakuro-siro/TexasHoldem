using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	//* 이 Class는 Dealer Position, Blind 지불, Card 나누기, 각 Player의 선택, 라운드 시작, Showdown 등을 제어함
	public class Table
	{
		private PlayerList players = new PlayerList();
		private Deck deck;
		private Hand tableHand = new Hand();
		private int roundCounter;
		private Pot mainPot;
		private List<Pot> sidePots;
		private Random rand;
		private int turnCount;
		public string winnerMessage;

		private Winner mainPotWinners = new Winner();
		//private List<Winner> sidePotWinners = new List<Winner>();

		//* 이 Class는 Blind의 양, Bilnd를 지불할 Player의 위치 등을 포함함
		private class Blind
		{
			private int amount;
			public int position;
			public int Amount
			{
				get { return amount; }
				set { amount = value; }
			}
		}

		Blind smallBlind;
		Blind bigBlind;
		//* 딜러 Player의 Index
		private int dealerPosition;
		//* 턴 Player의 Index
		private int currentIndex;

		public int TurnCount
		{
			get { return turnCount; }
			set { turnCount = value; }
		}

		public int SmallBlind
		{
			get { return smallBlind.Amount; }
		}

		public int BigBlind
		{
			get { return bigBlind.Amount; }
		}

		public int RoundCount
		{
			get { return roundCounter; }
			set { roundCounter = value; }
		}

		//* 게임 시작, 딜러 위치 무작위 선택, 시작 금액 설정
		public Table(PlayerList players)
		{
			this.players = players;
			deck = new Deck();
			rand = new Random();
			mainPot = new Pot();
			sidePots = new List<Pot>();
			smallBlind = new Blind();
			bigBlind = new Blind();
			roundCounter = 0;
			turnCount = 0;
			dealerPosition = rand.Next(players.Count);
			smallBlind.Amount = 500;
			bigBlind.Amount = 1000;
			mainPot.SmallBlind = 500;
			mainPot.BigBlind = 1000;
			smallBlind.position = dealerPosition + 1;
			bigBlind.position = dealerPosition + 2;
			currentIndex = dealerPosition;
		}

		public Table()
		{
			players = new PlayerList();
			deck = new Deck();
			rand = new Random();
			mainPot = new Pot();
			sidePots = new List<Pot>();
			smallBlind = new Blind();
			bigBlind = new Blind();
			roundCounter = 0;
			turnCount = 0;
			dealerPosition = rand.Next(players.Count);
			smallBlind.Amount = 500;
			bigBlind.Amount = 1000;
			mainPot.SmallBlind = 500;
			mainPot.BigBlind = 1000;
			smallBlind.position = dealerPosition + 1;
			bigBlind.position = dealerPosition + 2;
			currentIndex = dealerPosition;
		}

		//* Player Indexer
		public Player this[int index]
		{
			get { return players.GetPlayer(ref index); }
			set { players[index] = value; }
		}

		public PlayerList getPlayers()
		{
			return players;
		}

		public int getDealerPosition()
		{
			return dealerPosition;
		}

		public int getCurrentIndex()
		{
			return currentIndex;
		}

		public void setCurrentIndex(int index)
		{
			currentIndex = index;
		}

		public string getSmallBlind()
		{
			return smallBlind.Amount.ToString();
		}

		public string getBigBlind()
		{
			return bigBlind.Amount.ToString();
		}

		public Pot getPot()
		{
			return mainPot;
		}

		public List<Pot> getSidePots()
		{
			return sidePots;
		}

		public Hand getCommunityCards()
		{
			return tableHand;
		}

		public Deck getDeck()
		{
			return deck;
		}

		public Winner getMainPotWinners()
		{
			return mainPotWinners;
		}

		//public List<Winner> getSidePotWinners()
		//{
		//	return sidePotWinners;
		//}

		//* Player가 Bust 했을 때 삭제
		public void RemovePlayer(Player player)
		{
			if (player.ChipStack != 0)
				throw new InvalidOperationException();
			players.Remove(player);
		}

		public void RemovePlayer(int index)
		{
			if (players[index].ChipStack != 0)
				throw new InvalidOperationException();
			players.RemoveAt(index);
		}

		//* 새 라운드 시작
		//* 딜러, Small Bilnd 위치 이동
		//* player/counter 변수 재설정
		//* 필요한 경우, Blind 재설정
		public void startNextMatch()
		{
			players.ResetPlayers();
			deck = new Deck();

			if (roundCounter == 10)
			{
				roundCounter = 0;
				smallBlind.Amount *= 2;
				bigBlind.Amount = smallBlind.Amount * 2;
				mainPot.SmallBlind = SmallBlind;
				mainPot.BigBlind = BigBlind;
			}
			if (roundCounter != 0)
			{
				dealerPosition = incrementIndex(dealerPosition);
				smallBlind.position = incrementIndex(dealerPosition);
				bigBlind.position = incrementIndex(smallBlind.position);
			}

			roundCounter++;
			mainPot.Amount = 0;
			mainPot.AgressorIndex = -1;
			mainPot.MinimumRaise = bigBlind.Amount;
			tableHand.Clear();
			currentIndex = dealerPosition;
			winnerMessage = null;
			mainPot.getPlayersInPot().Clear();
			sidePots.Clear();
		}

		//* 현재 라운드가 언제 종료될지 결정
		public bool beginNextTurn()
		{
			turnCount++;

			while (players[mainPot.AgressorIndex].IsFolded() && currentIndex != mainPot.AgressorIndex)
				mainPot.AgressorIndex = decrementIndex(mainPot.AgressorIndex);

			if (currentIndex == mainPot.AgressorIndex && turnCount > 1)
			{
				Console.WriteLine("beginNextTurn : currentIndex == mainPot.AgressorIndex && turnCount > 1");
				return false;
			}
			else if (EveryoneAllIn())
			{
				Console.WriteLine("beginNextTurn : EveryoneAllIn");
				return false;
			}
			else
				return true;
		}

		//* 모든 Player가 AllIn한걸 제어
		public bool EveryoneAllIn()
		{
			int zeroCount = 0;
			int totalCount = 0;

			for (int i = 0; i < getPlayers().Count; i++)
			{
				if (this[i].isBusted || this[i].IsFolded())
					continue;
				if (this[i].ChipStack == 0)
					zeroCount++;
				totalCount++;
			}

			if (zeroCount != 0 && totalCount == zeroCount)
				return true;

			//else if (totalCount - zeroCount == 1)
			//{
			//	for (int i = 0; i < getPlayers().Count; i++)
			//	{
			//		if (this[i].isBusted || this[i].IsFolded())
			//			continue;
			//		if (this[i].ChipStack != 0 && this[i].AmountInPot == 0)
			//			return true;
			//	}
			//}

			return false;
		}

		//* Index 증가, Fold, Bust 등 한 Player 넘기기
		public int incrementIndex(int currentIndex)
		{
			currentIndex++;

			while (players.GetPlayer(ref currentIndex).IsFolded() ||
				   players.GetPlayer(ref currentIndex).isBusted ||
				   players.GetPlayer(ref currentIndex).ChipStack == 0)
			{
				if (EveryoneAllIn())
					break;
				currentIndex++;
			}

			players.GetPlayer(ref currentIndex);

			return currentIndex;
		}

		//* Index 중가, ChipStack이 0인 Player를 넘기지 않음
		public int incrementIndexShowdown(int currentIndex)
		{
			currentIndex++;

			while (players.GetPlayer(ref currentIndex).IsFolded() ||
				   players.GetPlayer(ref currentIndex).isBusted)
				currentIndex++;

			players.GetPlayer(ref currentIndex);

			return currentIndex;
		}

		public int decrementIndex(int currentIndex)
		{
			currentIndex--;

			while (players.GetPlayer(ref currentIndex).IsFolded() ||
				   players.GetPlayer(ref currentIndex).isBusted ||
				   players.GetPlayer(ref currentIndex).ChipStack == 0)
			{
				if (EveryoneAllIn())
					break;
				currentIndex--;
			}

			players.GetPlayer(ref currentIndex);

			return currentIndex;
		}

		//* 각 선수에게 2장의 Card를 Deal
		public void DealHoleCards()
		{
			deck.Shuffle();

			for (int i = 0; i < players.Count; i++)
			{
				if (i == 0)
				{
					players[i].AddToHand(deck.Deal());
					players[i].AddToHand(deck.Deal());
				}
				else
				{
					players[i].AddToHand(deck.Deal(false));
					players[i].AddToHand(deck.Deal(false));
				}
			}
		}

		//* Small Blind / Big Blind 지불
		public void PaySmallBlind()
		{
			currentIndex = smallBlind.position % players.Count;
			players.GetPlayer(ref smallBlind.position).PaySmallBlind(smallBlind.Amount, mainPot, currentIndex, this);
		}

		public void PayBigBlind()
		{
			currentIndex = bigBlind.position % players.Count;
			players.GetPlayer(ref bigBlind.position).PayBigBlind(bigBlind.Amount, mainPot, currentIndex, this);
			turnCount = 0;
		}

		//* Flop
		public void DealFlop()
		{
			for (int i = 0; i < 3; i++)
				tableHand.Add(deck.Deal());

			for (int i = 0; i < players.Count; i++)
				players[i].AddToHand(tableHand);

			mainPot.IsBetted = false;
			mainPot.LastAction = ACTION.NULL;
		}

		//* Turn
		public void DealTurn()
		{
			Card turn = deck.Deal();
			tableHand.Add(turn);

			for (int i = 0; i < players.Count; i++)
				players[i].AddToHand(turn);

			mainPot.IsBetted = false;
			mainPot.LastAction = ACTION.NULL;
		}

		//* River
		public void DealRiver()
		{
			Card river = deck.Deal();
			tableHand.Add(river);

			for (int i = 0; i < players.Count; i++)
				players[i].AddToHand(river);

			mainPot.IsBetted = false;
			mainPot.LastAction = ACTION.NULL;
		}

		//* Showdown
		public void Showdown()
		{
			//* Side Pot 생성
			//* All In 한 Player가 1명 이상일 경우, 그 Player가 Bet 한 총 Pot까지는 Main Pot이 되고, 나머지가 겨루는 Pot이 Side Pot
			//* 핸드도 따로 비교
			//* All In Player가 A One Pair, Side Pot에서 비교한 두 Player가 K One Pair, J One Pair 일 때
			//* All In Player는 Main Pot을 모두 가져가고 Side Pot은 K One Pair Player가 소유
			//* All In Player가 Side Pot Player에게 졌을 경우 Side Pot을 이긴 플레이어가 Main Pot까지 모두 소유
			//* All In Player와 Side Pot Player 중 한명이 비겼다면 Main Pot은 Split, Side Pot은 여전히 Side Pot Player의 소유

			//if (CreateSidePots())
			//{
			//	mainPot.getPlayersInPot().Sort();

			//	for (int i = 0; i < mainPot.getPlayersInPot().Count - 1; i++)
			//	{
			//		if (mainPot.getPlayersInPot()[i].AmountInPot != mainPot.getPlayersInPot()[i + 1].AmountInPot)
			//		{
			//			PlayerList tempPlayers = new PlayerList();

			//			for (int j = mainPot.getPlayersInPot().Count - 1; j > i; j--)
			//				tempPlayers.Add(mainPot.getPlayersInPot()[j]);

			//			int potSize = (mainPot.getPlayersInPot()[i + 1].AmountInPot - mainPot.getPlayersInPot()[i].AmountInPot) * tempPlayers.Count;
			//			mainPot.Amount -= potSize;
			//			sidePots.Add(new Pot(potSize, tempPlayers));
			//		}
			//	}
			//}

			//* Main Pot 지급
			PlayerList bestHandList = QuickSortBestHand(new PlayerList(mainPot.getPlayersInPot()));

			for (int i = 0; i < bestHandList.Count; i++)
				bestHandList[i].myHand = HandCombination.getBestHand(bestHandList[i].getHand());

			for (int i = 0; i < bestHandList.Count; i++)
			{
				for (int j = 0; j < this.getPlayers().Count; j++)
					if (players[j] == bestHandList[i])
					{
						mainPotWinners.getWinners().Add(bestHandList[i]);
						mainPotWinners.getWinnerIndex().Add(j);
					}

				if (HandCombination.getBestHand(new Hand(bestHandList[i].getHand())) != HandCombination.getBestHand(new Hand(bestHandList[i + 1].getHand())))
					break;
			}

			if (mainPotWinners.getWinners().Count != 0)
				mainPot.Amount /= mainPotWinners.getWinners().Count;
			else
				return;

			if (mainPotWinners.getWinners().Count > 1)
			{
				for (int i = 0; i < this.getPlayers().Count; i++)
				{
					if (mainPotWinners.getWinners().Contains(this.getPlayers()[i]))
					{
						currentIndex = i;
						players[i].CollectMoney(mainPot);
						winnerMessage += players[i].Name + ", ";
					}
				}
				winnerMessage += Environment.NewLine + " split the pot.";
			}
			else
			{
				currentIndex = mainPotWinners.getWinnerIndex()[0];
				players[currentIndex].CollectMoney(mainPot);
				winnerMessage = players[currentIndex].Message;
			}

			//* Side Pot 지급
			//for (int i = 0; i < sidePots.Count; i++)
			//{
			//	List<int> sidePotWinners = new List<int>();

			//	for (int x = 0; x < bestHandList.Count; x++)
			//	{
			//		for (int j = 0; j < this.getPlayers().Count; j++)
			//			if (players[j] == bestHandList[x] && sidePots[i].getPlayersInPot().Contains(bestHandList[x]))
			//				sidePotWinners.Add(j);

			//		if (HandCombination.getBestHand(new Hand(bestHandList[x].getHand())) != HandCombination.getBestHand(new Hand(bestHandList[x + 1].getHand())) && sidePotWinners.Count != 0)
			//			break;
			//	}

			//	sidePots[i].Amount /= sidePotWinners.Count;

			//	for (int j = 0; j < this.getPlayers().Count; j++)
			//	{
			//		if (sidePotWinners.Contains(j))
			//		{
			//			currentIndex = j;
			//			players[j].CollectMoney(sidePots[i]);
			//		}
			//	}
			//}

			mainPot.Amount = 0;
		}

		//* Side Pot 생성이 필요한지 체크
		//private bool CreateSidePots()
		//{
		//	for (int i = 0; i < mainPot.getPlayersInPot().Count() - 1; i++)
		//		if (mainPot.getPlayersInPot()[i].AmountInPot != mainPot.getPlayersInPot()[i + 1].AmountInPot)
		//			return true;
		//	return false;
		//}

		PlayerList QuickSortBestHand(PlayerList myPlayers)
		{
			Player pivot;
			Random ran = new Random();

			if (myPlayers.Count() <= 1)
				return myPlayers;

			pivot = myPlayers[ran.Next(myPlayers.Count())];
			myPlayers.Remove(pivot);

			var less = new PlayerList();
			var greater = new PlayerList();

			//* 더 작거나 큰 List에 할당
			foreach (Player player in myPlayers)
			{
				if (HandCombination.getBestHand(new Hand(player.getHand())) > HandCombination.getBestHand(new Hand(pivot.getHand())))
					greater.Add(player);
				else if (HandCombination.getBestHand(new Hand(player.getHand())) <= HandCombination.getBestHand(new Hand(pivot.getHand())))
					less.Add(player);
			}

			//* 더 작은 수의 List와 더 큰 수의 List의 반복
			var list = new PlayerList();

			list.AddRange(QuickSortBestHand(greater));
			list.Add(pivot);
			list.AddRange(QuickSortBestHand(less));

			return list;
		}

		//* 1명을 제외한 모든 Player가 Fold 했는지 체크
		public bool PlayerWon()
		{
			int foldCount = 0;

			foreach (Player player in this)
			{
				if (player.isBusted)
					continue;
				if (player.IsFolded())
					foldCount++;
			}

			if (foldCount == this.getPlayers().Count - 1)
				return true;

			return false;
		}

		//* foreach support
		public IEnumerator<Player> GetEnumerator()
		{
			return players.GetEnumerator();
		}
	}
}
