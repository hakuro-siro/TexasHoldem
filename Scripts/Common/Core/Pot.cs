using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	//* 이 Class는 Pot의 총량, 최소 배팅, 베팅 라운드 종료, Pot에 참가한 사람, Pot을 가져갈 사람 등을 제어함
	public class Pot
	{
		private PlayerList playersInPot = new PlayerList();
		private int amountInPot;
		private int minimumRaise;
		private int maximumAmountPutIn;

		private int minimumAllInAmount;
		private int playersAllInCount;
		private int amountInPotBeforeAllIn;

		private int agressorIndex;
		private int smallBlind;
		private int bigBlind;

		private bool isBetted;
		private ACTION lastAction;

		public ACTION LastAction
		{
			get { return lastAction; }
			set { lastAction = value; }
		}

		public bool IsBetted
		{
			get { return isBetted; }
			set { isBetted = value; }
		}

		public int SmallBlind
		{
			get { return smallBlind; }
			set { smallBlind = value; }
		}

		public int BigBlind
		{
			get { return bigBlind; }
			set { bigBlind = value; }
		}

		public int MinimumRaise
		{
			get { return minimumRaise; }
			set { minimumRaise = value; }
		}

		public int Amount
		{
			get { return amountInPot; }
			set { if (value < 0) { value = 0; } amountInPot = value; }
		}

		public int MinimumAllInAmount
		{
			get { return minimumAllInAmount; }
			set { if (value < 0) { value = 0; } minimumAllInAmount = value; }
		}

		public int PlayersAllIn
		{
			get { return playersAllInCount; }
			set { if (value < 0) { value = 0; } playersAllInCount = value; }
		}

		public int AmountInPotBeforeAllIn
		{
			get { return amountInPotBeforeAllIn; }
			set { if (value < 0) { value = 0; } amountInPotBeforeAllIn = value; }
		}

		public int AgressorIndex
		{
			get { return agressorIndex; }
			set { agressorIndex = value; }
		}

		//* Pot 구조
		public Pot()
		{
			amountInPot = 0;
			minimumRaise = 0;
			maximumAmountPutIn = 0;
			minimumAllInAmount = 0;
			playersAllInCount = 0;
			amountInPotBeforeAllIn = 0;
			agressorIndex = -1;
			isBetted = false;
			lastAction = ACTION.NULL;
		}

		public Pot(int amount, PlayerList playersInPot)
		{
			this.Amount = amount;
			this.playersInPot = playersInPot;
			minimumAllInAmount = 0;
			playersAllInCount = 0;
			amountInPotBeforeAllIn = 0;
			agressorIndex = -1;
			isBetted = false;
			lastAction = ACTION.NULL;
		}

		//* Getter
		public PlayerList getPlayersInPot()
		{
			return playersInPot;
		}

		//* Player를 Pot에 추가
		public void AddPlayer(Player player)
		{
			if (!playersInPot.Contains(player))
				playersInPot.Add(player);
		}

		//* 돈을 Pot에 추가
		public void Add(int amount)
		{
			if (amount < 0)
				return;
			amountInPot += amount;
		}

		//* Pot의 최댓값을 가져옴
		public int getMaximumAmountPutIn()
		{
			return maximumAmountPutIn;
		}

		//* Pot의 최댓값을 지정함
		public void setMaximumAmount(int amount)
		{
			maximumAmountPutIn = amount;
		}
	}
}
