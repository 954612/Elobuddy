using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace BrandBrand
{
    public static class SpellManager
    {
        // You will need to edit the types of spells you have for each champ as they
        // don't have the same type for each champ, for example Xerath Q is chargeable,
        // right now it's  set to Active.
        public const int ConflagrationSpreadRange = 300;
        public const int PyroclasmSpreadRange = 600;

        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        static SpellManager()
        {
            // Initialize spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1600, 60);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 850, -1, 250);
            E = new Spell.Targeted(SpellSlot.E, 640);
            R = new Spell.Targeted(SpellSlot.R, 750);
        }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }

        public static bool ShouldCast(bool allowAutos = true)
        {
            return !Player.Instance.Spellbook.IsCastingSpell ||
                   (!allowAutos || (Player.Instance.Spellbook.IsAutoAttacking && Orbwalker.CanBeAborted));
        }

        public static bool IsBlazed(this Obj_AI_Base target)
        {
            return target.HasBuff("BrandAblaze");
        }
    }
}
