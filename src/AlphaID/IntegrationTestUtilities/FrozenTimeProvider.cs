namespace IntegrationTestUtilities;

/// <summary>
/// 提供冻结的时间以便于测试。
/// </summary>
public class FrozenTimeProvider : TimeProvider
{
    private readonly DateTimeOffset time;

    /// <summary>
    /// 通过指定的时间创建一个冻结的TimeProvider.
    /// </summary>
    /// <param name="time"></param>
    public FrozenTimeProvider(DateTimeOffset time)
    {
        this.time = time;
    }

    /// <summary>
    /// 从当前UTC时间创建一个冻结时间。该时间表明创建TimeProvider时的时间，并且在接下来的声明周期里不再发生变化。
    /// </summary>
    public FrozenTimeProvider()
        : this(DateTimeOffset.UtcNow)
    { }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override DateTimeOffset GetUtcNow()
    {
        return this.time.ToUniversalTime();
    }
}
