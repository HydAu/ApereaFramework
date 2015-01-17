﻿using System;

namespace Aperea.CQRS.EventStore.Storage
{
    public interface IIdentityMap<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IEventProvider<TDomainEvent>, new();
        void Add<TAggregate>(TAggregate aggregateRoot) where TAggregate : class, IEventProvider<TDomainEvent>, new();
        void Remove(Type aggregateRootType, Guid aggregateRootId);
    }
}