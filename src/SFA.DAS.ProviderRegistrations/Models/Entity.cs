﻿using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRegistrations.Models;

public abstract class Entity
{
    protected void Publish<T>(Func<T> action) where T : class
    {
        UnitOfWorkContext.AddEvent(action);
    }
}