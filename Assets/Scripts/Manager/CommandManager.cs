using System;
using System.Collections.Generic;

/// <summary>
/// Manages the execution of commands by providing a flexible command invocation.
/// </summary>
public class CommandManager
{
    public delegate void InvokeCommandDelegate<TCommand>(TCommand e) where TCommand : ICommand;

    private delegate void InvokeCommandDelegate(ICommand e);
    private readonly Dictionary<System.Type, Delegate> _invokeCommandDelegates = new Dictionary<System.Type, Delegate>();
        
    public void InvokeCommand(ICommand command)
    {
        if (_invokeCommandDelegates.TryGetValue(command.GetType(), out var del))
        {
            del.DynamicInvoke(command);
        }
    }

    public void AddCommandListener<TCommand>(InvokeCommandDelegate<TCommand> invokeDelegate) where TCommand : ICommand
    {
        AddCommandListenerImpl(invokeDelegate);
    }
        
    private void AddCommandListenerImpl<TCommand>(InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
    {
        if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out var tempDel))
        {
            _invokeCommandDelegates[typeof(TCommand)] = Delegate.Combine(tempDel, del);
        }
        else
        {
            _invokeCommandDelegates[typeof(TCommand)] = del;
        }
    }
    
    public void RemoveCommandListener<TCommand>(InvokeCommandDelegate<TCommand> del) where TCommand : ICommand
    {
        if (_invokeCommandDelegates.TryGetValue(typeof(TCommand), out var tempDel))
        {
            tempDel = Delegate.Remove(tempDel, del);
            if (tempDel == null)
            {
                _invokeCommandDelegates.Remove(typeof(TCommand));
            }
            else
            {
                _invokeCommandDelegates[typeof(TCommand)] = tempDel;
            }
        }
    }
}