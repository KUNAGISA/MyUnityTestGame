using Framework;

namespace Game
{
    public readonly struct TestCommand : ICommand
    {
        void ICommand.Execute(ICommandOperate operate)
        {
            operate.SendEvent<Event.WorldRunChangeEvent>();
        }
    }
}
