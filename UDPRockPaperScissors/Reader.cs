namespace UDPRockPaperScissors;

internal static class Reader {
    private static readonly AutoResetEvent GetInput;
    private static readonly AutoResetEvent GotInput;
    private static string? _input;

    static Reader() {
        GetInput = new AutoResetEvent(false);
        GotInput = new AutoResetEvent(false);
        var inputThread = new Thread(Read)
        {
            IsBackground = true
        };
        inputThread.Start();
    }

    private static void Read() {
        while (true) {
            GetInput.WaitOne();
            _input = Console.ReadLine();
            GotInput.Set();
        }
    }

    // omit the parameter to read a line without a timeout
    public static string? ReadLine(int timeOutMillisecs = Timeout.Infinite) {
        GetInput.Set();
        var success = GotInput.WaitOne(timeOutMillisecs);
        if (success)
            return _input;
        else
            throw new TimeoutException("User did not provide input within the time-limit.");
    }
}