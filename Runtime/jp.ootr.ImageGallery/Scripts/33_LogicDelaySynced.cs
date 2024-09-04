using System;
using jp.ootr.common;
using UdonSharp;
using VRC.SDKBase;

namespace jp.ootr.ImageGallery
{
    public class LogicDelaySynced : LogicDelayLocal
    {
        [UdonSynced] private int _delaySyncedIndex;

        protected void InitDelaySynced()
        {
            if (!Networking.IsOwner(gameObject)) return;
            TryCastImageDelaySynced();
        }

        public void TryCastImageDelaySynced()
        {
            var currentTime = DateTime.Now.ToUnixTime();
            if (LastCastTime != targetScreen.lastImageUpdated)
            {
                if (restoreMode == RestoreMode.None) return;
                if (restoreMode == RestoreMode.Timeout)
                {
                    var diff = (int)(currentTime - targetScreen.lastImageUpdated);
                    if (diff < restoreTimeout)
                        SendCustomEventDelayedSeconds(nameof(TryCastImageDelaySynced), restoreTimeout - diff + 0.1f);

                    return;
                }
            }

            _delaySyncedIndex = (_delaySyncedIndex + 1) % Urls.Length;
            Sync();
            targetScreen.LoadImage(Urls[_delaySyncedIndex], FileNames[_delaySyncedIndex]);
        }

        public override void _OnDeserialization()
        {
            base._OnDeserialization();
            if (syncMode == SyncMode.DelaySynced)
                SendCustomEventDelayedSeconds(nameof(TryCastImageDelaySynced), seekInterval);
        }
    }
}
