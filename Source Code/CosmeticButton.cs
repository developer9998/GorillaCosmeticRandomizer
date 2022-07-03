using System;
using UnityEngine;
using System.Collections;

namespace GorillaCosmeticRandomizer
{
    public class CosmeticButton : GorillaPressableButton
    {
        Material pressedMat = Resources.Load<Material>("objects/treeroom/materials/pressed");
        Material unpressedMat = Resources.Load<Material>("objects/treeroom/materials/plastic");
        public override void Start()
        {
            base.Start();
            gameObject.GetComponent<Renderer>().material = unpressedMat;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.layer = 18;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
            StartCoroutine(ButtonPress());
        }

        IEnumerator ButtonPress()
        {
            gameObject.GetComponent<Renderer>().material = pressedMat;
            Plugin.instance.Generate();
            yield return new WaitForSeconds(debounceTime);
            gameObject.GetComponent<Renderer>().material = unpressedMat;
            yield break;
        }
    }
}
