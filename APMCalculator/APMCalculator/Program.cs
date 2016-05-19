using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace APMCalculator
{
    public static class Program
    {
        public static Menu Menu;
        public static int Actioncount, RealActioncount, APM, RAPM;
        public static int lastupdatetick;
        public static int[] DataPoints, RealDP;
        public static int Xpos, Ypos;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            Menu = MainMenu.AddMenu("APMCalculator", "APMCalculator");
            Menu.AddGroupLabel("APMCalculator 1.0.0");
            Menu.Add("Xpos", new Slider("X position", 15, 0, 1920));
            Menu.Add("Ypos", new Slider("Y position", 1000, 0, 1080));
            Menu.Add("Timeframe", new Slider("Timeframe(ms)", 1000, 500, 5000));

            lastupdatetick = Environment.TickCount;
            DataPoints = new int[21];
            RealDP = new int[21];

            Spellbook.OnCastSpell += OnCastSpell;
            Player.OnIssueOrder += OnIssueOrder;
            Game.OnTick += onTick;
            Drawing.OnDraw += onDraw =>
            {
                Xpos = Menu["Xpos"].Cast<Slider>().CurrentValue;
                Ypos = Menu["Ypos"].Cast<Slider>().CurrentValue;

                Drawing.DrawText(Xpos, Ypos - 20, System.Drawing.Color.Aqua, "APM: " + APM + " MAX: " + DataPoints.Max() + " Real: " + RealDP.Max());
                Drawing.DrawLine(Xpos, Ypos, Xpos, Ypos + 50, 1f, Color.Aqua);
                Drawing.DrawLine(Xpos, Ypos, Xpos + 200, Ypos, 1f, Color.Aqua);
                Drawing.DrawLine(Xpos + 200, Ypos, Xpos + 200, Ypos + 50, 1f, Color.Aqua);
                Drawing.DrawLine(Xpos, Ypos + 50, Xpos + 200, Ypos + 50, 1f, Color.Aqua);

                for (int i = 0; i < 20; i++)
                {
                    Drawing.DrawLine(Xpos + i * 10, Ypos + 50 - (float)RealDP[i] / DataPoints.Max() * 50, Xpos + i * 10 + 10, Ypos + 50 - (float)RealDP[i + 1] / DataPoints.Max() * 50, 1f, Color.Red);
                    Drawing.DrawLine(Xpos + i * 10, Ypos + 50 - (float)DataPoints[i] / DataPoints.Max() * 50, Xpos + i * 10 + 10, Ypos + 50 - (float)DataPoints[i + 1] / DataPoints.Max() * 50, 1f, Color.Aqua);
                }


            };
        }

        private static void onTick(EventArgs args)
        {
            if (Environment.TickCount - lastupdatetick > Menu["Timeframe"].Cast<Slider>().CurrentValue)
            {
                APM = (int)((float)Actioncount / (Environment.TickCount - lastupdatetick) * 60000);
                RAPM = (int)((float)RealActioncount / (Environment.TickCount - lastupdatetick) * 60000);
                lastupdatetick = Environment.TickCount;

                for (int i = 20; i > 0; i--)
                {
                    DataPoints[i] = DataPoints[i - 1];
                    RealDP[i] = RealDP[i - 1];
                }

                DataPoints[0] = APM;
                RealDP[0] = RAPM;
                Actioncount = 0;
                RealActioncount = 0;
            }
        }

        private static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (sender.IsMe) Actioncount++;
            if (args.Process) RealActioncount++;
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe) Actioncount++;
            if (args.Process) RealActioncount++;
        }
    }
}
