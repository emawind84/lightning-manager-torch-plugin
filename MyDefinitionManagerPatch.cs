using NLog;
using Sandbox.Definitions;
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
    public static class MyDefinitionManagerPatch
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static readonly MethodInfo getWeatherEffect =
            typeof(MyDefinitionManager).GetMethod(nameof(MyDefinitionManager.GetWeatherEffect), BindingFlags.Instance | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        internal static readonly MethodInfo getWeatherEffectPach =
            typeof(MyDefinitionManagerPatch).GetMethod(nameof(PrepareWeatherDefinition), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(getWeatherEffect).Suffixes.Add(getWeatherEffectPach);
            Log.Debug("Patching Successful MyDefinitionManager!");
        }

        public static void PrepareWeatherDefinition(MyDefinitionManager __instance, ref MyWeatherEffectDefinition __result, ref string subtype)
        {
            if (__result != null)
            {
                var weatherDefinition = __result;

                if (LightningManager.Config.LightningCharacterHitIntervalMax != -1)
                    weatherDefinition.LightningCharacterHitIntervalMax = LightningManager.Config.LightningCharacterHitIntervalMax;
                if (LightningManager.Config.LightningCharacterHitIntervalMin != -1)
                    weatherDefinition.LightningCharacterHitIntervalMin = LightningManager.Config.LightningCharacterHitIntervalMin;
                if (LightningManager.Config.LightningGridHitIntervalMax != -1)
                    weatherDefinition.LightningGridHitIntervalMax = LightningManager.Config.LightningGridHitIntervalMax;
                if (LightningManager.Config.LightningGridHitIntervalMin != -1)
                    weatherDefinition.LightningGridHitIntervalMin = LightningManager.Config.LightningGridHitIntervalMin;
                if (LightningManager.Config.LightningIntervalMax != -1)
                    weatherDefinition.LightningIntervalMax = LightningManager.Config.LightningIntervalMax;
                if (LightningManager.Config.LightningIntervalMin != -1)
                    weatherDefinition.LightningIntervalMin = LightningManager.Config.LightningIntervalMin;
            }
        }
    }
}
