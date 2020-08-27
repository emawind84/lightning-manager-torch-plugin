using NLog;
using Sandbox.Game;
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
        public LightningManagerConfig Config => _config?.Data;

        public static int LightningDamage;

        private readonly Stopwatch stopWatch = new Stopwatch();

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            SetupConfig();
            
            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");
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
                if (elapsed.TotalSeconds < TimeSpan.FromSeconds(10).TotalSeconds)
                    return;

                //var entities = new HashSet<IMyEntity>();
                //MyAPIGateway.Entities.GetEntities(entities);
                //foreach (var entity in entities)
                //{
                //    Log.Info("Found entity: " + entity.GetFriendlyName());
                //}

                ////MyVisualScriptLogicProvider.AddToPlayersInventory();
                
                //Log.Info("do stuff here");
                //foreach (var mod in MySession.Static.Mods)
                //{
                //    Log.Info("Mod loaded: " + mod.FriendlyName);
                //}

                MyAPIGateway.Utilities.InvokeOnGameThread(() => {
                    //DO STUFF

                    //MyAPIGateway.Players.GetPlayers(players);
                    var players = MySession.Static.Players.GetOnlinePlayers();
                    
                    foreach (var player in players)
                    {
                        //Log.Info(player.DisplayName);
                        //MySession.Static.Players.TryGetSteamId()
                        //Torch.CurrentSession.Managers.GetManager<ChatManagerServer>()?.SendMessageAsOther("Bubba", "AAAAAA", Color.Red, player.Client.SteamUserId);
                        //Plugin.Instance.Torch.CurrentSession.Managers.GetManager<ChatManagerServer>()?.SendMessageAsOther(AuthorName, StringMsg, Color, ulong);
                        //items.Add()

                        IMyWeatherEffects effects = Torch.CurrentSession.KeenSession.WeatherEffects;
                        string weather = effects.GetWeather(player.GetPosition());
                        float intensity = effects.GetWeatherIntensity(player.GetPosition());
                        Log.Debug($"Weather {weather} intensity near {player.DisplayName} is {intensity}");
                        
                    }
                    
                });

                stopWatch.Restart();

                // do stuff here

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

            LightningDamage = Config.LightningDamage;
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
