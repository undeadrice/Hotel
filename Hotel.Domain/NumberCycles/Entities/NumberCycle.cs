using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Exceptions;

namespace Hotel.Domain.NumberCycles.Entities;

public class NumberCycle
{
    public Guid Id { get; private set; }
    public NumberCycleTopic Topic { get; private set; }
    public string Prefix { get; private set; }
    public int StartIndex { get; private set; }
    public int CurrentIndex { get; private set; }
    public DateTime CreatedAt { get; private set; }

#pragma warning disable CS8618
    public NumberCycle() { }
#pragma warning restore CS8618

    private NumberCycle(
        Guid id,
        NumberCycleTopic topic,
        string prefix,
        int startIndex,
        DateTime createdAt)
    {
        Id = id;
        Topic = topic;
        Prefix = prefix;
        StartIndex = startIndex;
        CurrentIndex = startIndex;
        CreatedAt = createdAt;
    }

    public static NumberCycle Create(
        NumberCycleTopic topic,
        string prefix,
        int startIndex)
    {
        Validate(topic, prefix, startIndex);

        return new NumberCycle(
            Guid.NewGuid(),
            topic,
            prefix.Trim().ToUpperInvariant(),
            startIndex,
            DateTime.UtcNow);
    }

    public string NextIdentifier()
    {
        var identifier = $"{Prefix}-{CurrentIndex}";
        CurrentIndex++;
        return identifier;
    }

    private static void Validate(NumberCycleTopic topic, string prefix, int startIndex)
    {
        if (!Enum.IsDefined(topic))
        {
            throw new NumberCycleInvalidTopicException();
        }

        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new NumberCyclePrefixRequiredException();
        }

        if (startIndex < 0)
        {
            throw new NumberCycleStartIndexInvalidException();
        }
    }
}