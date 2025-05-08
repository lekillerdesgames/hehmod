using System;
using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace hehmod
{
    [BepInPlugin(Id, "hehmod", VersionString)]
    [BepInProcess("Among Us.exe")]
    public class hehmod : BasePlugin
    {
        public const string Id = "com.lekillerdesgames.hehmod";
        public const string VersionString = "1.0.0";
        public static Sprite heh;

        private static DLoadImage _iCallLoadImage;
        public override void Load()
        {
            System.Console.WriteLine("it's heh time :heh:");

            heh = CreateSprite("hehmod.Resources.heh.png");

            IL2CPPChainloader.Instance.Finished += PatchAll;
        }

        public static void PatchAll()
        {
            var _harmony = new Harmony("com.lekillerdesgames.hehmod");
            _harmony.PatchAll();
        }

        public static Sprite CreateSprite(string name)
        {
            var pixelsPerUnit = 100f;
            var pivot = new Vector2(0.5f, 0.5f);

            var assembly = Assembly.GetExecutingAssembly();
            var tex = CreateEmptyTexture();
            var imageStream = assembly.GetManifestResourceStream(name);
            var img = imageStream.ReadFully();
            LoadImage(tex, img, true);
            tex.DontDestroy();
            var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, pixelsPerUnit);
            sprite.DontDestroy();
            return sprite;
        }

        public static Texture2D CreateEmptyTexture(int width = 0, int height = 0)
        {
            return new Texture2D(width, height, TextureFormat.RGBA32, Texture.GenerateAllMips, false, IntPtr.Zero);
        }

        public static void LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            _iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2CPPArray = (Il2CppStructArray<byte>) data;
            _iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
        }

        private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    }
}
