using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace BrandBrand.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Since this is permaactive mode, always execute the loop
            return true;
        }

        public override void Execute()
        {
            const int range = 1100;
            const float hitchance = 56f;
            const float aoeratio = 0.2f;

            var enemies = EntityManager.Heroes.Enemies.Where(n => n.IsValidTarget(range));
            var selectedTarget = TargetSelector.GetTarget(range, DamageType.Magical);
            var allTargets =
                new[] { selectedTarget }.Concat(
                    enemies.Where(n => n.Index != selectedTarget.Index)
                        .OrderByDescending(n => Damage.TotalDamage(SpellSlot.Q, n) / n.Health)).Where(n => n.IsValidTarget() && !n.WillDie());

            if (selectedTarget == null && !enemies.Any())
            {
                return;
            }

            var ksQ = Config.Menu["ksQ"].Cast<CheckBox>().CurrentValue &&
                      Player.CanUseSpell(SpellSlot.Q) == SpellState.Ready;
            var ksW = Config.Menu["ksW"].Cast<CheckBox>().CurrentValue &&
                      Player.CanUseSpell(SpellSlot.W) == SpellState.Ready;
            var ksE = Config.Menu["ksE"].Cast<CheckBox>().CurrentValue &&
                      Player.CanUseSpell(SpellSlot.E) == SpellState.Ready;
            var ksR = Config.Menu["ksR"].Cast<CheckBox>().CurrentValue &&
                      Player.CanUseSpell(SpellSlot.R) == SpellState.Ready;

            foreach (var enemy in allTargets)
            {
                if (ksE && Player.Instance.IsInRange(enemy, E.Range) && enemy.Killable(SpellSlot.E))
                {
                    E.Cast(enemy);
                    return;
                }

                if (ksW && Player.Instance.IsInRange(enemy, W.Range + W.Radius) && enemy.Killable(SpellSlot.W))
                {
                    var prediction = W.GetPrediction(enemy);

                    if (prediction.HitChancePercent >= hitchance)
                    {
                        W.Cast(prediction.CastPosition);
                        return;
                    }
                }

                if (ksQ && Player.Instance.IsInRange(enemy, Q.Range) && enemy.Killable(SpellSlot.Q))
                {
                    var prediction = Q.GetPrediction(enemy);

                    if (prediction.HitChancePercent >= hitchance)
                    {
                        Q.Cast(prediction.CastPosition);
                        return;
                    }
                }

                if (ksR && Player.Instance.IsInRange(enemy, R.Range) && enemy.Killable(SpellSlot.R))
                {
                    R.Cast(enemy);
                    return;
                }
            }
        }
    }
}
