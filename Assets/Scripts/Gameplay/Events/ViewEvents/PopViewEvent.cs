namespace Game.Event
{
    public struct PopViewEvent
    {
        public readonly ViewDefine.ViewName viewName;

        public PopViewEvent(ViewDefine.ViewName viewName)
        {
            this.viewName = viewName;
        }
    }
}
