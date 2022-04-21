using System.Security.Cryptography;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

/// <summary>
///   Gets the current DateTime.UtcNow.Ticks and adds
///   an offset so we never get the same value twice
///   If the resulting ticks are before UtcNow.Ticks
///   then we jump the values forward so we don't return a
///   value that would cause inserts to the middle of the table.
///   In a single process this would ensure
///   the value is always added to the end of the table.
/// </summary>
internal static class LongGenerator
{
	private static long LastValueUsed = DateTime.UtcNow.Ticks;

	public static long Next()
	{
		long result;
		long ticksNow = DateTime.UtcNow.Ticks;
		do
		{
			result = Interlocked.Increment(ref LastValueUsed);
			if (result >= ticksNow)
				return result;
		} while (Interlocked.CompareExchange(ref LastValueUsed, ticksNow, result) != result);
		return result;
	}
}

/// <summary>
///   Creates a one-off 8 byte crypto secure random
///   number as the process Id when the app starts
///   and then appends the time based sequence number
///   as the next 8 bytes to give us a sequential
///   value this is statistically globally unique.
/// </summary>
internal static class SequentialGuidGenerator
{
	private static byte[] ProcessId;

	static SequentialGuidGenerator()
	{
		ProcessId = RandomNumberGenerator.GetBytes(8);
	}

	public static Guid Next()
	{
		byte[] result = new byte[16];
		byte[] sequentialTimeStamp =
	 BitConverter.GetBytes(LongGenerator.Next());
		if (!BitConverter.IsLittleEndian)
			Array.Reverse(sequentialTimeStamp);

		result[0] = ProcessId[0];
		result[1] = ProcessId[1];
		result[2] = ProcessId[2];
		result[3] = ProcessId[3];
		result[4] = ProcessId[4];
		result[5] = ProcessId[5];
		result[6] = ProcessId[6];
		result[7] = ProcessId[7];
		result[8] = sequentialTimeStamp[1];
		result[9] = sequentialTimeStamp[0];
		result[10] = sequentialTimeStamp[7];
		result[11] = sequentialTimeStamp[6];
		result[12] = sequentialTimeStamp[5];
		result[13] = sequentialTimeStamp[4];
		result[14] = sequentialTimeStamp[3];
		result[15] = sequentialTimeStamp[2];
		return new Guid(result);
	}
}