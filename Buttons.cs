using System.Collections;
using UnityEngine;

namespace DaytimeAndNightTimeChanger
{
    public class SunButton : GorillaPressableButton
    {
        private void OnTriggerEnter(Collider collider)
        {
            StartCoroutine(Press());
        }

        private IEnumerator Press()
        {
            Plugin.instance.ChangeToDay();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public class MoonButton : GorillaPressableButton
    {
        private void OnTriggerEnter(Collider collider)
        {
            StartCoroutine(Press());
        }

        private IEnumerator Press()
        {
            Plugin.instance.ChangeToNight();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
