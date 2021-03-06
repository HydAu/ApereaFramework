﻿using System;
using System.Linq;
using Aperea.Commands;
using Aperea.Infrastructure.IoC;
using Machine.Fakes;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace Aperea.Common.Specs.Commands
{
    public class BehaviorDispatchers
    {
        OnEstablish context = accessor =>
        {
             container = new Container();
            container.Configure(c => c.AddRegistry<CommonCommandRegistry>());

            accessor.Configure<IServiceLocator>(new StructureMapServiceLocator(container));

            accessor.Configure<ICommandDispatcher>(new CommandDispatcher(accessor.The<IServiceLocator>()));
            accessor.Configure<IQueryDispatcher>(new QueryDispatcher(accessor.The<IServiceLocator>()));
        };

        OnCleanup cleanup = subject =>
        {
            //container.ResetDefaults();
            //container.Model.
        };

        static Container container;
    }
}