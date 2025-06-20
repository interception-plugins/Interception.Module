using System;

using SDG.Unturned;

namespace interception.extensions {
    public static class InteractableVehicle_ex {
        public static bool have_passengers(this InteractableVehicle v) {
            if (v.passengers != null && v.passengers.Length > 0) {
                for (int i = 0; i < v.passengers.Length; i++)
                    if (v.passengers[i] != null && v.passengers[i].player != null) return true;
            }
            if (v.turrets != null && v.turrets.Length > 0) {
                for (int i = 0; i < v.turrets.Length; i++)
                    if (v.turrets[i] != null && v.turrets[i].player != null) return true;
            }
            return false;
        }

        public static int count_passengers(this InteractableVehicle v) {
            int result = 0;
            if (v.passengers != null && v.passengers.Length > 0) {
                for (int i = 0; i < v.passengers.Length; i++)
                    if (v.passengers[i] != null && v.passengers[i].player != null) result++;
            }
            if (v.turrets != null && v.turrets.Length > 0) {
                for (int i = 0; i < v.turrets.Length; i++)
                    if (v.turrets[i] != null && v.turrets[i].player != null) result++;
            }
            return result;
        }
    }
}
