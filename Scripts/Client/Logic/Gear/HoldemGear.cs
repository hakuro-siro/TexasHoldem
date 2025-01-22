using System;
using UniRx;
using System.Collections.Generic;
using CardGame.logic.Prepabs.Holdem;
using UnityEngine;
using CardGame.view.holdemview;
using CardGame.view;
using UnityEngine.UI;
using System.Collections;

namespace CardGame.logic
{
    public class HoldemGear
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
        /// <summary>
        /// Gear들은 모두 LogicManager를 가지고 있습니다.
        /// </summary>
        [SerializeField] 
        private LogicManager logicmanager;

        private HoldemView _holdem;

        public List<S_DealHoleCards.Hand> HandList = new List<S_DealHoleCards.Hand>();

        public List<S_UserDataSync.Player> PlayerList = new List<S_UserDataSync.Player>();
        
        public List<S_ChipStackSync.Player> MoneyList = new List<S_ChipStackSync.Player>();

        public string basicPath = "CardImage/CardImage/";
        /// <summary>
        /// LogicManager의 Subject를 Subscribe
        /// </summary>
        
        // Subscribing............
        // nothing!
        public void SubscribeManager()
        {
            logicmanager = GameObject.Find("LogicManager").GetComponent<LogicManager>();
        }
                
        // Holdem.cs 클래스를 추적
        public void FindMe()
        {
            _holdem = GameObject.Find("HoldemManager").GetComponent<HoldemView>();
        }

        //TODO LIST
        // 가지고 있는 손패의 갱신
        // 게임의 결과의 공개
        // 딜러가 보여주는 카드의 갱신
        // 각 플레이어의 베팅양 갱신
        // 자신의 소지 재화 갯수 갱신
        // ^^ 이 모든것을 한번에 갱신하게 되는 것인가? 

        /// <summary>
        /// 가지고 있는 나의 손패의 갱신
        /// </summary>
        /// <param name="pkt"></param>
        public void GetHand(S_DealHoleCards pkt)
        {
            HandList.Clear();
            foreach (var hand in pkt.hands)
            {
               HandList.Add(hand);
            }
            S_DealHoleCards.Hand hand_one = HandList[0];
            S_DealHoleCards.Hand hand_two = HandList[1];
            _holdem.SetCard(hand_one,hand_two);
        }

        /// <summary>
        /// 카드 내부 프리팹 정보를 넣어주기 위한 메소드
        /// </summary>
        /// <param name="card"></param>
        /// <param name="cardinfo"></param>
        /// <returns></returns>
        public GameObject SetCardObject(GameObject card,S_DealHoleCards.Hand cardinfo)
        {
            string suit = "";
            int rank = cardinfo.rank;
            string basefilename = "CardImage_"+rank;
            string FileLink;
            //base link
            //Assets/Resources/CardImage/CardImage/CardImage_0.asset
            switch (cardinfo.suit)
            {
                case 1:
                    suit = "Diamond/";
                    break;
                case 2:
                    suit = "Clover/";
                    break;
                case 3:
                    suit = "Heart/";
                    break;
                case 4:
                    suit = "Spade/";
                    break;
            }

            FileLink = basicPath + suit + basefilename;
            //Debug.LogWarning(FileLink);
            card.GetComponent<MyCard>().setrank(cardinfo.rank);
            card.GetComponent<MyCard>().setsuit(cardinfo.suit);
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>(FileLink);
            return card;
        }
        /// <summary>
        /// 카드 내부 프리팹 정보를 넣어주기 위한 메소드
        /// </summary>
        /// <param name="card"></param>
        /// <param name="cardinfo"></param>
        /// <returns></returns>
        public GameObject SetCardObject(GameObject card,S_ShowDown.Hand1 cardinfo)
        {
            string suit = "";
            int rank = cardinfo.rank1;
            string basefilename = "CardImage_"+rank;
            string FileLink;
            //base link
            //Assets/Resources/CardImage/CardImage/CardImage_0.asset
            switch (cardinfo.suit1)
            {
                case 1:
                    suit = "Diamond/";
                    break;
                case 2:
                    suit = "Clover/";
                    break;
                case 3:
                    suit = "Heart/";
                    break;
                case 4:
                    suit = "Spade/";
                    break;
            }

            FileLink = basicPath + suit + basefilename;
            Debug.LogWarning(FileLink);
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>(FileLink);
            return card;
        }
        public GameObject SetCardObject(GameObject card,S_ShowDown.Hand2 cardinfo)
        {
            string suit = "";
            int rank = cardinfo.rank2;
            string basefilename = "CardImage_"+rank;
            string FileLink;
            //base link
            //Assets/Resources/CardImage/CardImage/CardImage_0.asset
            switch (cardinfo.suit2)
            {
                case 1:
                    suit = "Diamond/";
                    break;
                case 2:
                    suit = "Clover/";
                    break;
                case 3:
                    suit = "Heart/";
                    break;
                case 4:
                    suit = "Spade/";
                    break;
            }

            FileLink = basicPath + suit + basefilename;
            Debug.LogWarning(FileLink);
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>(FileLink);
            return card;
        }
        public GameObject SetCardObject(GameObject card,S_CommunityCards.Card cardinfo)
        {
            string suit = "";
            int rank = cardinfo.rank;
            string basefilename = "CardImage_"+rank;
            string FileLink;
            //base link
            //Assets/Resources/CardImage/CardImage/CardImage_0.asset
            switch (cardinfo.suit)
            {
                case 1:
                    suit = "Diamond/";
                    break;
                case 2:
                    suit = "Clover/";
                    break;
                case 3:
                    suit = "Heart/";
                    break;
                case 4:
                    suit = "Spade/";
                    break;
            }

            FileLink = basicPath + suit + basefilename;
            //Debug.LogWarning(FileLink);
            card.GetComponent<CommuCard>().setrank(cardinfo.rank);
            card.GetComponent<CommuCard>().setsuit(cardinfo.suit);
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>(FileLink);
            return card;
        }
        public GameObject SetDummyCard(GameObject card)
        {
            string FileLink = "InGame/CardBack";
            //base link
            //Assets/Resources/CardImage/CardImage/CardImage_0.asset
            //Debug.LogWarning(FileLink);
            card.GetComponent<Image>().sprite = Resources.Load<Sprite>(FileLink);
            return card;
        }
        /// <summary>
        /// 서버에 대한 호출. 내 카드에 대한 정보
        /// </summary>
        /// <param name="pkt"></param>
        public void ServerCall(C_DealHoleCards pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }
        /// <summary>
        /// 서버에 대한 호출. 모든 유저에 대한 정보
        /// </summary>
        /// <param name="pkt"></param>
        public void ServerCall(C_UserDataSync pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }
        /// <summary>
        /// 서버에 대한 호출. 모든 유저의 돈 정보
        /// </summary>
        /// <param name="pkt"></param>
        public void ServerCall(C_ChipStackSync pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }
        //C_LeaveGame
        //C_LeaveInGame
        public void ServerCall(C_LeaveInGame pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }

