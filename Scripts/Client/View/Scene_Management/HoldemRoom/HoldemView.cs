using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Popup;
using CardGame.logic;
using CardGame.logic.Prepabs.Holdem;
using CardGame.logic.Prepabs.Animate;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

namespace CardGame.view.holdemview
{
    public class HoldemView : MonoBehaviour
    {
        public Canvas mycanvas;
        
        public GameObject MyCard;
        public List<GameObject> MyCardOBJpool = new List<GameObject>();
       
        public GameObject CommuCard;
        public List<GameObject> CommuCardOBJpool = new List<GameObject>();

        public GameObject EnemyCard;
        public GameObject Enemy;
        public List<GameObject> EnemyOBJpool = new List<GameObject>();

        public List<String> usernames;

        public GameObject ShowdownOBJ;
        public GameObject whiteCutton;
        public GameObject showDownTobj;
        
        public HoldemObjManager holdemObjManager;

        public bool isBetting = false;

        public TMP_Text ShowDownText;

        public GameObject OutButton;
        
        /// <summary>
        /// 플레이어  리스트
        /// </summary>
        public List<GameObject> EnemyList;

        private List<GameObject> ShowdownList = new List<GameObject>(); 
        public int[] isplayer2 = {3};
        public int[] isplayer3 = {3,5};
        public int[] isplayer4 = {2,4,6};
        public int[] isplayer5 = {3,4,6,7};
        public int[] isplayer6 = {2,3,4,5,6};
        public int[] isplayer7 = {1,2,3,5,6,7};
        public int[] isplayer8 = {1,2,3,4,5,6,7};

        void Awake()
        {
            Debug.Log("HOLDEM START");
            LogicManager.instance.holdemgear.FindMe();
        }

        private void Start()
        {
            PopupManager.instance.setCanvas(mycanvas);
            Debug.LogWarning(LogicManager.instance.playergear.GetLeader());

            C_InGameLoadingComplete pkt = new C_InGameLoadingComplete();
            LogicManager.instance.holdemgear.ServerCall(pkt);
            // if (LogicManager.instance.playergear.GetLeader())
            // {
            //     C_DealHoleCards dhcpkt = new C_DealHoleCards();
            //     LogicManager.instance.holdemgear.ServerCall(dhcpkt);
            // }
        }

        #region  모두의데이터

