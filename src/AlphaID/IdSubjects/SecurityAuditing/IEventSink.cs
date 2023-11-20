﻿using IdSubjects.SecurityAuditing.Events;

namespace IdSubjects.SecurityAuditing;

/// <summary>
/// Models persistence of events
/// </summary>
public interface IEventSink
{
    /// <summary>
    /// Raises the specified event.
    /// </summary>
    /// <param name="evt">The event.</param>
    Task PersistAsync(AuditLogEvent evt);
}
