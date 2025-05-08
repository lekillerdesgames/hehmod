using HarmonyLib;

namespace hehmod.Patches
{
    [HarmonyPriority(Priority.VeryLow)]
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTracker_Update
    {
        public static void Postfix(PingTracker __instance)
        {
            var position = __instance.GetComponent<AspectPosition>();
            position.Alignment = AspectPosition.EdgeAlignments.Top;
            position.DistanceFromEdge = new(0f, 0.1f, 0);
            position.AdjustPosition();
            __instance.text.text += $"\nhehmod v{hehmod.VersionString} by <color=#FF0000>le killer</color>";
        }
    }
}
