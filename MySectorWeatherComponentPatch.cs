using NLog;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.Managers.PatchManager;
using VRage.Game;
using VRage.Utils;

namespace LightningManager
{
    [PatchShim]
    public static class MySectorWeatherComponentPatch
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal static readonly MethodInfo createLightning =
            typeof(MySectorWeatherComponent).GetMethod(nameof(MySectorWeatherComponent.CreateLightning), BindingFlags.Instance | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        internal static readonly MethodInfo createLightningPatch =
            typeof(MySectorWeatherComponentPatch).GetMethod(nameof(ChangeLightningSettings), BindingFlags.Static | BindingFlags.Public) ??
            throw new Exception("Failed to find patch method");

        public static void Patch(PatchContext ctx)
        {
            ctx.GetPattern(createLightning).Prefixes.Add(createLightningPatch);
            Log.Debug("Patching Successful MySectorWeatherComponent!");
        }

        public static void ChangeLightningSettings(MySectorWeatherComponent __instance, ref MyObjectBuilder_WeatherLightning lightning, bool doDamage)
        {
            if (lightning != null)
            {
                lightning.Damage = LightningManager.LightningDamage;
                Log.Debug($"Lightning damage: {lightning.Damage} - damageflag: {doDamage}");
            }
        }
        
    }
}
