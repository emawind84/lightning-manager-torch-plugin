using NLog;
using Sandbox.Game;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Torch.Managers.PatchManager;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace LightningManager
{
    [PatchShim]
    public static class MyExplosionsPatch
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static readonly MethodInfo addExplosion =
            typeof(MyExplosions).GetMethod(nameof(MyExplosions.AddExplosion), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        internal static readonly MethodInfo addExplosionPatch =
            typeof(MyExplosionsPatch).GetMethod(nameof(DoBeforeAddExplosion), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(addExplosion).Prefixes.Add(addExplosionPatch);
            Log.Debug("Patching Successful MyExplosions!");
        }

        public static bool DoBeforeAddExplosion(ref MyExplosionInfo explosionInfo)
        {
            if (explosionInfo.HitEntity == null)
            {
                explosionInfo.HitEntity = new VRage.Game.Entity.MyEntity();  // bug fix armor blocks get destroyed at the first hit
            }

            // this change damage for every explosion
            //explosionInfo.Damage = LightningManager.LightningDamage;

            return true;
        }
    }
}
