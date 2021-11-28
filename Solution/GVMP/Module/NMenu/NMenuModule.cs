using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVMP
{
    class NMenuModule : GVMP.Module.Module<NMenuModule>
    {
        [RemoteEvent("REQUEST_ANIMATION_USE")]
        public void REQUEST_ANIMATION_USE(Client c, int id)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                if (dbPlayer.DeathData.IsDead) return;

                if (id == 0)
                {
                    dbPlayer.StopAnimation();
                }
                else
                {
                    Animation animation = Animations.animations.FirstOrDefault((Animation anim) => anim.Slot == id);

                    if (animation == null)
                        return;

                    string dict = animation.Select.Split("|")[0];
                    string anim = animation.Select.Split("|")[1];

                    dbPlayer.PlayAnimation(animation.Flag, dict, anim);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_ANIMATION_USE] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_ANIMATION_USE] " + ex.StackTrace);
            }
        }
    }
}
