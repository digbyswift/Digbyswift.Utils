using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Digbyswift.Utils.Security
{
	/// <summary>
	/// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and 
	/// decrypt data. As long as encryption and decryption routines use the same
	/// parameters to generate the keys, the keys are guaranteed to be the same.
	/// The class uses static functions with duplicate code to make it easier to
	/// demonstrate encryption and decryption logic. In a real-life application, 
	/// this may not be the most efficient way of handling encryption, so - as
	/// soon as you feel comfortable with it - you may want to redesign this class.
	/// </summary>
	public class AES
	{

		public static string Encrypt(string plainText, string salt)
		{
			if (String.IsNullOrEmpty(plainText))
				return null;

            string passPhrase = "WEY6hwVNsfLdGAWbbmvmtoAWTr2Muu";        // can be any string
			//string hashAlgorithm = "SHA1";             // can be "MD5"
			string initVector = "1B2c$d4e5F6g7H8"; // must be 16 bytes

			return Encrypt(plainText, passPhrase, salt, 1, initVector, 256);
		}

		/// <summary>
		/// Encrypts specified plaintext using Rijndael symmetric key algorithm
		/// and returns a base64-encoded result.
		/// </summary>
		/// <param name="plainText">
		/// Plaintext value to be encrypted.
		/// </param>
		/// <param name="passPhrase">
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </param>
		/// <param name="saltValue">
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </param>
		/*/// <param name="hashAlgorithm">
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </param>*/
		/// <param name="passwordIterations">
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </param>
		/// <param name="initVector">
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be 
		/// exactly 16 ASCII characters long.
		/// </param>
		/// <param name="keySize">
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
		/// Longer keys are more secure than shorter keys.
		/// </param>
		/// <returns>
		/// Encrypted value formatted as a base64-encoded string.
		/// </returns>
		internal static string Encrypt(string plainText,
									 string passPhrase,
									 string saltValue,
									 //string hashAlgorithm,
									 int passwordIterations,
									 string initVector,
									 int keySize)
		{
			// Convert strings into byte arrays.
			// Let us assume that strings only contain ASCII codes.
			// If strings include Unicode characters, use Unicode, UTF7, or UTF8 
			// encoding.
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

			// Convert our plaintext into a byte array.
			// Let us assume that plaintext contains UTF8-encoded characters.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			// First, we must create a password, from which the key will be derived.
			// This password will be generated from the specified passphrase and 
			// salt value. The password will be created using the specified hash 
			// algorithm. Password creation can be done in several iterations.
			var password = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);

			// Use the password to generate pseudo-random bytes for the encryption
			// key. Specify the size of the key in bytes (instead of bits).
			byte[] keyBytes = password.GetBytes(keySize / 8);

			// Create uninitialized Rijndael encryption object.
			// It is reasonable to set encryption mode to Cipher Block Chaining
			// (CBC). Use default options for other symmetric key parameters.
			RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };

			// Generate encryptor from the existing key bytes and initialization 
			// vector. Key size will be defined based on the number of the key 
			// bytes.
			// Define memory stream which will be used to hold encrypted data.
			// Define cryptographic stream (always use Write mode for encryption).
			using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
			using (var memoryStream = new MemoryStream())
			using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
			{
				// Start encrypting.
				cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

				// Finish encrypting.
				cryptoStream.FlushFinalBlock();

				// Convert our encrypted data from a memory stream into a byte array.
				byte[] cipherTextBytes = memoryStream.ToArray();

				// Convert encrypted data into a base64-encoded string.
				string cipherText = Convert.ToBase64String(cipherTextBytes);

				// Return encrypted string.
				return cipherText;
			}

			
		}

	}
}