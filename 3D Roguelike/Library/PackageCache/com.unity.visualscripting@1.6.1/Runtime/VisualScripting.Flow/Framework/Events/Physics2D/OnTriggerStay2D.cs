using System;

namespace Unity.VisualScripting
{
    /// <summary>
    /// Called once per frame for every collider that is touching the trigger.
    /// </summary>
    public sealed class OnTriggerStay2D : TriggerEvent2DUnit
    {
        public override Type MessageListenerType => typeof(UnityOnTriggerStay2DMessageListener);
        protected override string hookName => EventHooks.OnTriggerStay2D;
    }
}
