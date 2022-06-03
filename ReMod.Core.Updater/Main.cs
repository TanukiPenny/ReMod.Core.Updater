using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using MelonLoader;
using ReMod.Core.Updater;

[assembly: AssemblyDescription(BuildShit.Description)]
[assembly: AssemblyCopyright($"Created by {BuildShit.Author}, Copyright © 2022")]
[assembly: MelonInfo(typeof(ReMod.Core.Updater.Main), BuildShit.Name, BuildShit.Version, BuildShit.Author, BuildShit.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ReMod.Core.Updater
{
    
    public static class BuildShit
    {
        public const string Name = "ReMod.Core.Updater";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/ReMod.Core.Updater";
        public const string Description = "A plugin that handles all updating of ReMod.Core";
    }
    
    public class Main : MelonPlugin
    {
        
        public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.Cyan);
        
        public override void OnPreInitialization()
        {
            
            var sha256 = SHA256.Create();
            byte[] localcore, remotecore;
            
            if (!File.Exists("ReMod.Core.dll"))
            {
                remotecore = new WebClient().DownloadData(
                    "https://github.com/RequiDev/ReMod.Core/releases/latest/download/ReMod.Core.dll");
                File.WriteAllBytes("ReMod.Core.dll", remotecore);
                Log.Msg("ReMod.Core downloaded!");
            }
            else
            {
                remotecore = new WebClient().DownloadData(
                    "https://github.com/RequiDev/ReMod.Core/releases/latest/download/ReMod.Core.dll");
                var remotehash = ComputeHash(sha256, remotecore);
                
                localcore = File.ReadAllBytes("ReMod.Core.dll");
                var localhash = ComputeHash(sha256, localcore);

                if (localhash != remotehash)
                {
                    File.WriteAllBytes("ReMod.Core.dll", remotecore);
                    Log.Msg("ReMod.Core updated!");
                }
                else
                {
                    Log.Msg("ReMod.Core is already up to date!");
                }
                
            }

        }
        
        private static string ComputeHash(HashAlgorithm sha256, byte[] data)
        {
            var bytes = sha256.ComputeHash(data);
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
    
}