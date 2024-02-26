namespace Models.Scheduling;

public record Job
{
    public Job(int id, int duration, int deadline)
    {
        Id = id;
        Duration = duration;
        Deadline = deadline;
    }

    public int Id { get; init; }

    public int Duration { get; init; }

    public int Deadline { get; init; }

    public int Weight { get; init; }
}
