using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using HarmonyLib;
using InventorySystem.Items.Usables.Scp330;
using PluginAPI.Core;
using PlayerRoles.PlayableScps.Scp3114;
using PluginAPI.Roles;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp173;
using static PlayerList;
using PluginAPI.Core.Interfaces;
using Mirror;
using PlayerRoles;
using System;
using UnityEngine;
using System.Collections.Generic;
using PlayerRoles.PlayableScps.Scp049;

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
            Log.Info("Patch all.");
        }

        //[HarmonyPatch(typeof(CandyPink), "SpawnChanceWeight", MethodType.Getter)]
        //public class PinkCandyPatch
        //{
        //    public static void Postfix(ref float __result)
        //    {
        //        float candyWeight = Singleton.Config.CandyWeight;
        //        __result = candyWeight;
        //    }
        //}


        [HarmonyPatch(typeof(Scp3114Spawner), "OnPlayersSpawned")]
        public class Scp3114Patch
        {
            //private bool _ragdollsSpawned = true;
            //public List<ReferenceHub> SpawnCandidates = new List<ReferenceHub>();


            //[HarmonyPostfix]
            //[HarmonyPatch(typeof(Scp3114Spawner), "_ragdollsSpawned")]
            //public static bool GetRagdollSpawned()
            //{
            //    return Traverse.Create(typeof(Scp3114Spawner)).Field("_ragdollsSpawned").GetValue<bool>();
            //}


            //[HarmonyPostfix]
            //[HarmonyPatch(typeof(Scp3114Spawner), "SpawnCandidates")]
            //public static List<ReferenceHub> GetSpawnCandidates()
            //{
            //    List<ReferenceHub> SpawnCandidates = Traverse.Create(typeof(Scp3114Spawner)).Field("SpawnCandidates").GetValue<List<ReferenceHub>>();
            //    return SpawnCandidates;
            //}

            [HarmonyPostfix]
            //[HarmonyPatch(typeof(Scp3114Spawner), "OnPlayersSpawned")]
            private static void Postfix()
            {
                Traverse spawnerTraverse = Traverse.Create(typeof(Scp3114Spawner));
                //bool _ragdollsSpawned = spawnerTraverse.Field("_ragdollsSpawned").GetValue<bool>();
                bool _ragdollsSpawned = false;
                bool overrideHuman = Singleton.Config.Spawn3114OverridesHumanQueue;
                float spawnChance = Singleton.Config.Spawn3114Chance;
                int minPlayer = Singleton.Config.Spawn3114MinPlayers;
                
                List<ReferenceHub> SpawnCandidates = new List<ReferenceHub>();
                //SpawnCandidates.AddRange(spawnerTraverse.Field("SpawnCandidates").GetValue<List<ReferenceHub>>());
                Log.Info("Patch spawn");
                //Log.Info("Candidates: " + SpawnCandidates.Count.ToString());
                //Log.Info("RagdollSpawned: " + _ragdollsSpawned.ToString());

                if (!NetworkServer.active)
                {
                    return;
                }

                _ragdollsSpawned = false;
                //spawnerTraverse.Field("_ragdollsSpawned").SetValue(_ragdollsSpawned);
                Log.Info("Patch 50%");
                //Log.Info("RagdollSpawned: " + spawnerTraverse.Field("_ragdollsSpawned").GetValue<bool>().ToString());

                if (!(UnityEngine.Random.value >= spawnChance))
                {
                    SpawnCandidates.Clear();
                    //PlayerRolesUtils.ForEachRole<Scp173Role>(new Action<ReferenceHub>(SpawnCandidates.Add));
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
                        Log.Info("Try Spawn.");
                        SpawnCandidates.RandomItem().roleManager.ServerSetRole(RoleTypeId.Scp3114, RoleChangeReason.RoundStart);
                        _ragdollsSpawned = true;
                    }
                }
                spawnerTraverse.Field("_ragdollsSpawned").SetValue(_ragdollsSpawned);
                spawnerTraverse.Field("SpawnCandidates").GetValue<List<ReferenceHub>>().AddRange(SpawnCandidates);
                Log.Info("Patch finished.");
            }
            //public static void SpawnChancePrefix()
            //{
            //    float spawnChance = Singleton.Config.Spawn3114Chance;
            //    Traverse.Create(typeof(Scp3114Spawner)).Field("SpawnChance").SetValue(spawnChance);
            //}

            //[HarmonyPrefix]
            //[HarmonyPatch(typeof(Scp3114Spawner), "MinHumans")]
            //public static void SpawnMinPlayerPrefix() 
            //{
            //    //var originalMinPlayer = Traverse.Create(typeof(Scp3114Spawner)).Field("MinHumans").GetValue();
            //    int minPlayer = Singleton.Config.Spawn3114MinPlayers;
            //    Traverse.Create(typeof(Scp3114Spawner)).Field("MinHumans").SetValue(minPlayer);               
            //}


        }
    }
}