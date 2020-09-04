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

                if (LightningManagerPlugin.Config.LightningCharacterHitIntervalMax != -1)
                    weatherDefinition.LightningCharacterHitIntervalMax = LightningManagerPlugin.Config.LightningCharacterHitIntervalMax;
                if (LightningManagerPlugin.Config.LightningCharacterHitIntervalMin != -1)
                    weatherDefinition.LightningCharacterHitIntervalMin = LightningManagerPlugin.Config.LightningCharacterHitIntervalMin;
                if (LightningManagerPlugin.Config.LightningGridHitIntervalMax != -1)
                    weatherDefinition.LightningGridHitIntervalMax = LightningManagerPlugin.Config.LightningGridHitIntervalMax;
                if (LightningManagerPlugin.Config.LightningGridHitIntervalMin != -1)
                    weatherDefinition.LightningGridHitIntervalMin = LightningManagerPlugin.Config.LightningGridHitIntervalMin;
                if (LightningManagerPlugin.Config.LightningIntervalMax != -1)
                    weatherDefinition.LightningIntervalMax = LightningManagerPlugin.Config.LightningIntervalMax;
                if (LightningManagerPlugin.Config.LightningIntervalMin != -1)
                    weatherDefinition.LightningIntervalMin = LightningManagerPlugin.Config.LightningIntervalMin;
            }
        }
    }
}
