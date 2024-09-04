using System;
using jp.ootr.common;

namespace jp.ootr.ImageGallery
{
    public class LogicDelayLocal : LogicTimestamp
    {
        private int _delayLocalIndex;

        protected void InitDelayLocal()
        {
            TryCastImageDelayLocal();
        }

        public void TryCastImageDelayLocal()
        {
            var currentTime = DateTime.Now.ToUnixTime();
            if (LastCastTime != targetScreen.lastImageUpdated)
            {
                if (restoreMode == RestoreMode.None) return;
                if (restoreMode == RestoreMode.Timeout)
                {
                    var diff = (int)(currentTime - targetScreen.lastImageUpdated);
                    if (diff < restoreTimeout)
                        SendCustomEventDelayedSeconds(nameof(TryCastImageDelayLocal), restoreTimeout - diff + 0.1f);
                    return;
                }
            }

            _delayLocalIndex = (_delayLocalIndex + 1) % Urls.Length;

            targetScreen.LoadImage(Urls[_delayLocalIndex], FileNames[_delayLocalIndex]);
            LastCastTime = targetScreen.lastImageUpdated;
            SendCustomEventDelayedSeconds(nameof(TryCastImageDelayLocal), seekInterval);
        }
    }
}
