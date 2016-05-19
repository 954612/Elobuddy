using System;
using System.Security.Cryptography.X509Certificates;
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
        public const string ChampName = "Tristana";

        public static Menu TEA;

        public static SpellDataInst E;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName) return;

            TEA = MainMenu.AddMenu("Tristana Auto E", "TEA");
            TEA.Add("enb", new CheckBox("Enable"));
            TEA.Add("harassOnly", new CheckBox("Harass Only"));
            TEA.Add("useCombo", new CheckBox("Use in Combo"));

            E = Player.GetSpell(SpellSlot.E);

            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;

            Chat.Print("Tristana E Auto Loaded");
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || MenuGUI.IsChatOpen || !TEA["enb"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (TEA["harassOnly"].Cast<CheckBox>().CurrentValue &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) return;

            if (!TEA["useCombo"].Cast<CheckBox>().CurrentValue &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

            if (!E.IsReady) return;

            var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy ,Player.Instance.Position, Player.Instance.GetAutoAttackRange());
            var heroes = EntityManager.Heroes.Enemies;

            foreach (var minion in minions)
            {
                foreach (var hero in heroes)
                {
                    if (Prediction.Health.GetPrediction(minion, 300) <= Player.Instance.GetAutoAttackDamage(minion) &&
                        minion.IsInRange(Prediction.Position.PredictUnitPosition(hero, 500), 100) && Orbwalker.CanAutoAttack)
                    {
                        Orbwalker.ForcedTarget = minion;
                        Player.CastSpell(SpellSlot.E, minion);
                        Core.DelayAction(Orbwalker.ResetAutoAttack, 150);
                    }
                }
            }

        }

        private static void OnDraw(EventArgs args)
        {

        }
    }
}
