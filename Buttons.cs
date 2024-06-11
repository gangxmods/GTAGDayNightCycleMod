using System.Collections;
using UnityEngine;

namespace DaytimeAndNightTimeChanger
{
    public class SunButton : GorillaPressableButton
    {
        public static bool CD = false;
        private void OnTriggerEnter(Collider collider)
        {
            if (!CD)
            {
                StartCoroutine(Press());
            }
        }

        private IEnumerator Press()
        {
            CD = true;
            Plugin.instance.ChangeToDay();
            yield return new WaitForSeconds(0.25f);
            CD = false;
        }
    }

    public class MoonButton : GorillaPressableButton
    {

        public static bool CD = false;
        private void OnTriggerEnter(Collider collider)
        {
            if (!CD)
            {
                StartCoroutine(Press());
            }
        }

        private IEnumerator Press()
        {
            CD = true;
            Plugin.instance.ChangeToNight();
            yield return new WaitForSeconds(0.25f);
            CD = false;
        }
    }

    public class SunsetButton : GorillaPressableButton
    {
        public static bool CD = false;
        private void OnTriggerEnter(Collider collider)
        {
            if (!CD)
            {
                StartCoroutine(Press());
            }
        }

        private IEnumerator Press()
        {
            CD = true;
            Plugin.instance.ChangeToSunset();
            yield return new WaitForSeconds(0.25f);
            CD = false;
        }
    }
}
