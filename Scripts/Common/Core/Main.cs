using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Holdem
{
	class Test
	{
		static void Main(string[] args)
		{
			int k = 0;
			while (true)
			{
				k++;
				Console.WriteLine();
				Console.WriteLine(">>>>>>>>>>>>>> " + k);
				Console.WriteLine();

				int n = 3;

				PlayerList playerList = new PlayerList();

				for (int i = 0; i < n; i++)
				{
					Player player = new Player(((char)(i + 'A')).ToString(), 100000);
					playerList.Add(player);
				}

				Table table = new Table(playerList);
				Console.WriteLine("Table");
				Console.WriteLine("Dealer\t" + table.getDealerPosition() + "\t" + table[table.getDealerPosition()].Name);
				Console.WriteLine("Index\t" + table.getCurrentIndex() + "\t" + table[table.getCurrentIndex()].Name);
				Console.WriteLine("Agres\t" + table.getPot().AgressorIndex + "\t" + table[table.getPot().AgressorIndex].Name + "\n");


				table.PaySmallBlind();
				Console.WriteLine("SB");
				Console.WriteLine("Dealer\t" + table.getDealerPosition() + "\t" + table[table.getDealerPosition()].Name);
				Console.WriteLine("Index\t" + table.getCurrentIndex() + "\t" + table[table.getCurrentIndex()].Name);
				Console.WriteLine("Agres\t" + table.getPot().AgressorIndex + "\t" + table[table.getPot().AgressorIndex].Name + "\n");

				table.PayBigBlind();
				Console.WriteLine("BB");
				Console.WriteLine("Dealer\t" + table.getDealerPosition() + "\t" + table[table.getDealerPosition()].Name);
				Console.WriteLine("Index\t" + table.getCurrentIndex() + "\t" + table[table.getCurrentIndex()].Name);
				Console.WriteLine("Agres\t" + table.getPot().AgressorIndex + "\t" + table[table.getPot().AgressorIndex].Name + "\n");

				table.DealHoleCards();

				while (true)
				{
					Console.WriteLine(table[table.getCurrentIndex()].Name + " " + table.beginNextTurn());
					table.setCurrentIndex(table.incrementIndex(table.getCurrentIndex()));
					Console.Write("1. Fold | ");
					if (table[table.getCurrentIndex()].canCheck(table.getPot()))
						Console.Write("2. Check | ");
					if (table[table.getCurrentIndex()].canCall(table.getPot()))
						Console.Write("3. Call | ");
					Console.Write("4. Raise | ");
					if (table[table.getCurrentIndex()].canBet(table.getPot()))
						Console.Write("5. Bet | ");
					Console.Write("6. AllIn | 9. Exit\n");
					string sAct;
					int Act;
					sAct = Console.ReadLine();
					Act = Convert.ToInt32(sAct);

					switch (Act)
					{
						case 1:
							table[table.getCurrentIndex()].Fold(table.getPot(), table.getCurrentIndex(), table);
							Console.WriteLine(table[table.getCurrentIndex()].Name + " Fold");
							break;
						case 2:
							table[table.getCurrentIndex()].Check(table.getPot(), table.getCurrentIndex(), table);
							Console.WriteLine(table[table.getCurrentIndex()].Name + " Check");
							break;
						case 3:
							table[table.getCurrentIndex()].Call(table.getPot(), table.getCurrentIndex(), table);
							Console.WriteLine(table[table.getCurrentIndex()].Name + " Call");
							break;
						case 4:
							table[table.getCurrentIndex()].Raise(1000000, table.getPot(), table.decrementIndex(table.getCurrentIndex()), table);
							Console.WriteLine(table[table.getCurrentIndex()].Name + " Raise");
							break;
						case 5:
							table[table.getCurrentIndex()].Bet(30000, table.getPot(), table.decrementIndex(table.getCurrentIndex()), table);
							Console.WriteLine(table[table.getCurrentIndex()].Name + " Bet");
							break;
						case 6:
							table[table.getCurrentIndex()].AllIn(table.getPot(), table.decrementIndex(table.getCurrentIndex()), table);
							Console.WriteLine(table[table.getCurrentIndex()].Name + " All In");
							break;
						case 9:
							break;
						default:
							return;
					}
					if (Act == 9) 
						break;
					Console.WriteLine("Dealer\t" + table.getDealerPosition() + "\t" + table[table.getDealerPosition()].Name);
					Console.WriteLine("Index\t" + table.getCurrentIndex() + "\t" + table[table.getCurrentIndex()].Name);
					Console.WriteLine("Agres\t" + table.getPot().AgressorIndex + "\t" + table[table.getPot().AgressorIndex].Name + "\n");
					Console.WriteLine("Money\t" + table[table.getCurrentIndex()].ChipStack);
				}

				//for (int i = 0; i < n; i++)
				//{
				//	table.setCurrentIndex(table.incrementIndex(table.getCurrentIndex()));
				//	table[table.getCurrentIndex()].Bet(10000, table.getPot(), table.decrementIndex(table.getCurrentIndex()), table);
				//}

				table.DealFlop();
				table.DealTurn();
				table.DealRiver();

				for (int i = 0; i < 5; i++)
					Console.WriteLine((SIMBOL)table.getCommunityCards()[i].getSuit() + "" + table.getCommunityCards()[i].getRank());

				Console.WriteLine();

				for (int i = 0; i < n; i++)
					Console.WriteLine(table[i].Name + " : " + (SIMBOL)table[i].getHand()[0].getSuit() + table[i].getHand()[0].getRank() + "\t" + (SIMBOL)table[i].getHand()[1].getSuit() + table[i].getHand()[1].getRank());

				Console.WriteLine();

				table.Showdown();



				for (int i = 0; i < table.getMainPotWinners().getWinners().Count; i++)
				{
					Console.WriteLine(table.getMainPotWinners().getWinners()[i].getHand().getValue().Count());
					Console.WriteLine(table.getMainPotWinners().getWinners()[i].getHand().getValue()[0]);
					Console.WriteLine(table.getMainPotWinners().getWinners()[i].Name + " : " + table.getMainPotWinners().getWinners()[i].myHand);
				}
			}
		}
	}
}
