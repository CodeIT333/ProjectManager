﻿namespace Domain.Commons.Models
{
    public abstract class AggregateRoot<TId> : Entity<TId> 
        where TId : notnull
    {

    }
}
