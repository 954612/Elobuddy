using System;
using System.Security.Cryptography.X509Certificates;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Tester
{
    public static class Program
    {
        // Change this line to the champion you want to make the addon for,
        // watch out for the case being correct!
        //public const string ChampName = "Brand";

        public static int lasttick;
        public static int dtick;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            Drawing.OnDraw += OnDraw;
            Game.OnTick += onTick;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Player.OnIssueOrder += Player_OnIssueOrder;

            Chat.Print("Tester Loaded");
        }

        private static void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            
            if (!sender.Owner.IsMe) return;
            Chat.Print("Spellcast by " + sender.Owner.CharData.ToString());
            Chat.Print("Slot:"+args.Slot.ToString() + " Startpos:"+ args.StartPosition.ToString() +" Endpos:" + args.EndPosition.ToString() +
                       " Target:" +args.Target.ToString());
        }

        private static void onTick(EventArgs args)
        {
        }

        private static void OnDraw(EventArgs args)
        {
        }
    }
}
