using System.IO;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace hehmod
{
    public static class Extensions
    {
        public static byte[] ReadFully(this Stream input)
        {
            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        public static T DontDestroy<T>(this T obj) where T : UnityObject
        {
            obj.hideFlags |= HideFlags.HideAndDontSave;

            return obj.DontDestroyOnLoad();
        }

        public static T DontDestroyOnLoad<T>(this T obj) where T : UnityObject
        {
            UnityObject.DontDestroyOnLoad(obj);

            return obj;
        }

        public static TMPro.TextMeshPro nameText(this PlayerControl p) => p?.cosmetics?.nameText;
    }
}