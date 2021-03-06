﻿namespace Aperea.Commands
{
    public interface ICommandHandler
    {
        void Execute(ICommand command);
    }
    
    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand
    {
        void Execute(T command);
    }

    public interface ICommandHandler<in T, out TResult> : ICommandHandler<T> where T : ICommand<TResult>
    {
        new TResult Execute(T command);
    }
}