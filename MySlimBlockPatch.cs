using NLog;
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
    public static class MySlimBlockPatch
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static readonly MethodInfo doDamage =
            typeof(MySlimBlock).GetMethod(nameof(MySlimBlock.DoDamage), new[] { typeof(float), typeof(MyStringHash), typeof(bool), typeof(MyHitInfo), typeof(long) }) ??
            throw new Exception("Failed to find patch method");

        internal static readonly MethodInfo doDamagePatch =
            typeof(MySlimBlockPatch).GetMethod(nameof(LogBlockDamage), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(doDamage).Prefixes.Add(doDamagePatch);
            Log.Debug("Patching Successful MySlimBlock!");
        }

        public static bool LogBlockDamage(MySlimBlock __instance, ref float damage, ref MyStringHash damageType)
        {
            Log.Debug($"Block damage ({damageType}): {damage}");
            
            return true;
        }
    }
}
