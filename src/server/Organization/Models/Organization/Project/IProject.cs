﻿using Organization.Models.Organization.Accounts;

namespace Organization.Models.Organization.Project;

public interface IProject : IUserAggregator
{
    ICollection<IAccount> Members { get; init; }
}