        public void ServerCall(C_LeaveGame pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }
        //C_Winner
        public void ServerCall(C_Winner pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }

        //C_ShowDown
        public void ServerCall(C_ShowDown pkt)
        { HoldemClientMain.instance.NetworkManager.Send(pkt.Write()); }

        public void ServerCall(C_DealerButton pkt)
        {
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
        }
        //C_InGameLoadingComplete
        public void ServerCall(C_InGameLoadingComplete pkt)
        {
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
        }
        //C_Bet
        public void ServerCall(C_Bet pkt)
        {
            HoldemClientMain.instance.NetworkManager.Send(pkt.Write());
        }
        // 플레이어 정보 동기화에 대한 메소드 SetPlayers
        public void SetPlayers(S_UserDataSync playes)
        {
            PlayerList.Clear();
            foreach (var player in playes.players)
            {
                PlayerList.Add(player);
            }
            //TODO 플레이어 칩 동기화 부분
            _holdem.SetUserDatas(PlayerList);
        }
        // 플레이어 돈 정보 동기화에 대한 메소드 SetMoneys
        public void SetMoneys(S_ChipStackSync moneys)
        {
            MoneyList.Clear();
            foreach (var player in moneys.players)
            {
                MoneyList.Add(player);
            }

            _holdem.SetPandon(moneys.pot);
            _holdem.updateMoneys(MoneyList);
        }

        public void SetDelarButton(int who)
        {
            _holdem.SetDelarButton(who);
        }

        public void SetDummyCards()
        {
            _holdem.SetDummyCards();
        }

        public void SetBetting(S_Bet pkt)
        {
            _holdem.SetBetting(pkt);
        }

        private int cntComucard = 0;
        private List<S_CommunityCards.Card> list;
        public void SetCommuCard(S_CommunityCards pkt)
        {
            list = pkt.cards;
            
            if (list.Count >= 2)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    cntComucard++;
                    _holdem.SetcommuCard(list[i], i);
                }
            }
            else
            {
                cntComucard++;
                _holdem.SetcommuCard(list[0],cntComucard-1);
            }
        }

        public void SetFoldUser(S_Fold pkt)
        {
            _holdem.SetFoldUser(pkt.playerId);
        }
        public void ShowDownGame(S_ShowDown pkt)
        {
            List<S_ShowDown.Hand1> list1 = pkt.hand1s;
            List<S_ShowDown.Hand2> list2 = pkt.hand2s;
            _holdem.Showdown(list1,list2);
        }
        //set winner
        public void SetWinner(S_Winner pkt)
        {
            //sidepot nothing 
            if (pkt.sidePot == "")
            {
                _holdem.ShowDownMessage(pkt.mainPot);
            }
            else
            {
                //_holdem.ShowDownMessage(, true);
            }
            
            
        }

        public void Getout()
        {
            list.Clear();
            cntComucard = 0;
            LogicManager.instance.playergear.SetLeader(false);
            Scenemanager.instance.ChangeScene(2);
        }
    }
}