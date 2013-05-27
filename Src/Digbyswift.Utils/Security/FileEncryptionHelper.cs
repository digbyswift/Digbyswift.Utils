using System.Security.Cryptography;
using System.IO;

namespace Digbyswift.Utils.Security
{

	public class FileEncryptionHelper
	{

		#region Fields

		/// <summary>
		/// Tag to make sure this file is readable/decryptable by this class
		/// </summary>
		private const ulong FC_TAG = 0xFC010203040506CF;

		/// <summary>
		/// The amount of bytes to read from the file
		/// </summary>
		private const int BUFFER_SIZE = 128*1024;

		/// <summary>
		/// Crypto Random number generator for use in EncryptFile
		/// </summary>
		private readonly static RandomNumberGenerator Rand = new RNGCryptoServiceProvider();

		#endregion

		/// <summary>
		/// CallBack delegate for progress notification
		/// </summary>
		public delegate void CryptoProgressCallBack(int min, int max, int value);

		#region Methods: Public

		/// <summary>
		/// This takes an input file and encrypts it into the output file
		/// </summary>
		/// <param name="inputFilePath">the file to encrypt</param>
		/// <param name="outputFilePath">the file to write the encrypted data to</param>
		/// <param name="password">the password for use as the key</param>
		public static void EncryptFile(string inputFilePath, string outputFilePath, string password)
		{
			EncryptFile(inputFilePath, outputFilePath, password, null);
		}

		/// <summary>
		/// This takes an input file and encrypts it into the output file
		/// </summary>
		/// <param name="inputFilePath">the file to encrypt</param>
		/// <param name="outputFilePath">the file to write the encrypted data to</param>
		/// <param name="password">the password for use as the key</param>
		/// <param name="callback">the method to call to notify of progress</param>
		public static void EncryptFile(string inputFilePath, string outputFilePath, string password, CryptoProgressCallBack callback)
		{
			DoEncryption(inputFilePath, outputFilePath, password, callback);
		}

		/// <summary>
		/// takes an input file and decrypts it to the output file
		/// </summary>
		/// <param name="inputFilePath">the file to decrypt</param>
		/// <param name="outputFilePath">the to write the decrypted data to</param>
		/// <param name="password">the password used as the key</param>
		public static DecryptionResult TryDecryptFile(string inputFilePath, string outputFilePath, string password)
		{
			return TryDecryptFile(inputFilePath, outputFilePath, password, null);
		}

		/// <summary>
		/// takes an input file and decrypts it to the output file
		/// </summary>
		/// <param name="inputFilePath">the file to decrypt</param>
		/// <param name="outputFilePath">the to write the decrypted data to</param>
		/// <param name="password">the password used as the key</param>
		/// <param name="callback">the method to call to notify of progress</param>
		public static DecryptionResult TryDecryptFile(string inputFilePath, string outputFilePath, string password, CryptoProgressCallBack callback)
		{
			using (FileStream inputStream = File.OpenRead(inputFilePath))
			{
				int orignalInputSize = (int)inputStream.Length;
				byte[] bytes = new byte[BUFFER_SIZE];
				int numOutputBytesProcessed = 0;

				// read off the IV and Salt
				byte[] iv = new byte[16];
				inputStream.Read(iv, 0, 16);
				byte[] salt = new byte[16];
				inputStream.Read(salt, 0, 16);

				// create the crypting stream
				SymmetricAlgorithm sma = CreateRijndael(password, salt);
				sma.IV = iv;

				int numTotalBytesProcessed = 32;
				long encryptedStreamLength; // the size stored in the input stream

				// create the cryptostreams that will process the file
				using (CryptoStream inputCryptoStream = new CryptoStream(inputStream, sma.CreateDecryptor(), CryptoStreamMode.Read))
				{
					using (var bReader = new BinaryReader(inputCryptoStream))
					{
						encryptedStreamLength = bReader.ReadInt64();
						ulong tag = bReader.ReadUInt64();

						if (FC_TAG != tag)
							return DecryptionResult.FailedFileCorrupted;

						// Create the hash algorithm object. This will be generated as we output the encrypted
						// file and it can then be compared against the original has appended to the file
						using (HashAlgorithm workingHashAlgorithm = SHA256.Create())
						{
							using (
								CryptoStream hashCryptoStream = new CryptoStream(Stream.Null, workingHashAlgorithm, CryptoStreamMode.Write))
							{
								using (FileStream outputStream = File.OpenWrite(outputFilePath))
								{
									#region Decrypt and output

									// Determine number of reads required to loop through the input stream
									long numReads = encryptedStreamLength/BUFFER_SIZE;
									for (int i = 0; i < numReads; ++i)
									{
										int numBytesRead = inputCryptoStream.Read(bytes, 0, bytes.Length);

										outputStream.Write(bytes, 0, numBytesRead);
										hashCryptoStream.Write(bytes, 0, numBytesRead);

										numTotalBytesProcessed += numBytesRead;
										numOutputBytesProcessed += numBytesRead;

										if (callback != null)
											callback(0, orignalInputSize, numTotalBytesProcessed);
									}

									// Calculate remaining bytes of stream
									long remainingBytes = encryptedStreamLength % BUFFER_SIZE;
									if (remainingBytes > 0)
									{
										int numBytesRead = inputCryptoStream.Read(bytes, 0, (int) remainingBytes);

										outputStream.Write(bytes, 0, numBytesRead);
										hashCryptoStream.Write(bytes, 0, numBytesRead);

										numTotalBytesProcessed += numBytesRead;
										numOutputBytesProcessed += numBytesRead;

										if (callback != null)
											callback(0, orignalInputSize, numTotalBytesProcessed);
									}

									#endregion
								}
							}

							byte[] hashGeneratedFromDecryption = workingHashAlgorithm.Hash;

							// Get and compare the current and old hash values
							byte[] hashExtractedFromInputFile = new byte[workingHashAlgorithm.HashSize/8];
							int hashLength = inputCryptoStream.Read(hashExtractedFromInputFile, 0, hashExtractedFromInputFile.Length);

							if ((hashExtractedFromInputFile.Length != hashLength) ||
							    (!CheckByteArrays(hashExtractedFromInputFile, hashGeneratedFromDecryption)))
								return DecryptionResult.FailedHashesDoNotMatch;
						}
					}
				}

				if (numOutputBytesProcessed != encryptedStreamLength)
					return DecryptionResult.FailedFileSizesDoNotMatch;
			}

			return DecryptionResult.Success;
		}

