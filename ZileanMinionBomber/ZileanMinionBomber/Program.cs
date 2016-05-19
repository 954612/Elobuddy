using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace AddonTemplate
{
    public static class Program
    {
        // Change this line to the champion you want to make the addon for,
        // watch out for the case being correct!
        public const string ChampName = "Zilean";

        public static Menu ZmbMenu;

        public static SpellDataInst Q, W;

        public static void Main(string[] args)
        {
            // Wait till the loading screen has passed
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            // Verify the champion we made this addon for
            if (Player.Instance.ChampionName != ChampName)
            {
                // Champion is not the one we made this addon for,
                // therefore we return
                return;
            }

            ZmbMenu = MainMenu.AddMenu("ZileanMinionBomber", "ZileanMinionBomber");
            ZmbMenu.Add("enb", new CheckBox("Enable"));
            ZmbMenu.Add("enbCombo", new CheckBox("Enable On Combo"));
            ZmbMenu.Add("enbHarass", new CheckBox("Enable On Harass"));

            Q = Player.GetSpell(SpellSlot.Q);
            W = Player.GetSpell(SpellSlot.W);

            // Listen to events we need
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;

            Chat.Print("ZileanMinionBomber Loaded!");
        }

        private static void OnTick(EventArgs args)
        {
            if (!ZmbMenu["enb"].Cast<CheckBox>().CurrentValue) return;
            if (Player.Instance.IsDead || MenuGUI.IsChatOpen) return;

            if ((!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                 !ZmbMenu["enbCombo"].Cast<CheckBox>().CurrentValue) &&
                (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                 !ZmbMenu["enbHarass"].Cast<CheckBox>().CurrentValue)) return;
            
            //if(TargetSelector.GetTarget(900,DamageType.Magical,Player.Instance.Position).IsValid()) return;

            var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Both, Player.Instance.Position, 850);
            foreach (var minion in minions)
            {
                if (minion.IsInRange(Prediction.Position.PredictUnitPosition(TargetSelector.GetTarget(1100, DamageType.Magical, Player.Instance.Position, true), 250), 250) &&
                    !minion.IsInRange(Prediction.Position.PredictUnitPosition(TargetSelector.GetTarget(1100, DamageType.Magical, Player.Instance.Position, true), 500), 100))
                {
                    var pos = minion.Position;
                    if (!Q.IsReady || !W.IsReady) continue;
                    Player.CastSpell(SpellSlot.Q, pos);      
                    Core.DelayAction(() => Player.CastSpell(SpellSlot.W), 250);
                    Core.DelayAction(() => Player.CastSpell(SpellSlot.Q, pos), 500);
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
        }
    }
}