        /// <summary>
        /// 모든 유저들의 데이터를 동기화 하는 용도로 사용됨.
        /// </summary>
        /// <param name="players"></param>
        public void SetUserDatas(List<S_UserDataSync.Player> players)
        {
            foreach (var player in players)
            {
                Debug.LogWarning("Name : " + player.playerName);
                Debug.LogWarning("ID : " + player.playerId);

            }
            EnemyOBJpool.Clear();
            int[] playerpath = {};
            switch (players.Count)
            {
                case 2:
                    playerpath = isplayer2;
                    break;
                case 3:
                    playerpath = isplayer3;
                    break;
                case 4:
                    playerpath = isplayer4;
                    break;
                case 5:
                    playerpath = isplayer5;
                    break;
                case 6:
                    playerpath = isplayer6;
                    break;
                case 7:
                    playerpath = isplayer7;
                    break;
                case 8:
                    playerpath = isplayer8;
                    break;
            }

            foreach (var path in playerpath)
            {
               
                ShowdownList.Add(holdemObjManager.getspotpos(path));
                EnemyList.Add(holdemObjManager.getplayerpos(path));
            }
            foreach (var player in players)
            {
                if (player.isMine)
                {
                    SetMydata(player);
                    players.Remove(player);
                    break;
                }
            }
            
            for (int i = 0; i < EnemyList.Count; i++)
            {
                GameObject enemy = Instantiate(Enemy);
                usernames.Add(players[i].playerName);
                enemy.GetComponent<EnemyPos>().setenemyname(players[i].playerName);
                enemy.GetComponent<EnemyPos>().setenemyid(players[i].playerId);
                enemy.transform.SetParent(EnemyList[i].transform,false);
                // enemy status 에 따라서 폴드 상태를 재정의.
                EnemyOBJpool.Add(enemy);
                //todo 여기서 나머지 플레이어에대한 정보를 정리
            }
            //todo 유저데이터중 나를 찾기
            //유저데이터 2 3 4 5 6 7 8 명 마다 다른 처리
            //유저데이터 세팅 완료! 여기서부터 유저 콜 가능!
            if (LogicManager.instance.playergear.GetLeader())
            {
                C_UserDataSync pkt_SYNC = new C_UserDataSync();
                LogicManager.instance.holdemgear.ServerCall(pkt_SYNC);
            }
            C_ChipStackSync pkt = new C_ChipStackSync();
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }
        // enemy status 에 따라서 폴드 상태를 재정의.
        public void SetFoldUser(int usernum)
        {
            try
            {
                EnemyPos enp = FindEnemyWho(usernum);
                enp.foldcutton.SetActive(true);

            }
            catch
            {
                Debug.Log("자턴폴드");
            }
        }
        /// <summary>
        /// 딜러버튼 생성용
        /// </summary>
        /// <param name="who"></param>
        public void SetDelarButton(int who)
        {
            if(holdemObjManager.mypos.myid == who)
                holdemObjManager.mypos.setdelarButton(true);
            else
                holdemObjManager.mypos.setdelarButton(false);
            
            for (int i = 0; i < EnemyList.Count; i++)
            {
                if (EnemyList[i].GetComponentInChildren<EnemyPos>().enemyid == who)
                    EnemyList[i].GetComponentInChildren<EnemyPos>().setdelarButton(true);
                else
                    EnemyList[i].GetComponentInChildren<EnemyPos>().setdelarButton(false);
            }
            //todo 딜러버튼 확인
            if (LogicManager.instance.playergear.GetLeader())
            {
                C_DealerButton pktt = new C_DealerButton();
                LogicManager.instance.holdemgear.ServerCall(pktt);
            }
        }
        /// <summary>
        /// 상대의 유저들의 더미 핸드를 뿌려주는 메소드
        /// </summary>
        public void SetDummyCards()
        {
            foreach (var enemy in EnemyList)
            {
                GameObject enemypos_one = enemy.GetComponentInChildren<EnemyPos>()._enemyHand.Hand_one_pos;
                GameObject enemypos_two = enemy.GetComponentInChildren<EnemyPos>()._enemyHand.Hand_two_pos;

                GameObject card_one = Instantiate(EnemyCard);
                card_one = LogicManager.instance.holdemgear.SetDummyCard(card_one);
                card_one.transform.SetParent(enemypos_one.transform,false);
                card_one.GetComponent<AnimatedCard>().despos =enemypos_one.transform.position;
                card_one.GetComponent<AnimatedCard>().startpos =holdemObjManager.Center;
                card_one.GetComponent<AnimatedCard>().Resign();
                GameObject card_two = Instantiate(EnemyCard);
                card_two = LogicManager.instance.holdemgear.SetDummyCard(card_two);
                card_two.transform.SetParent(enemypos_two.transform,false);
                card_two.GetComponent<AnimatedCard>().despos = enemypos_two.transform.position;
                card_two.GetComponent<AnimatedCard>().startpos =holdemObjManager.Center;
                card_two.GetComponent<AnimatedCard>().Resign();
            }
        }
        
        /// <summary>
        /// 모든 유저들의 카드 공개
        /// </summary>
        public void SetUserCards(S_ShowDown.Hand1 hand_one ,S_ShowDown.Hand2  hand_two,int userid)
        {
            
            if(FindWho(userid))
                return;
            Debug.Log("opencard!");
            EnemyPos enemy = FindEnemyWho(userid);
            GameObject enemypos_one = enemy._enemyHand.Hand_one_pos;
            GameObject enemypos_two = enemy._enemyHand.Hand_two_pos;
            
            GameObject card_one = Instantiate(EnemyCard);
            card_one = LogicManager.instance.holdemgear.SetCardObject(card_one,hand_one);
            card_one.transform.SetParent(enemypos_one.transform,false);
            card_one.GetComponent<AnimatedCard>().despos =enemypos_one.transform.position;
            card_one.GetComponent<AnimatedCard>().startpos = enemypos_one.transform;
            card_one.GetComponent<AnimatedCard>().Resign();

            GameObject card_two = Instantiate(EnemyCard);
            card_two = LogicManager.instance.holdemgear.SetCardObject(card_two,hand_two);
            card_two.transform.SetParent(enemypos_two.transform,false);
            card_two.GetComponent<AnimatedCard>().despos = enemypos_two.transform.position;
            card_two.GetComponent<AnimatedCard>().startpos = enemypos_two.transform;
            card_two.GetComponent<AnimatedCard>().Resign();

        }

