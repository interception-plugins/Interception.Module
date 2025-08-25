using System;

using SDG.Unturned;

namespace interception.utils {
    public static class structure_util {
        public static void do_for_each_structure(Action<StructureDrop> callback) {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++) {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++) {
                    if (Regions.checkSafe(x, y)) {
                        StructureRegion region = StructureManager.regions[x, y];
                        for (int i = 0; i < region.drops.Count; i++) {
                            callback(region.drops[i]);
                        }
                    }
                }
            }
        }
    }
}
