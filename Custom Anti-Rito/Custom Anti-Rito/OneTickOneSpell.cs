using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Direct3D;
using System.Drawing;

namespace CustomAntiRito
{
    public static class OneTickOneSpell
    {
        public static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static LastSpellCast LastSpell = new LastSpellCast();
        public static List<LastSpellCast> LastSpellsCast = new List<LastSpellCast>();
        public static int BlockedCount = 0;
        public static Menu menu, config, ConfigMenu;
        public static Random _random;
        public static Dictionary<string, int> _lastCommandT;
        public static bool _thisMovementCommandHasBeenTamperedWith = false;

        public static void InitSettings()
        {
            menu = MainMenu.AddMenu("Custom Anti-Rito", "Anti Rito");
            menu.AddGroupLabel("Custom Anti-Rito");
            menu.AddLabel("Version: " + "1.0.0.0");

            ConfigMenu = menu.AddSubMenu("OneSpellOneTick", "OneSpellOneTick");
            ConfigMenu.AddGroupLabel("Settings");
            ConfigMenu.Add("Spells", new CheckBox("Humanize Spells"));
            ConfigMenu.Add("ImportantSpells", new CheckBox("Humanize Ult/Summoners"));
            //ConfigMenu.Add("Recast", new CheckBox("Recast blocked spell on next tick"));
            ConfigMenu.Add("Attacks", new CheckBox("Humanize Attacks"));
            ConfigMenu.Add("Movements", new CheckBox("Humanize Movements"));
            ConfigMenu.Add("MinClicks", new Slider("Min clicks per second", 10, 1, 10));
            ConfigMenu.Add("MaxClicks", new Slider("Max clicks per second", 15, 10, 15));
            ConfigMenu.Add("Drawing", new CheckBox("Show Block Count", true));
        }

        public static void Init()
        {
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            EloBuddy.Player.OnIssueOrder += PlayerOnOnIssueOrder;
            Drawing.OnDraw += onDrawArgs =>
            {
                if (ConfigMenu["Drawing"].Cast<CheckBox>().CurrentValue)
                {
                    Drawing.DrawText(15, 1055, System.Drawing.Color.Aqua, "Blocked orders: " + BlockedCount);
                }
            };
        }

        private static void PlayerOnOnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs issueOrderEventArgs)
        {
            if (sender.IsMe && !issueOrderEventArgs.IsAttackMove)
            {
                if (issueOrderEventArgs.Order == GameObjectOrder.AttackUnit ||
                    issueOrderEventArgs.Order == GameObjectOrder.AttackTo &&
                    !ConfigMenu["Attacks"].Cast<CheckBox>().CurrentValue)
                    return;
                if (issueOrderEventArgs.Order == GameObjectOrder.MoveTo &&
                    !ConfigMenu["Movements"].Cast<CheckBox>().CurrentValue)
                    return;
            }

            var orderName = issueOrderEventArgs.Order.ToString();
            var order = _lastCommandT.FirstOrDefault(e => e.Key == orderName);
            if (Environment.TickCount - order.Value <
                Randomize(1000 / ConfigMenu["MaxClicks"].Cast<Slider>().CurrentValue,
                    1000 / ConfigMenu["MinClicks"].Cast<Slider>().CurrentValue) + _random.Next(-10, 10))
            {
                BlockedCount += 1;
                issueOrderEventArgs.Process = false;
                return;
            }
            if (issueOrderEventArgs.Order == GameObjectOrder.MoveTo &&
                        issueOrderEventArgs.TargetPosition.IsValid() && !_thisMovementCommandHasBeenTamperedWith)
            {
                _thisMovementCommandHasBeenTamperedWith = true;
                issueOrderEventArgs.Process = false;
                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo,
                    Randomize(issueOrderEventArgs.TargetPosition, -10, 10));
            }
            _thisMovementCommandHasBeenTamperedWith = false;
            _lastCommandT.Remove(orderName);
            _lastCommandT.Add(orderName, Environment.TickCount);
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (!ConfigMenu["Spells"].Cast<CheckBox>().CurrentValue)
                return;
            if (!sender.Owner.IsMe)
                return;
            if (!(new SpellSlot[] {SpellSlot.Q,SpellSlot.W,SpellSlot.E,SpellSlot.R,SpellSlot.Summoner1,SpellSlot.Summoner2
                ,SpellSlot.Item1,SpellSlot.Item2,SpellSlot.Item3,SpellSlot.Item4,SpellSlot.Item5,SpellSlot.Item6,SpellSlot.Trinket})
                .Contains(args.Slot))
                return;

            if (LastSpellsCast.Any(x => x.Slot == args.Slot))
            {
                LastSpellCast spell = LastSpellsCast.FirstOrDefault(x => x.Slot == args.Slot);
                if (spell != null)
                {
                    if (Environment.TickCount - spell.CastTick <= 100 + Game.Ping)
                    {
                        args.Process = false;
                        BlockedCount += 1;
                    }
                    else
                    {
                        LastSpellsCast.RemoveAll(x => x.Slot == args.Slot);
                        LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount });
                    }
                }
                else
                {
                    LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount });
                }
            }
            else
            {
                LastSpellsCast.Add(new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount });
            }

            if (!ConfigMenu["ImportantSpells"].Cast<CheckBox>().CurrentValue &&
                !(new SpellSlot[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E }).Contains(args.Slot))
                return;

            if (Environment.TickCount - LastSpell.CastTick < 33)
            {
                args.Process = false;
                BlockedCount += 1;
            }
            else
            {
                LastSpell = new LastSpellCast() { Slot = args.Slot, CastTick = Environment.TickCount };
            }
            
        }

        public class LastSpellCast
        {
            public SpellSlot Slot = SpellSlot.Unknown;
            public int CastTick = 0;
        }

        public static double Randomize(int min, int max)
        {
            var x = _random.Next(min, max) + 1 + 1 - 1 - 1;
            var y = _random.Next(min, max);
            if (_random.Next(0, 1) > 0)
            {
                return x;
            }
            if (1 == 1)
            {
                return (x + y) / 2d;
            }
            return y;
        }

        public static Vector3 Randomize(Vector3 position, int min, int max)
        {
            var ran = new Random(Environment.TickCount);
            return position + new Vector2(ran.Next(min, max), ran.Next(min, max)).To3D();
        }
    }
}