        public int ShowDownCount = 0;
        public void Showdown( List<S_ShowDown.Hand1> list1, List<S_ShowDown.Hand2>list2)
        { 
            Debug.LogWarning("SHOWDOWN!");
            StartCoroutine(ShowDown(list1, list2));
        }
        IEnumerator ShowDown( List<S_ShowDown.Hand1> list1, List<S_ShowDown.Hand2>list2)
        {
            bool findme = false;
            showdowncall();
            for (int i = 0; i < list1.Count; i++)
            {            
                yield return new WaitForSeconds(3f);
                if (FindWho(i))
                {
                    if(i+1==list1.Count)
                        break;
                    findme = true;
                    continue;
                }

                //Debug.Log(i);
                if (findme)
                {
                    ShowdownList[i-1].SetActive(true);
                    SetUserCards(list1[i], list2[i], i);
                }
                else
                {
                    ShowdownList[i].SetActive(true);
                    SetUserCards(list1[i], list2[i], i);
                }

            }
            
            //OutButton.SetActive(true);
            C_ShowDown pkt = new C_ShowDown();
            yield return new WaitForSeconds(3f);
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }

        public void showdowncall()
        {
            showDownTobj.SetActive(true);
            whiteCutton.SetActive(true);
            ShowdownOBJ.SetActive(true);
        }

