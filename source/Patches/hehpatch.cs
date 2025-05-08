using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace hehmod.Patches
{
    [HarmonyPatch]
    public class hehPatch
    {
        public static Dictionary<byte, GameObject> Hehs = new Dictionary<byte, GameObject>();
        public static float OriginalFlipX = 0f;

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static void Postfix()
        {
            if (!PlayerControl.LocalPlayer || PlayerControl.AllPlayerControls.Count <= 0)
            {
                while (Hehs.Keys.Any())
                {
                    var heh1 = Hehs.FirstOrDefault();
                    if (heh1.Value != null) Object.Destroy(heh1.Value.gameObject);
                    Hehs.Remove(heh1.Key);
                }
                return;
            }
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.Data.Disconnected)
                {
                    if (Hehs.TryGetValue(player.PlayerId, out GameObject hehObject2))
                    {
                        if (hehObject2 != null) Object.Destroy(hehObject2);
                        Hehs.Remove(player.PlayerId);
                    }
                    continue;
                }
                if (player == null || player.Data == null) continue;
                if (!Hehs.ContainsKey(player.PlayerId))
                {
                    var heh = new GameObject($"HEH{player.PlayerId}");
                    SpriteRenderer render = heh.AddComponent<SpriteRenderer>();
                    render.sprite = hehmod.heh;
                    Hehs.Add(player.PlayerId, heh);
                    if (OriginalFlipX == 0f) OriginalFlipX = heh.transform.localScale.x;
                }
                Hehs.TryGetValue(player.PlayerId, out GameObject hehObject);
                var position = player.transform.localPosition;
                hehObject.transform.localPosition = new Vector3(position.x, position.y, -1f);
                var scale = hehObject.transform.localScale;
                bool flipped = player.cosmetics.currentBodySprite.BodySprite.flipX;
                if (flipped) hehObject.transform.localScale = new Vector3(OriginalFlipX * -1, scale.y, scale.z);
                else hehObject.transform.localScale = new Vector3(OriginalFlipX * 1, scale.y, scale.z);

                hehObject.SetActive(!player.Data.Disconnected && (!player.Data.IsDead || PlayerControl.LocalPlayer.Data.IsDead) &&
                player.nameText().color != Color.clear && !player.inVent);
            }
            foreach (var key in Hehs.Keys)
            {
                if (key > PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.Disconnected).ToList().Count - 1)
                {
                    Hehs.TryGetValue(key, out GameObject hehObject);
                    if (hehObject != null) Object.Destroy(hehObject);
                    Hehs.Remove(key);
                }
            }
        }
    }
}