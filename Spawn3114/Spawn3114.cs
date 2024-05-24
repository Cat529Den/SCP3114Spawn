using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using HarmonyLib;
using PluginAPI.Core;
using PlayerRoles.PlayableScps.Scp3114;
using PluginAPI.Roles;
using PlayerRoles.PlayableScps;
using static PlayerList;
using PluginAPI.Core.Interfaces;
using Mirror;
using PlayerRoles;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Spawn3114
{
    public class Spawn3114
    {
        

        public static Spawn3114 Singleton;

        [PluginConfig]
        public Config Config;

        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("Spawn3114", "0.1.0", "Naturally spawns SCP-3114.", "529")]

        public void LoadPlugin()
        {
            if (!Config.IsEnabled)
            {
                return;
            }

            Singleton = this;
            EventManager.RegisterEvents(this);

            Harmony harmony = new Harmony("Spawn3114test");
            Log.Info("In Patch");
            harmony.PatchAll();
            //Log.Info("Patch all.");
        }


        [HarmonyPatch(typeof(Scp3114Spawner), "OnPlayersSpawned")]
        public class Scp3114Patch
        {

            [HarmonyPostfix]
            private static void Postfix()
            {
                Traverse spawnerTraverse = Traverse.Create(typeof(Scp3114Spawner));
                bool _ragdollsSpawned = false;
                bool overrideHuman = Singleton.Config.Spawn3114OverridesHumanQueue;
                float spawnChance = Singleton.Config.Spawn3114Chance;
                int minPlayer = Singleton.Config.Spawn3114MinPlayers;
                
                List<ReferenceHub> SpawnCandidates = new List<ReferenceHub>();
                //Log.Info("Patch spawn");

                if (!NetworkServer.active)
                {
                    return;
                }

                _ragdollsSpawned = false;
                //Log.Info("Patch 50%");

                if (!(UnityEngine.Random.value >= spawnChance))
                {
                    SpawnCandidates.Clear();
                    if (overrideHuman)
                    {
                        PlayerRolesUtils.ForEachRole<HumanRole>(new Action<ReferenceHub>(SpawnCandidates.Add));
                    }
                    else
                    {
                        PlayerRolesUtils.ForEachRole<FpcStandardScp>(new Action<ReferenceHub>(SpawnCandidates.Add));
                    }
                    
                    Log.Info("value greater than " + spawnChance.ToString() + "candidates: " + SpawnCandidates.Count.ToString());
                    if (SpawnCandidates.Count >= minPlayer)
                    {
                        //Log.Info("Try Spawn.");
                        SpawnCandidates.RandomItem().roleManager.ServerSetRole(RoleTypeId.Scp3114, RoleChangeReason.RoundStart);
                        _ragdollsSpawned = true;
                    }
                }
                spawnerTraverse.Field("_ragdollsSpawned").SetValue(_ragdollsSpawned);
                spawnerTraverse.Field("SpawnCandidates").GetValue<List<ReferenceHub>>().AddRange(SpawnCandidates);
                //Log.Info("Patch finished.");
            }

        }
    }
}