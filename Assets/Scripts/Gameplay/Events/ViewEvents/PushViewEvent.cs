namespace Game.Event
{
    public struct PushViewEvent
    {
        public readonly ViewDefine.ViewName viewName;

        public PushViewEvent(ViewDefine.ViewName viewName)
        {
            this.viewName = viewName;
        }
    }
}