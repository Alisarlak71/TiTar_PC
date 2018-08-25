using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace MaterialDesign2.Classes
{
    public class Encryption
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
    public class registerysetting
    {
        public string encrypted_password;
        public string decrypted_password;
        public string encrypted_username;
        public string decrypted_username;
        public registerysetting()
        {
            encrypted_password="";
            decrypted_password = "";
            encrypted_username = "";
            decrypted_username = "";
        }
        public void write_to_registery(string username , string password)
        {
            /// encryption password to registery
            /// 
            string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
            Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
            string key = variables["UserKey"];

            encrypted_password = Encryption.Encrypt(password, key);
            Microsoft.Win32.RegistryKey passRegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Titar_TV");
            passRegistryKey.SetValue("pass", encrypted_password);
            passRegistryKey.Close();

            /// encryption username to registery
            encrypted_username = Encryption.Encrypt(username , key);
            Microsoft.Win32.RegistryKey userRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Titar_TV",true);
            userRegistryKey.SetValue("uname", encrypted_username);
            userRegistryKey.Close();
        }
        public void read_from_registery()
        {
            try
            {
                string pathToFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\envfile.env";
                Dictionary<string, string> variables = DotEnvFile.DotEnvFile.LoadFile(pathToFile);
                string key = variables["UserKey"];

                /// decryption password from registery
                Microsoft.Win32.RegistryKey passRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Titar_TV");
                object str = passRegistryKey.GetValue("pass");
                passRegistryKey.Close();
                decrypted_password = Encryption.Decrypt(str.ToString(), key);

                /// decryption username from registery
                Microsoft.Win32.RegistryKey userRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Titar_TV");
                object str2 = userRegistryKey.GetValue("uname");
                userRegistryKey.Close();
                decrypted_username = Encryption.Decrypt(str2.ToString(), key);
            }
            catch
            {
                decrypted_password = "";
                decrypted_username = "";
            }
        }
        public void delete_from_registery()
        {
            Microsoft.Win32.RegistryKey passRegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Titar_TV",true);
            passRegistryKey.DeleteValue("pass");
            passRegistryKey.DeleteValue("uname");
        }

    }
}
