namespace Digbyswift.Utils.Security
{
	public enum DecryptionResult
	{
		Unknown = 0,
		Success = 1,
		FailedFileCorrupted = 2,
		FailedHashesDoNotMatch = 3,
		FailedFileSizesDoNotMatch = 4
	}
}