		#endregion

		#region Methods: Private

		private static void DoEncryption(string inputFilePath, string outputFilePath, string password, CryptoProgressCallBack callback)
		{
			using (FileStream inputStream = File.OpenRead(inputFilePath), outputStream = File.OpenWrite(outputFilePath))
			{
				long inputSize = inputStream.Length;
				int workingSize = (int)inputSize;
				byte[] bytes = new byte[BUFFER_SIZE];
				int processedSize = 0;

				// Create salt and IV
				byte[] iv = GenerateRandomBytes(16);
				byte[] salt = GenerateRandomBytes(16);

				// Write the IV and salt values to the beginning of the output stream
				outputStream.Write(iv, 0, iv.Length);
				outputStream.Write(salt, 0, salt.Length);

				// Create the crypting object
				SymmetricAlgorithm sma = CreateRijndael(password, salt);
				sma.IV = iv;

				// Create the hashing and crypto streams
				HashAlgorithm hashAlgorithm = SHA256.Create();

				using (CryptoStream outputCryptoStream = new CryptoStream(outputStream, sma.CreateEncryptor(), CryptoStreamMode.Write))
				{
					// Write the size of the file and the
					// crypto tag to the output file
					using (BinaryWriter bWriter = new BinaryWriter(outputCryptoStream))
					{
						bWriter.Write(inputSize);
						bWriter.Write(FC_TAG);

						using (CryptoStream hashCryptoStream = new CryptoStream(Stream.Null, hashAlgorithm, CryptoStreamMode.Write))
						{
							int bytesRead;
							while ((bytesRead = inputStream.Read(bytes, 0, bytes.Length)) != 0)
							{
								outputCryptoStream.Write(bytes, 0, bytesRead);
								hashCryptoStream.Write(bytes, 0, bytesRead);
								processedSize += bytesRead;

								if (callback != null)
									callback(0, workingSize, processedSize);
							}
						}

						// read the hash
						byte[] hash = hashAlgorithm.Hash;

						// write the hash to the end of the file
						outputCryptoStream.Write(hash, 0, hash.Length);
					}

				}
			}
		}

		/// <summary>
		/// Checks to see if two byte array are equal
		/// </summary>
		/// <param name="b1">the first byte array</param>
		/// <param name="b2">the second byte array</param>
		/// <returns>true if b1.Length == b2.Length and each byte in b1 is
		/// equal to the corresponding byte in b2</returns>
		private static bool CheckByteArrays(byte[] b1, byte[] b2)
		{
			if (b1.Length != b2.Length)
				return false;

			for (int i = 0; i < b1.Length; ++i)
			{
				if (b1[i] != b2[i])
					return false;
			}
			return true;
		}

		/// <summary>
		/// Creates a Rijndael SymmetricAlgorithm for use in EncryptFile and DecryptFile
		/// </summary>
		/// <param name="password">the string to use as the password</param>
		/// <param name="salt">the salt to use with the password</param>
		/// <returns>A SymmetricAlgorithm for encrypting/decrypting with Rijndael</returns>
		private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
		{
			var derivedBytes = new Rfc2898DeriveBytes(password, salt, 1000);

			return new AesManaged { KeySize = 256, Key = derivedBytes.GetBytes(32) };
		}

		/// <summary>
		/// Generates a specified amount of random bytes
		/// </summary>
		/// <param name="count">the number of bytes to return</param>
		/// <returns>a byte array of count size filled with random bytes</returns>
		private static byte[] GenerateRandomBytes(int count)
		{
			byte[] bytes = new byte[count];
			Rand.GetBytes(bytes);
			return bytes;
		}

		#endregion

	}

}
