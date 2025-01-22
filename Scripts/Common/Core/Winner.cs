using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	public class Winner
	{
		private PlayerList winners = new PlayerList();
		private List<int> winnerIndex = new List<int>();
		//bool isSidePotWinner;
		//int sidePotIndex;

		public Winner()
		{
			winners = new PlayerList();
			//isSidePotWinner = false;
			//sidePotIndex = -1;
		}

		public Winner(int sidePotIndex)
		{
			winners = new PlayerList();
			//isSidePotWinner = true;
			//this.sidePotIndex = sidePotIndex;
		}

		public List<int> getWinnerIndex()
		{
			return winnerIndex;
		}

		public PlayerList getWinners()
		{
			return winners;
		}

		//public bool getIsSidePotWinner()
		//{
		//	return isSidePotWinner;
		//}

		//public int getSidePotIndex()
		//{
		//	return sidePotIndex;
		//}
	}
}
