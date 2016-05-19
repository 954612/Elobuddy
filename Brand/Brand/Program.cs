using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace BrandBrand
{
    public static class Program
    {
        // Change this line to the champion you want to make the addon for,
        // watch out for the case being correct!
        public const string ChampName = "ChampionName, example Annie or Teemo";

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

            // Initialize the classes that we need
            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();

            // Listen to events we need
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (!Config.DrawMenu.Get<CheckBox>("enbDraw").CurrentValue) return;

            if (Config.DrawMenu.Get<CheckBox>("drawQ").CurrentValue && SpellManager.Q.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, SpellManager.Q.Range, System.Drawing.Color.OrangeRed);
            }
            if (Config.DrawMenu.Get<CheckBox>("drawW").CurrentValue && SpellManager.W.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, SpellManager.W.Range, System.Drawing.Color.OrangeRed);
            }
            if (Config.DrawMenu.Get<CheckBox>("drawE").CurrentValue && SpellManager.E.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, SpellManager.E.Range, System.Drawing.Color.OrangeRed);
            }
            if (Config.DrawMenu.Get<CheckBox>("drawR").CurrentValue && SpellManager.R.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, SpellManager.R.Range, System.Drawing.Color.OrangeRed);
            }
        }
    }
}
