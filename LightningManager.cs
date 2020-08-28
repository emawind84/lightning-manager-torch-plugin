using NLog;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Commands;
using Torch.Managers.ChatManager;
using Torch.Session;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace LightningManager
{
    public class LightningManager : TorchPluginBase
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Persistent<LightningManagerConfig> _config;
        public static LightningManagerConfig Config;

        private readonly Stopwatch stopWatch = new Stopwatch();

        private readonly double PluginCycleInSeconds = 10;

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            SetupConfig();
            
            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
            {
                sessionManager.SessionStateChanged += SessionChanged;
            }
            else
            {
                Log.Warn("No session manager loaded!");
            }
        }

        public override void Update()
        {

            base.Update();

            try
            {
                /* stopWatch not running? Nothing to do */
                if (!stopWatch.IsRunning)
                    return;

                /* Session not loaded? Nothing to do */
                if (Torch.CurrentSession == null || Torch.CurrentSession.State != TorchSessionState.Loaded)
                    return;

                var elapsed = stopWatch.Elapsed;
                if (elapsed.TotalSeconds < PluginCycleInSeconds)
                    return;
                
                MyAPIGateway.Utilities.InvokeOnGameThread(() => {

                    IMyWeatherEffects effects = Torch.CurrentSession.KeenSession.WeatherEffects;
                    var players = MySession.Static.Players.GetOnlinePlayers();
                    foreach (var player in players)
                    {
                        string weather = effects.GetWeather(player.GetPosition());
                        float intensity = effects.GetWeatherIntensity(player.GetPosition());
                        Log.Debug($"Weather {weather} intensity near {player.DisplayName} is {intensity}");
                        
                    }
                    
                });

                stopWatch.Restart();
            }
            catch (Exception e)
            {
                Log.Error(e, "Something is not right");
            }
        }

        public void Save()
        {
            try
            {
                _config.Save();
                Log.Debug("Configuration Saved.");
            }
            catch (IOException e)
            {
                Log.Warn(e, "Configuration failed to save");
            }
        }

        private void SetupConfig()
        {
            var configFile = Path.Combine(StoragePath, "LightningManager.cfg");

            try
            {
                _config = Persistent<LightningManagerConfig>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
            {
                Log.Debug("Create Default Config, because none was found!");

                _config = new Persistent<LightningManagerConfig>(configFile, new LightningManagerConfig());
                _config.Save();
            }

            Config = _config?.Data;
        }

        private void SessionChanged(ITorchSession session, TorchSessionState newState)
        {
            if (newState == TorchSessionState.Loaded)
            {
                stopWatch.Start();
                Log.Debug("Session loaded, start backup timer!");
            }
            else if (newState == TorchSessionState.Unloading)
            {
                stopWatch.Stop();
                Log.Debug("Session Unloading, suspend backup timer!");
            }
        }
    }
}
