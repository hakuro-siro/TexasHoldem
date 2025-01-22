using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;

namespace Holdem
{
	public enum ACTION
	{
		NULL = 0,
		FOLD,
		CHECK,
		CALL,
		RAISE,
		BET,
		ALLIN
	}

	//* 각각의 플레이어들의 Money나 각종 액션에 대한 내용 포함
	public class Player : INotifyPropertyChanged
	{
		public Hand myHand = new Hand();
		protected string name;
		protected int chipStack;
		protected int amountInPot;
		protected bool folded;
		protected int amountContributed;
		protected int initialStack;
		protected string message;
		protected string simplifiedMessage;
		public bool isBusted;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int ChipStack
		{
			get { return chipStack; }
			set
			{
				if (value < 0)
					value = 0;
				chipStack = value;
				InvokePropertyChanged(new PropertyChangedEventArgs("ChipStack"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, e);
		}

		public int AmountContributed
		{
			get { return amountContributed; }
			set { amountContributed = value; }
		}

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		public string SimplifiedMessage
		{
			get { return simplifiedMessage; }
			set { simplifiedMessage = value; InvokePropertyChanged(new PropertyChangedEventArgs("SimplifiedMessage")); }
		}

		public int InitialStack
		{
			get { return initialStack; }
			set { if (value < 0) { value = 0; } initialStack = value; }
		}

		public int AmountInPot
		{
			get { return amountInPot; }
			set { if (value < 0) { value = 0; } amountInPot = value; }
		}

		//* Player name과 chipstack의 구조
		public Player()
		{
			this.name = "You";
			ChipStack = 10000;
			amountInPot = 0;
			initialStack = ChipStack;
			folded = false;
			message = "No decision has been made";
			simplifiedMessage = "";
			isBusted = false;
		}

		public Player(int buyInAmount)
		{
			this.name = "You";
			ChipStack = buyInAmount;
			initialStack = ChipStack;
			amountInPot = 0;
			folded = false;
			message = "No decision has been made";
			simplifiedMessage = "";
			isBusted = false;
		}

		public Player(string name, int buyInAmount)
		{
			if (name == "") { name = "You"; }

			this.name = name;
			ChipStack = buyInAmount;
			initialStack = ChipStack;
			amountInPot = 0;
			folded = false;
			message = "No decision has been made";
			simplifiedMessage = "";
			isBusted = false;
		}

		public Hand getHand()
		{
			return myHand;
		}

		//* hands에 hand 추가
		public void AddToHand(Hand hand)
		{
			myHand += hand;
		}

		public void AddToHand(Card card)
		{
			myHand.Add(card);
		}

		//* Small Blind와 Big Blind만큼 지불
		//* 일부 함수에 라운드를 계속하기 위한 AgressorIndex를 reset 하는데 필요한 Index가 있음
		public void PaySmallBlind(int amount, Pot mainPot, int index, Table table)
		{
			if (ChipStack <= amount)
			{
				AllIn(mainPot, table.decrementIndex(index), table);
				return;
			}
			ChipStack -= amount;
			amountInPot += amount;
			mainPot.Add(amount);
			mainPot.AddPlayer(this);
			if (mainPot.getMaximumAmountPutIn() < amount)
				mainPot.setMaximumAmount(amount);
			mainPot.MinimumRaise = amount;
			Message = this.Name + " pays the small blind";
			SimplifiedMessage = "SMALL BLIND " + amount;
			mainPot.AgressorIndex = index;
		}

		public void PayBigBlind(int amount, Pot mainPot, int index, Table table)
		{
			if (ChipStack <= amount)
			{
				AllIn(mainPot, table.decrementIndex(index), table);
				return;
			}
			Message = "Pay blind of " + amount.ToString("c0");
			ChipStack -= amount;
			amountInPot += amount;
			mainPot.Add(amount);
			mainPot.AddPlayer(this);
			if (mainPot.getMaximumAmountPutIn() < amount)
				mainPot.setMaximumAmount(amount);
			mainPot.MinimumRaise = amount;
			Message = this.Name + " pays the big blind";
			SimplifiedMessage = "BIG BLIND " + amount;
			mainPot.AgressorIndex = index;
		}

		public bool canRaise(Pot mainPot)
		{
			if (IsFolded())
				return false;

			return true;
		}

		public bool canFold(Pot mainPot)
		{
			if (IsFolded())
				return false;

			return true;
		}

		public bool canAllIn(Pot mainPot)
		{
			if (IsFolded())
				return false;

			return true;
		}

		public bool canCheck(Pot mainPot)
		{
			if (IsFolded() || mainPot.IsBetted == true)
				return false;

			return true;
		}

		public bool canBet(Pot mainPot)
		{
			if (IsFolded() || mainPot.IsBetted == true)
				return false;

			return true;
		}

		public bool canCall(Pot mainPot)
		{
			if (IsFolded() || mainPot.LastAction == ACTION.NULL || mainPot.LastAction == ACTION.CHECK)
				return false;

			return true;
		}

		//* 게임 포기
		public void Fold(Pot mainPot, int index, Table table)
		{
			folded = true;
			mainPot.getPlayersInPot().Remove(this);
			Message = "Fold";
			SimplifiedMessage = "FOLDED";
		}

		public bool IsFolded()
		{
			return folded;
		}

		//* Bet 하지 않음
		public void Check(Pot mainPot, int index, Table table)
		{
			Message = "Check";
			SimplifiedMessage = "CHECK";
			mainPot.LastAction = ACTION.CHECK;
		}

		//* 라운드에 남을 만큼 Bet
		public void Call(Pot mainPot, int index, Table table)
		{
			int amount = mainPot.getMaximumAmountPutIn();

			if (ChipStack <= amount)
			{
				AllIn(mainPot, index, table);
				return;
			}
			ChipStack -= amount;
			amountInPot += amount;
			mainPot.Add(amount);
			mainPot.AddPlayer(this);
			Message = "Call " + amount.ToString("c0");
			SimplifiedMessage = "CALL " + amount;
			mainPot.LastAction = ACTION.CALL;
		}

		//* Call 하고 추가로 bet
		public void Raise(int raise, Pot mainPot, int index, Table table)
		{
			int amount = mainPot.getMaximumAmountPutIn() + raise;

			if (ChipStack <= amount)
			{
				AllIn(mainPot, index, table);
				return;
			}
			ChipStack -= amount;
			amountInPot += amount;
			mainPot.Add(amount);
			if (mainPot.getMaximumAmountPutIn() < amount)
				mainPot.setMaximumAmount(amount);
			mainPot.AddPlayer(this);
			mainPot.MinimumRaise = raise;
			Message = "Call " + (amount - raise).ToString("c0") + " and raise " + raise.ToString("c0");
			SimplifiedMessage = "RAISE " + (amount - raise);
			mainPot.AgressorIndex = index;
			mainPot.IsBetted = true;
			mainPot.LastAction = ACTION.RAISE;
		}

		//* 돈을 Bet함
		public void Bet(int bet, Pot mainPot, int index, Table table)
		{
			if (ChipStack <= bet)
			{
				AllIn(mainPot, index, table);
				return;
			}
			ChipStack -= bet;
			amountInPot += bet;
			mainPot.Add(bet);
			if (mainPot.getMaximumAmountPutIn() < bet)
				mainPot.setMaximumAmount(bet);
			mainPot.AddPlayer(this);
			mainPot.MinimumRaise = bet;
			Message = "Bet " + bet.ToString("c0");
			SimplifiedMessage = "BET " + bet;
			mainPot.AgressorIndex = index;
			mainPot.IsBetted = true;
			mainPot.LastAction = ACTION.BET;
		}

		//* 가지고 있는 모든 돈을 Bet
		public void AllIn(Pot mainPot, int index, Table table)
		{
			AmountContributed = ChipStack;

			if (mainPot.MinimumAllInAmount == 0)
			{
				mainPot.AmountInPotBeforeAllIn = mainPot.Amount;
				mainPot.MinimumAllInAmount = ChipStack;
			}
			else if (chipStack < mainPot.MinimumAllInAmount)
				mainPot.MinimumAllInAmount = ChipStack;

			if (ChipStack > mainPot.MinimumRaise)
				mainPot.MinimumRaise = ChipStack;

			mainPot.AddPlayer(this);
			mainPot.Add(ChipStack);
			amountInPot += ChipStack;

			ChipStack = 0;

			if (amountInPot > mainPot.getMaximumAmountPutIn())
				mainPot.setMaximumAmount(amountInPot);

			Message = "I'm All-In";
			SimplifiedMessage = "ALL IN";
			mainPot.AgressorIndex = index;
			mainPot.IsBetted = true;
			mainPot.LastAction = ACTION.ALLIN;
		}

		//* 각각의 Player 리셋
		public void Reset()
		{
			this.amountInPot = 0;
			this.folded = false;
			InitialStack = ChipStack;
			this.myHand.Clear();
			this.Message = "";
			this.SimplifiedMessage = "";
		}

		//* 승자를 위한 돈 모으기
		public void CollectMoney(Pot mainPot)
		{
			this.ChipStack += mainPot.Amount;
			this.Message = this.Name + " wins the pot!";
		}

		//* Player가 Bust할 경우, isBusted를 true로
		public void Leave()
		{
			if (this.ChipStack != 0)
				throw new InvalidOperationException();
			this.Message = this.Name + " busted out!";
			isBusted = true;
		}
	}
}