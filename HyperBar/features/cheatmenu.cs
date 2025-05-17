using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Steamworks;
using HyperBar.core;
using HyperBar.utilities;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace HyperBar.features
{
    [linked]
    public class cheatmenu : MonoBehaviour
    {
        public void Start()
        {
        }

        public void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                this.menuOpened = !this.menuOpened;
            }
        }

        private void OnGUI()
        {
            //logger.print("OnGUI", 7);
            Manager TempManager = null;
            DiceGamePlayManager TempDiceGamePlayManager = null;
            BlorfGamePlayManager TempBlorfGamePlayManager = null;

            // Check if we are in cards:
            var aBlorfGamePlayManager = FindObjectOfType<BlorfGamePlayManager>();
            if (aBlorfGamePlayManager != null)
            {
                TempBlorfGamePlayManager = aBlorfGamePlayManager;
                //logger.print("BlorfGamePlayManager instance found", 7);

                FieldInfo fieldInfo = typeof(BlorfGamePlayManager).GetField("manager", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    Manager manager = (Manager)fieldInfo.GetValue(aBlorfGamePlayManager);
                    if (manager != null)
                    {
                        //logger.print("BlorfGamePlayManager's manager found", 7);

                        if (manager.GameStarted)
                        {
                            //logger.print("Card Game Started", 7);
                            GUI.Label(new Rect(ScreenX / 2, 10f, 200f, 30f), "Card Game Started!");
                            TempManager = manager;
                            if (this.menuOpened)
                            {
                                //logger.print("Opening card game menu", 7);
                                this.menuRect = GUI.Window(68, this.menuRect, new GUI.WindowFunction(this.DevilsDeckMenu), "Hacker's Bar By HyperHax (Devils Deck)");
                            }
                        }
                        else
                        {
                            //logger.print("Card Game not started yet", 7);
                        }
                    }
                    else
                    {
                        //logger.print("BlorfGamePlayManager's manager is null", 7);
                    }
                }
                else
                {
                    //logger.print("FieldInfo for 'manager' not found in BlorfGamePlayManager", 7);
                }
            }
            else
            {
                //logger.print("BlorfGamePlayManager instance not found", 7);
            }

            // Check if Dice Game
            var aDiceGamePlayManager = FindObjectOfType<DiceGamePlayManager>();
            if (aDiceGamePlayManager != null)
            {
                TempDiceGamePlayManager = aDiceGamePlayManager;
                //logger.print("DiceGamePlayManager instance found", 7);

                FieldInfo fieldInfo = typeof(DiceGamePlayManager).GetField("manager", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    Manager manager = (Manager)fieldInfo.GetValue(aDiceGamePlayManager);
                    if (manager != null)
                    {
                        //logger.print("DiceGamePlayManager's manager found", 7);

                        if (manager.GameStarted)
                        {
                            //logger.print("Dice Game Started", 7);
                            GUI.Label(new Rect(ScreenX / 2, 10f, 200f, 30f), "Dice Game Started!");
                            TempManager = manager;
                            if (this.menuOpened)
                            {
                                //logger.print("Opening dice game menu", 7);
                                this.menuRect = GUI.Window(69, this.menuRect, new GUI.WindowFunction(this.DiceGameMenu), "Hacker's Bar By HyperHax (Dice)");
                            }
                        }
                        else
                        {
                            //logger.print("Dice Game not started yet", 7);
                        }
                    }
                    else
                    {
                        //logger.print("DiceGamePlayManager's manager is null", 7);
                    }
                }
                else
                {
                    //logger.print("FieldInfo for 'manager' not found in DiceGamePlayManager", 7);
                }
            }
            else
            {
                //logger.print("DiceGamePlayManager instance not found", 7);
            }

            CurrentManager = TempManager;
            CurrentBlorfGamePlayManager = TempBlorfGamePlayManager;
            CurrentDiceGamePlayManager = TempDiceGamePlayManager;

            if (CurrentDiceGamePlayManager == null && CurrentBlorfGamePlayManager == null)
            {
                GUI.Label(new Rect(ScreenX / 2, 10f, 200f, 30f), "Waiting To Start!");

            }
        }

        #region "Liars Deck Menu"
        private void DevilsDeckMenu(int wId)
        {
            GUIStyle guistyle = new GUIStyle(GUI.skin.label);
            guistyle.richText = true;

            // Adjust button position and size for setting chamber round
            if (GUI.Button(new Rect(10f, 20f, 280f, 40f), "Set Chamber Round"))
            {
                foreach (var Player in CurrentManager.Players)
                {
                    if (Player.PlayerName.Equals(SteamFriends.GetPersonaName()))
                    {
                        Player.GetComponent<BlorfGamePlay>().Networkrevolverbulllet = bulletNumber;
                    }
                }
            }

            // Slider to adjust bullet number
            bulletNumber = (int)GUI.HorizontalSlider(new Rect(10f, 70f, 280f, 20f), bulletNumber, 0, 15);
            GUI.Label(new Rect(220f, 65f, 50f, 30f), bulletNumber.ToString(), guistyle);

            float yOffset = 100f; // Starting offset for players' cards display
            float cardLabelHeight = 25f; // Height of each label

            // Display each player's information and cards
            foreach (var player in CurrentManager.Players)
            {
                // Get player's cards
                List<int> playerCards = player.GetComponent<BlorfGamePlay>().CardTypes;
                List<string> translatedCards = playerCards.Select(card => ConvertIntToCard(card)).ToList();
                List<string> coloredCards = formatCardColor(translatedCards);
                string cardsString = string.Join(" | ", coloredCards);

                // Display player's name and bullet information
                string playerBulletInfo = string.Format("{0} - {1}",
                    player.NetworkPlayerName,
                    (player.GetComponent<BlorfGamePlay>().Networkrevolverbulllet - player.GetComponent<BlorfGamePlay>().Networkcurrentrevoler == 0) ? "Dead on next!" : string.Format("{0} / {1}", player.GetComponent<BlorfGamePlay>().Networkcurrentrevoler, player.GetComponent<BlorfGamePlay>().Networkrevolverbulllet + 1));
                GUI.Label(new Rect(10f, yOffset, 400f, cardLabelHeight), playerBulletInfo, guistyle);

                // Update yOffset for cards display
                yOffset += cardLabelHeight;

                // Display player's cards
                GUI.Label(new Rect(10f, yOffset, 400f, cardLabelHeight), cardsString, guistyle);

                // Update the yOffset for the next player's info
                yOffset += cardLabelHeight + 5f;
            }

            // Display cards on the table
            if (CurrentBlorfGamePlayManager.LastRound != null && CurrentBlorfGamePlayManager.LastRound.Count > 0)
            {
                List<string> translatedCardsOnTable = CurrentBlorfGamePlayManager.LastRound.Select(card => ConvertIntToCard(card)).ToList();
                string cardsOnTableString = string.Join(" | ", formatCardColor(translatedCardsOnTable));

                // Make the cards on the table bold and larger
                GUIStyle tableCardStyle = new GUIStyle(GUI.skin.label);
                tableCardStyle.richText = true;
                tableCardStyle.fontSize = 16;
                tableCardStyle.fontStyle = FontStyle.Bold;

                GUI.Label(new Rect(10f, yOffset, 400f, cardLabelHeight), "<b>Cards on the table:</b> " + cardsOnTableString, tableCardStyle);

                // Update yOffset for the buttons
                yOffset += cardLabelHeight + 10f;

            }
            // Display buttons to set the local player's hand
            List<string> cardNames = new List<string> { "Devil", "King", "Queen", "Ace", "Jack" };
            for (int i = 0; i < cardNames.Count; i++)
            {
                if (GUI.Button(new Rect(10f, yOffset, 280f, 30f), "Set Card " + cardNames[i]))
                {
                    var localPlayer = CurrentManager.Players.FirstOrDefault(player => player.PlayerName.Equals(SteamFriends.GetPersonaName()));
                    if (localPlayer != null)
                    {
                        int cardType = (i == 0) ? -1 : i;
                        EditCards(localPlayer.GetComponent<PlayerStats>(), cardType);
                        for (int j = 0; j < localPlayer.GetComponent<BlorfGamePlay>().CardTypes.Count; j++)
                        {
                            localPlayer.GetComponent<BlorfGamePlay>().CardTypes[j] = cardType;
                        }

                        var aBlorfGamePlay = FindObjectOfType<BlorfGamePlay>();
                        FieldInfo fieldInfo = typeof(BlorfGamePlay).GetField("manager", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fieldInfo != null)
                        {
                            Manager manager = (Manager)fieldInfo.GetValue(aBlorfGamePlay);
                            if (manager != null)
                            {
                                for (int c = 0; c < 5; c++)
                                {
                                    aBlorfGamePlay.Cards[c].GetComponent<Card>().Devil = false;
                                    aBlorfGamePlay.Cards[c].GetComponent<Card>().gameObject.layer = 0;
                                    aBlorfGamePlay.Cards[c].GetComponent<Card>().cardtype = aBlorfGamePlay.CardTypes[c];
                                    if (aBlorfGamePlay.Cards[c].GetComponent<Card>().cardtype == -1)
                                    {
                                        aBlorfGamePlay.Cards[c].GetComponent<Card>().cardtype = manager.BlorfGame.RoundCard;
                                        aBlorfGamePlay.Cards[c].GetComponent<Card>().Devil = true;
                                    }
                                    aBlorfGamePlay.Cards[c].GetComponent<Card>().Selected = false;
                                    //this way we dont "spawn" more cards
                                    if (aBlorfGamePlay.Cards[c].gameObject.activeSelf)
                                    {
                                        aBlorfGamePlay.Cards[c].GetComponent<Card>().gameObject.SetActive(true);
                                    }
                                    aBlorfGamePlay.Cards[c].GetComponent<Card>().SetCard();
                                }
                            }
                        }

                        //This one restores your entire hand
                        //// Use reflection to call UserCode_SetCardsRpc method
                        //MethodInfo setCardsRpcMethod = typeof(BlorfGamePlay).GetMethod("UserCode_SetCardsRpc", BindingFlags.NonPublic | BindingFlags.Instance);
                        //if (setCardsRpcMethod != null)
                        //{
                        //    setCardsRpcMethod.Invoke(localPlayer.GetComponent<BlorfGamePlay>(), null);
                        //}
                    }
                }
                yOffset += 35f; // Update yOffset for next button
            }


            // Make the window draggable
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 10000f));
        }

        private string ConvertIntToCard(int cardType)
        {
            string result;
            switch (cardType)
            {
                case -1:
                    result = "Devil";
                    break;
                case 1:
                    result = "K";
                    break;
                case 2:
                    result = "Q";
                    break;
                case 3:
                    result = "A";
                    break;
                case 4:
                    result = "J";
                    break;
                default:
                    result = cardType.ToString();
                    break;
            }
            return result;
        }

        private List<string> formatCardColor(List<string> cards)
        {
            List<string> list = new List<string>();
            foreach (string text in cards)
            {
                if (text.Equals("Q"))
                {
                    list.Add("<color=purple>Q</color>");
                }
                else if (text.Equals("A"))
                {
                    list.Add("<color=orange>A</color>");
                }
                else if (text.Equals("K"))
                {
                    list.Add("<color=red>K</color>");
                }
                else if (text.Equals("J"))
                {
                    list.Add("<color=yellow>J</color>");
                }
                else if (text.Equals("Devil"))
                {
                    list.Add("<color=black>Devil</color>");
                }
                else
                {
                    list.Add(text);
                }
            }
            return list;
        }

        private void EditCards(PlayerStats mainPlayer, int cardType)
        {
            foreach (GameObject gameObject in mainPlayer.GetComponent<BlorfGamePlay>().Cards)
            {
                foreach (Component component in gameObject.GetComponents(typeof(Component)))
                {
                    if (component is Card)
                    {
                        ((Card)component).cardtype = cardType;
                    }
                }
            }
        }

        #endregion

        #region "Dice"

        private void DiceGameMenu(int wId)
        {
            if (CurrentManager != null && CurrentDiceGamePlayManager != null)
            {
                GUIStyle guistyle = new GUIStyle(GUI.skin.button);
                guistyle.richText = true;

                // Display title label for the dice overview
                GUI.Label(new Rect(10f, 20f, menuX - 20f, 30f), "Each Player's Dice:");

                float yOffset = 60f; // Start position for player labels within the menu

                // Iterate through each player in the current manager
                foreach (var player in CurrentManager.Players)
                {
                    // Display the player's name
                    string playerName = player.NetworkPlayerName;
                    GUI.Label(new Rect(10f, yOffset, menuX - 20f, 30f), playerName + ":");

                    // Display the player's dice values in reverse order
                    string diceValues = string.Join(" | ", player.GetComponent<DiceGamePlay>().DiceValues.Reverse());
                    GUI.Label(new Rect(10f, yOffset + 25f, menuX - 20f, 20f), diceValues);

                    // Create a button to call DrinkRpc for each player
                    if (GUI.Button(new Rect(10f, yOffset + 50f, menuX - 20f, 30f), "Make " + playerName + " Drink"))
                    {
                        // Use reflection to call the private method DrinkRpc
                        Type type = CurrentManager.GetType();
                        MethodInfo methodInfo = type.GetMethod("DrinkRpc", BindingFlags.NonPublic | BindingFlags.Instance);

                        if (methodInfo != null)
                        {
                            methodInfo.Invoke(CurrentManager, new object[] { player.gameObject });
                        }
                        else
                        {
                            Debug.LogError("Could not find DrinkRpc method.");
                        }
                    }

                    // Increment yOffset for the next player's information and button
                    yOffset += 90f; // Adding space for player name, dice values, and button
                }

                // Adding spacing before showing dice totals
                yOffset += 20f;

                // Label for total dice section
                GUI.Label(new Rect(10f, yOffset, menuX - 20f, 20f), "Total Dice:");
                yOffset += 25f;

                // Use reflection to invoke the CalculateAllDice method and show dice totals
                Type diceGamePlayType = CurrentDiceGamePlayManager.GetType();
                MethodInfo calculateAllDiceMethod = diceGamePlayType.GetMethod("CalculateAllDice", BindingFlags.NonPublic | BindingFlags.Instance);

                if (calculateAllDiceMethod != null)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        // Invoke the private method and pass the required parameter
                        object result = calculateAllDiceMethod.Invoke(CurrentDiceGamePlayManager, new object[] { i });

                        if (result is int diceCount)
                        {
                            // Display the dice total for each value (1 to 6)
                            GUI.Label(new Rect(10f, yOffset, menuX - 20f, 20f), string.Format("{0} Dice: {1}", i, diceCount));
                            yOffset += 25f; // Increment yOffset for each dice total
                        }
                    }
                }
                else
                {
                    // Display error if the CalculateAllDice method couldn't be found
                    GUI.Label(new Rect(10f, yOffset, menuX - 20f, 20f), "Error: Could not find CalculateAllDice method.");
                    yOffset += 25f;
                }

                // Make the window draggable
                GUI.DragWindow(new Rect(0f, 0f, menuX, menuY));
            }
        }
        #endregion

        private static int ScreenX = Screen.width;
        private static int ScreenY = Screen.height;
        private static int menuX = 300;
        private static int menuY = 700;
        private static int menuXOffset = cheatmenu.ScreenX - cheatmenu.menuX - 10;
        private static int menuYOffset = 0;
        private Rect menuRect = new Rect((float)cheatmenu.menuXOffset, (float)cheatmenu.menuYOffset, (float)cheatmenu.menuX, (float)cheatmenu.menuY);
        private bool menuOpened;
        private int bulletNumber = 5;
        private static Manager CurrentManager = null;
        private static DiceGamePlayManager CurrentDiceGamePlayManager = null;
        private static BlorfGamePlayManager CurrentBlorfGamePlayManager = null;
    }
}
