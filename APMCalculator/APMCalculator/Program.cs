using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace APMCalculator
{
    public static class Program
    {
        public static Menu Menu;
        public static int Actioncount = 0, APM;
        public static int lastupdatetick;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("APMCalculator", "APMCalculator");
            Menu.AddGroupLabel("APMCalculator 1.0.0");
            Menu.Add("Xpos", new Slider("X position", 10, 0, 1920));
            Menu.Add("Ypos", new Slider("X position", 10, 0, 1080));
            Menu.Add("Timeframe", new Slider("Timeframe", 1000, 500, 5000));

            lastupdatetick = Environment.TickCount;

            Spellbook.OnCastSpell += OnCastSpell;
            Player.OnIssueOrder += OnIssueOrder;
            Game.OnTick += onTick;
            Drawing.OnDraw += onDraw =>
            {
                Drawing.DrawText(10, 1020, System.Drawing.Color.Aqua, "APM: " + APM);
            };
        }

        private static void onTick(EventArgs args)
        {
            if (Environment.TickCount - lastupdatetick > Menu["Timeframe"].Cast<Slider>().CurrentValue)
            {
                APM = (int)((float)Actioncount/(Environment.TickCount - lastupdatetick)*60000);
                lastupdatetick = Environment.TickCount;
                Actioncount = 0;
            }
        }

        private static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (sender.IsMe) Actioncount++;
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe) Actioncount++;
        }
    }
}
