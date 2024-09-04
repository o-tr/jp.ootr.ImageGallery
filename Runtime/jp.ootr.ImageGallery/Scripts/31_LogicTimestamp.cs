using System;
using jp.ootr.common;

namespace jp.ootr.ImageGallery
{
    public class LogicTimestamp : LogicInit
    {
        protected void InitTimestamp()
        {
            TryCastImageTimestamp();
        }

        public void TryCastImageTimestamp()
        {
            var currentTime = DateTime.Now.ToUnixTime();
            if (LastCastTime != targetScreen.lastImageUpdated)
            {
                if (restoreMode == RestoreMode.None) return;
                if (restoreMode == RestoreMode.Timeout)
                {
                    var diff = (int)(currentTime - targetScreen.lastImageUpdated);
                    if (diff < restoreTimeout)
                        SendCustomEventDelayedSeconds(nameof(TryCastImageTimestamp), restoreTimeout - diff + 0.1f);
                    return;
                }
            }

            var index = (int)(currentTime / (ulong)seekInterval) % Urls.Length;

            targetScreen.LoadImage(Urls[index], FileNames[index]);
            LastCastTime = targetScreen.lastImageUpdated;
            SendCustomEventDelayedSeconds(nameof(TryCastImageTimestamp), seekInterval);
        }
    }
}