        public void ShowDownMessage(string winners,int Mainpot,int sidepot,bool usesidepot)
        {
            
        }
        public void ShowDownMessage(string winners)
        {
            ShowDownText.text = "Main Pot\n" + winners;OutButton.SetActive(true);
            C_Winner pkt = new C_Winner();
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }

        
        /// <summary>
        /// 소지 금액들의 업데이트
        /// </summary>
        /// <param name="moneydatas"></param>
        public void updateMoneys(List<S_ChipStackSync.Player> moneydatas)
        {
            foreach (var player in moneydatas)
            {
                if (player.isMine)
                {
                    SetMymoney(player);
                    moneydatas.Remove(player);
                    break;
                }
            }
            foreach (var player in moneydatas)
            {
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    if (EnemyList[i].GetComponentInChildren<EnemyPos>().enemyid == player.playerId)
                    {
                        //Debug.LogWarning(player.chip);
                        EnemyList[i].GetComponentInChildren<EnemyPos>().setenemychip(player.chip);
                    }
                }
            }

            
        }
        /// <summary>
        /// id 따라서 나인지 확인
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public bool FindWho(int who)
        {
            if (holdemObjManager.mypos.myid == who)
                return true;
            return false;
        }
        /// <summary>
        /// enemy중에서 누구인지 확인
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public EnemyPos FindEnemyWho(int who)
        {
            EnemyPos myenemy = null;
            for (int i = 0; i < EnemyList.Count; i++)
            {
                if (EnemyList[i].GetComponentInChildren<EnemyPos>().enemyid == who)
                    myenemy = EnemyList[i].GetComponentInChildren<EnemyPos>();
            }
            return myenemy;
        }
        public void CleanEnemyName()
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].GetComponentInChildren<EnemyPos>()._enemyHand.enemyname.outlineWidth = 0f;
            }
        }
        public void SetPandon(int money)
        {
            string text;
            text = "$ " + GetThousandCommaText(money);
            holdemObjManager.Pandonmoney.GetComponent<BetMoney>().mymoney.text = text;
        }
        
        /// <summary>
        /// 커뮤니티 핸드를 세팅하기위한 메소드
        /// </summary>
        /// <param name="hands"></param>
        public void SetcommuCard(S_CommunityCards.Card card,int num)
        {
            Debug.Log("커뮤니티 핸드 오픈");
            MyCardOBJpool.Clear();
            GameObject commucard = Instantiate(CommuCard);
            commucard = LogicManager.instance.holdemgear.SetCardObject(commucard,card);
            commucard.transform.SetParent(holdemObjManager.deckpos.transform,false);
            commucard.GetComponent<AnimatedCard>().despos = holdemObjManager.getcardpos(num).transform.position;
            commucard.GetComponent<AnimatedCard>().startpos =holdemObjManager.deckpos.transform;
            CommuCardOBJpool.Add(commucard);
        }
        #endregion
        
        #region  나의데이터
        
        /// <summary>
        /// 나의 데이터를 업데이트 하는 용도
        /// </summary>
        /// <param name="player"></param>
        public void SetMydata(S_UserDataSync.Player player)
        {
            holdemObjManager.mypos.setmyname(player.playerName);
            usernames.Add(player.playerName);
            holdemObjManager.mypos.myid = player.playerId;
        }
        public void SetMymoney(S_ChipStackSync.Player player)
        {
            holdemObjManager.mypos.setmychip(player.chip);
        }
        /// <summary>
        /// 나의 핸드를 세팅하기위한 메소드
        /// </summary>
        /// <param name="hands"></param>
        public void SetCard(S_DealHoleCards.Hand hand_one ,S_DealHoleCards.Hand  hand_two)
        {
            MyCardOBJpool.Clear();
            GameObject card_one = Instantiate(MyCard);
            card_one = LogicManager.instance.holdemgear.SetCardObject(card_one,hand_one);
            card_one.transform.SetParent(holdemObjManager.mypos.myhand.Hand_one_pos.transform,false);
            card_one.GetComponent<AnimatedCard>().despos = holdemObjManager.mypos.myhand.Hand_one_pos.transform.position;
            card_one.GetComponent<AnimatedCard>().startpos =holdemObjManager.Center;

            GameObject card_two = Instantiate(MyCard);
            card_two = LogicManager.instance.holdemgear.SetCardObject(card_two,hand_two);
            card_two.transform.SetParent(holdemObjManager.mypos.myhand.Hand_two_pos.transform,false);
            card_two.GetComponent<AnimatedCard>().despos = holdemObjManager.mypos.myhand.Hand_two_pos.transform.position;
            card_two.GetComponent<AnimatedCard>().startpos =holdemObjManager.Center;

            MyCardOBJpool.Add(card_one);
            MyCardOBJpool.Add(card_two);
        }
        
        /// <summary>
        /// 베팅 렌더 세팅
        /// </summary>
        public void SetBetting(S_Bet pkt)
        {
            Debug.Log(pkt.playerId);
            if (FindWho(pkt.playerId))
            {
                Debug.Log("myBet!");
                isBetting = true;
                holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_uppos();
                CleanEnemyName();

                BetPanel bpl = holdemObjManager.Betmoney.GetComponent<BetPanel>();
                
                bpl.setactive(false);
                bpl.allin = false;
                bpl.bet =false;
                bpl.raise = false;
                bpl.call =false;
                bpl.fold =false;
                bpl.check = false;
                
                bpl.allin = pkt.canAllIn;
                bpl.bet = pkt.canBet;
                bpl.raise = pkt.canRaise;
                bpl.call = pkt.canCall;
                bpl.fold = pkt.canFold;
                bpl.check = pkt.canCheck;
                    
                if(pkt.canBet)
                    bpl.betbtn.SetActive(pkt.canBet);
                if(pkt.canCall)
                    bpl.callbtn.SetActive(pkt.canCall);
                if(pkt.canCheck)
                    bpl.checkbtn.SetActive(pkt.canCheck);
                if(pkt.canFold)
                    bpl.foldbtn.SetActive(pkt.canFold);
                if(pkt.canRaise)
                    bpl.raisebtn.SetActive(pkt.canRaise);
                if(pkt.canAllIn)
                    bpl.allinbutton.SetActive(pkt.canAllIn);
                holdemObjManager.betPanel.GetComponent<HowBetPanel>().minmoney = pkt.minAmount;
                holdemObjManager.betPanel.GetComponent<HowBetPanel>().maxmoney = holdemObjManager.mypos.mychip;
                holdemObjManager.betPanel.GetComponent<HowBetPanel>().Inituis();
            }
            else
            {
                //여기야? 적이 배팅중일때
                Debug.Log("EnemyBet!");
                isBetting = false;
                holdemObjManager.Betmoney.GetComponent<BetPanel>().setactive(isBetting);
                EnemyPos enemywho = FindEnemyWho(pkt.playerId);
                CleanEnemyName();
                enemywho._enemyHand.enemyname.outlineWidth = 0.2f;
                enemywho._enemyHand.enemyname.outlineColor = new Color32(255, 128, 0, 255);
                
            }
        }
      
        #endregion
        
        /// <summary>
        /// pair
        /// </summary>
        public void Check()
        {
            Debug.LogWarning("CHECK");
            holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_downpos();
            C_Bet pkt = new C_Bet();
            pkt.playerId = holdemObjManager.mypos.myid;
            pkt.info = "check";
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }
        public void Call()
        {
            Debug.LogWarning("CALL");
            holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_downpos();
            C_Bet pkt = new C_Bet();
            pkt.playerId = holdemObjManager.mypos.myid;
            pkt.info = "call";
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }

        #region Betting Raise
        public void openbetui()
        {
            holdemObjManager.betPanel.GetComponent<HowBetPanel>().Inituis();
            holdemObjManager.betPanel.GetComponent<HowBetPanel>().setpanelname("Bet");
            holdemObjManager.betPanel.GetComponent<HowBetPanel>().setIsbet();
            holdemObjManager.betPanel.GetComponent<AnimatedBetPanel>().change_uppos();
        }
        public void openraiseui()
        {
            holdemObjManager.betPanel.GetComponent<HowBetPanel>().Inituis();
            holdemObjManager.betPanel.GetComponent<HowBetPanel>().setpanelname("Raise");
            holdemObjManager.betPanel.GetComponent<HowBetPanel>().setIsraise();
            holdemObjManager.betPanel.GetComponent<AnimatedBetPanel>().change_uppos();
        }
        /// <summary>
        /// pair
        /// </summary>
        public void Bet(int a)
        {
            Debug.LogWarning("BET");
            holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_downpos();
            C_Bet pkt = new C_Bet();
            pkt.playerId = holdemObjManager.mypos.myid;
            pkt.info = "bet";
            pkt.chip = a;
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }
        public void Raise(int a)
        {
            Debug.LogWarning("RAISE");
            holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_downpos();
            C_Bet pkt = new C_Bet();
            pkt.playerId = holdemObjManager.mypos.myid;
            pkt.info = "raise";
            pkt.chip = a;
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }
        #endregion
        
        public void Fold()
        {
            Debug.LogWarning("FOLD");
            holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_downpos();
            C_Bet pkt = new C_Bet();
            pkt.playerId = holdemObjManager.mypos.myid;
            pkt.info = "fold";
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }
        public void All_in()
        {
            Debug.LogWarning("ALL_IN");
            holdemObjManager.Betmoney.GetComponent<AnimatedPanel>().change_downpos();
            C_Bet pkt = new C_Bet();
            pkt.playerId = holdemObjManager.mypos.myid;
            pkt.info = "allin";
            LogicManager.instance.holdemgear.ServerCall(pkt);
        }
        
        public string GetThousandCommaText(float data)
        {
            return string.Format("{0:#,###}", data);
        }
        public string GetThousandCommaText(int data)
        {
            return string.Format("{0:#,###}", data);
        }


        public void OutGame()
        {
            C_LeaveInGame pkt = new C_LeaveInGame();
            LogicManager.instance.holdemgear.ServerCall(pkt);
            
        }
        
        /// <summary>
        /// 카드를 생성하는데 걸리는 시간 기능 생성용 (일단보류)
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        IEnumerator cardInitTimer(GameObject card)
        {
            yield return new WaitForSeconds(0.5f);
            card.SetActive(true);
        }
    }

    
}
