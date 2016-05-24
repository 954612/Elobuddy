using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

using Color = System.Drawing.Color;

namespace Custom_Gank_Potential_Tracker
{
    class Program
    {
        public static Menu TrackerMenu;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            TrackerMenu = MainMenu.AddMenu("GPTracker", "GPTracker");
            Tracker.OnLoad();
            Drawing.OnEndScene += OnEndScene;
            AIHeroClient.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
        }

        private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!args.SData.Name.ToLower().StartsWith("summoner") || sender.IsAlly || !Tracker.TrackMenu["trackprint"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            string SpellS = "";

            switch (args.SData.Name.ToLower())
            {
                case "summonerheal":
                    {
                        SpellS = "Heal";
                        break;
                    }
                case "summonerdot"://ignite
                    {
                        SpellS = "Ignite";
                        break;
                    }
                case "summonerexhaust":
                    {
                        SpellS = "Exhaust";
                        break;
                    }
                case "summonerflash":
                    {
                        SpellS = "Flash";
                        break;
                    }
                case "summonerhaste"://Ghost
                    {
                        SpellS = "Ghost";
                        break;
                    }
                case "summonermana"://Clarity
                    {
                        SpellS = "Clarity";
                        break;
                    }
                case "summonerbarrier":
                    {
                        SpellS = "Barrier";
                        break;
                    }
                case "summonerteleport":
                    {
                        SpellS = "Teleport";
                        break;
                    }
                case "summonerboost"://Cleanse
                    {
                        SpellS = "Cleanse";
                        break;
                    }
                case "summonerclairvoyance"://Clairvoyance
                    {
                        SpellS = "Clairvoyance";
                        break;
                    }
                default:
                    return;
            }

            Chat.Print("<font color='#FFFF00'>[Summoner Spell Timer] {0} used {1}.</font>", Color.White, sender.BaseSkinName, SpellS);
        }

        private static void OnEndScene(EventArgs args)
        {
            Tracker.HPtrack();
        }
    }
}
