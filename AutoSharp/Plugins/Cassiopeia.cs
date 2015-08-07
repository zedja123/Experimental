﻿using System;
using AutoSharp.Utils;
using LeagueSharp;
using LeagueSharp.Common;

namespace AutoSharp.Plugins
{
    public class Cassiopeia : PluginBase
    {
        public Cassiopeia()
        {
            Q = new Spell(SpellSlot.Q, 850);
            Q.SetSkillshot(0.6f, 40f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            W = new Spell(SpellSlot.W, 850);
            W.SetSkillshot(0.5f, 90f, 2500, false, SkillshotType.SkillshotCircle);

            E = new Spell(SpellSlot.E, 700);
            E.SetTargetted(0.2f, float.MaxValue);

            R = new Spell(SpellSlot.R, 800);
            R.SetSkillshot(0.6f, (float)(80 * Math.PI / 180), float.MaxValue, false, SkillshotType.SkillshotCone);
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                if (E.IsReady() && Heroes.Player.Distance(Target) < E.Range && Target.HasBuffOfType(BuffType.Poison))
                {
                    E.Cast(Target);
                }
                if (Q.IsReady() && Heroes.Player.Distance(Target) < Q.Range)
                {
                    Q.Cast(Target, UsePackets);
                }
                if (W.IsReady() && Heroes.Player.Distance(Target) < W.Range)
                {
                    W.Cast(Target, UsePackets);
                }
                if (R.IsReady() && Target.IsValidTarget(R.Range))
                {
                    R.CastIfWillHit(Target, 2);
                }
            }
            if (HarassMode)
            {
                if (E.IsReady() && Heroes.Player.Distance(Target) < E.Range && Target.HasBuffOfType(BuffType.Poison))
                {
                    E.Cast(Target);
                }
                if (Q.IsReady() && Heroes.Player.Distance(Target) < Q.Range)
                {
                    Q.Cast(Target, UsePackets);
                }
            }
        }

        public override void OnPossibleToInterrupt(Obj_AI_Hero unit, Interrupter2.InterruptableTargetEventArgs spell)
        {
            if (spell.DangerLevel < Interrupter2.DangerLevel.High || unit.IsAlly)
            {
                return;
            }

            if (R.CastCheck(unit, "Interrupt.R"))
            {
                R.Cast(unit);
            }
        }

        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboE", "Use E", true);
            config.AddBool("ComboR", "Use R", true);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("Interrupt.R", "Use R to Interrupt Spells", true);
        }
    }
}