// Author(s): Paul Calande
// Timer class to be used for simulating periodic time-based behavior.

public class Timer
{
    // How many seconds it takes for the timer to run out of time.
    float secondsTarget;

    // The current number of seconds passed in this period.
    float secondsCurrent = 0.0f;

    // Constructor.
    public Timer(float seconds)
    {
        secondsTarget = seconds;
    }
    public Timer()
    {
        secondsTarget = 1.0f;
    }

    // Check if the time has run out, and if not, increment the time passed by deltaTime.
    public bool TimeUp(float deltaTime)
    {
        if (secondsCurrent >= secondsTarget)
        {
            secondsCurrent -= secondsTarget;
            return true;
        }
        secondsCurrent += deltaTime;
        return false;
    }

    public void SetTargetTime(float seconds)
    {
        secondsTarget = seconds;
    }

    // Reset the timer.
    public void Reset()
    {
        secondsCurrent = 0.0f;
    }
}