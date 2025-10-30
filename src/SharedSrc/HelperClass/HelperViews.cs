
namespace HelperClass;

using System;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security;
using System.Runtime.InteropServices;
using System.Security.Principal;

public static partial class HelperViewsStatic
{
    //static (string firstNumber, string secondNumer, string thirdNumber, string fourthNumber) UsingValueTuple(string allNumbers)
    //{
    //    string[] numbers = allNumbers.Split(' ');
    //    return new(numbers[0], numbers[1], numbers[2], numbers[3]);
    //}

    public static bool IsElevated()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
#pragma warning restore CA1416 // Validate platform compatibility
    }
    
    [SecurityCritical]
    public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d)
    {
        var result = (Delegate)(object)d;
        var MarshalMethod = typeof(Marshal).GetMethod("GetFunctionPointerForDelegateInternal", BindingFlags.NonPublic | BindingFlags.Static);
        var _compilationCallBackPtr = (IntPtr)(MarshalMethod.Invoke(null, new object[1] { result }));
        return _compilationCallBackPtr;
    }

#if NET451_OR_GREATER 
    public static (string PublicKey, string PublicToken) GetPublicKeyInfos(Assembly assembly)
    {
        byte[] _getPublicKeyArray = assembly.GetName().GetPublicKey();
        var _getPublicKey = ToHexString(_getPublicKeyArray);
        byte[] _getPublicKeyTokenArray = assembly.GetName().GetPublicKeyToken();
        var _getPublicKeyToken = ToHexString(_getPublicKeyTokenArray);
        //return Tuple.Create(_getPublicKey, _getPublicKeyToken);

        return (_getPublicKey, _getPublicKeyToken);
    }

    private static void testing()
    {
        var testing = GetPublicKeyInfos(default);
    }
#endif

    [__DynamicallyInvokable]
    public static bool IsNullOrWhiteSpace(string value)
    {
#if NET45_OR_GREATER
        return string.IsNullOrWhiteSpace(value);
#else
        if ((object)value == null)
        {
            return true;
        }

        for (int i = 0; i < value.Length; i++)
        {
            if (!char.IsWhiteSpace(value[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }

    public static void HTTPClientServicePointManager(int chunkcount)
    {
        ServicePointManager.Expect100Continue = false;
        ServicePointManager.DefaultConnectionLimit = 10000;
        ServicePointManager.SetTcpKeepAlive(true, Int32.MaxValue, 1);
        ServicePointManager.MaxServicePoints = chunkcount;
    }

    public static void SetupSecurityProtocol()
    {
#if NET20 || NET35
        // This property selects the version of the Secure Sockets Layer (SSL) or
        // existing connections aren't changed.
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)DotThreeFiveHelpers.Cores.SecurityProtocolTypeN.Tls11 
            | (SecurityProtocolType)DotThreeFiveHelpers.Cores.SecurityProtocolTypeN.Tls12;
#elif NET40
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | ((SecurityProtocolType)(0x300)) | ((SecurityProtocolType)(0xC00));

#else
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#endif

        // Accept the request for POST, PUT and PATCH verbs
        ServicePointManager.Expect100Continue = false;

        // Note: Any changes to the DefaultConnectionLimit property affect both HTTP 1.0 and HTTP 1.1 connections.
        // It is not possible to separately alter the connection limit for HTTP 1.0 and HTTP 1.1 protocols.
        ServicePointManager.DefaultConnectionLimit = 1000;

        // Set the maximum idle time of a ServicePoint instance to 10 seconds.
        // After the idle time expires, the ServicePoint object is eligible for
        // garbage collection and cannot be used by the ServicePointManager object.
        ServicePointManager.MaxServicePointIdleTime = 10000;

        ServicePointManager.ServerCertificateValidationCallback =
            new RemoteCertificateValidationCallback(CertificateValidationCallBack);
    }


    /// <summary>
    /// Sometime a server get certificate validation error
    /// https://stackoverflow.com/questions/777607/the-remote-certificate-is-invalid-according-to-the-validation-procedure-using
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="certificate"></param>
    /// <param name="chain"></param>
    /// <param name="sslPolicyErrors"></param>
    public static bool CertificateValidationCallBack(object sender,
        X509Certificate certificate,
        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // If the certificate is a valid, signed certificate, return true.
        if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

        // If there are errors in the certificate chain, look at each error to determine the cause.
        if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
        {
            if (chain?.ChainStatus != null)
            {
                foreach (X509ChainStatus status in chain.ChainStatus)
                {
                    if (status.Status == X509ChainStatusFlags.NotTimeValid)
                    {
                        // If the error is for certificate expiration then it can be continued
                        return true;
                    }
                    else if (certificate.Subject == certificate.Issuer &&
                             status.Status == X509ChainStatusFlags.UntrustedRoot)
                    {
                        // Self-signed certificates with an untrusted root are valid. 
                        continue;
                    }
                    else if (status.Status != X509ChainStatusFlags.NoError)
                    {
                        // If there are any other errors in the certificate chain, the certificate is invalid,
                        // so the method returns false.
                        return false;
                    }
                }
            }

            // When processing reaches this line, the only errors in the certificate chain are 
            // untrusted root errors for self-signed certificates. These certificates are valid
            // for default Exchange server installations, so return true.
            return true;
        }
        else
        {
            // In all other cases, return false.
            return false;
        }
    }

    //[__DynamicallyInvokable]
    //internal static readonly string Empty;

    //public static string Join<T>(string separator, IEnumerable<T> values)
    //{
    //    if (values == null)
    //    {
    //        throw new ArgumentNullException("values");
    //    }

    //    if ((object)separator == null)
    //    {
    //        separator = String.Empty;
    //    }

    //    using IEnumerator<T> enumerator = values.GetEnumerator();
    //    if (!enumerator.MoveNext())
    //    {
    //        return String.Empty;
    //    }

    //    StringBuilder stringBuilder = StringBuilderCache.Acquire();
    //    if (enumerator.Current != null)
    //    {
    //        string text = enumerator.Current.ToString();
    //        if ((object)text != null)
    //        {
    //            stringBuilder.Append(text);
    //        }
    //    }

    //    while (enumerator.MoveNext())
    //    {
    //        stringBuilder.Append(separator);
    //        if (enumerator.Current != null)
    //        {
    //            string text2 = enumerator.Current.ToString();
    //            if ((object)text2 != null)
    //            {
    //                stringBuilder.Append(text2);
    //            }
    //        }
    //    }

    //    return StringBuilderCache.GetStringAndRelease(stringBuilder);
    //}


    public static string ToHexString(byte[] arrays)
    {
      
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < arrays.GetLength(0); i++)
        {
            sb.Append(String.Format("{0:x2}", arrays[i]));
        }

        return sb.ToString();
    }

    const int BYTES_TO_READ = sizeof(Int64);

    static bool FilesAreEqual(FileInfo first, FileInfo second)
    {
        if (first.Length != second.Length)
            return false;

        if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
            return true;

        int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

        using (FileStream fs1 = first.OpenRead())
        using (FileStream fs2 = second.OpenRead())
        {
            byte[] one = new byte[BYTES_TO_READ];
            byte[] two = new byte[BYTES_TO_READ];

            for (int i = 0; i < iterations; i++)
            {
#pragma warning disable CA2022 // Avoid inexact read with 'Stream.Read'
                fs1.Read(one, 0, BYTES_TO_READ);
                fs2.Read(two, 0, BYTES_TO_READ);
#pragma warning restore CA2022 // Avoid inexact read with 'Stream.Read'
                if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                    return false;
            }
        }

        return true;
    }

    public static void RecustFileAndFolder(string folderpath)
    {
        if (IsNullOrWhiteSpace(folderpath) && !Directory.Exists(folderpath))
        {
            Debugger.Break();
            throw new ArgumentNullException(nameof(folderpath));
            var folderNewPath = "C:\\VSLatestSymbols";
        }
        //C:\\SymbolFiles\\WindowsCodecs.pdb\\EAE1253DBDF1015497E086409850476A1
        var dir = Directory.GetDirectories(folderpath, "*", System.IO.SearchOption.AllDirectories);
        //F:\\SymbolCache\\srvcli.pdb\\9B2D4D1514F7A3653BCA5EAFB4C3C4351\\stripped
        //string[] folders = System.IO.Directory.GetDirectories(@"F:\SymbolCache\", "*", System.IO.SearchOption.AllDirectories);

        for (int i = 0; i < dir.Length; i++)
        {
            string folder = dir[i];
            var files = Directory.GetFiles(folder);
            if (files.Length == 0) continue;
            for (int j = 0; j < files.Length; j++)
            {
                var file = files[j];
                var filetension = Path.GetExtension(file);

                if (filetension == ".pdb")
                {
                    var dirinfo = new DirectoryInfo(folder);
                    var subdir = dirinfo.Parent.FullName;
                    //var fileinfo = new FileInfo(file);
                    //fileinfo.MoveTo(subdir, "");

                    //var from = Path.GetDirectoryName(file);
                    //var to = subdir;

                    if (File.Exists(file))
                    {
                        var combined = Path.Combine(subdir, Path.GetFileName(file));
                        //var folderName = Path.GetFullPath(file);

                        var foldername = Path.GetDirectoryName(file);

                        if (foldername.Contains("stripped"))
                        {
                            if (File.Exists(combined) /*&& !FilesAreEqual(new FileInfo(file), new FileInfo(combined))*/)
                            {
                                File.Delete(combined);
                                Console.WriteLine("deleted {0}", dirinfo); // Success
                            }
                            File.Move(file, combined);
                            Console.WriteLine("moved {0}", dirinfo); // Success

                            //if (File.Exists(file) && FilesAreEqual(new FileInfo(file), new FileInfo(combined)))
                            //{
                            //    dirinfo.Delete(true);
                            //    Console.WriteLine("deleted {0}", dirinfo); // Success
                            //}
                            //else
                            //{
                            //    File.Move(file, combined); // Try to move
                            //    dirinfo.Delete(true);
                            //    Console.WriteLine("deleted {0}", dirinfo); // Success
                            //}
                                
                        }
                    }
                }
            }


        }


        //foreach (var subfolder in dir)
        //{
        //    var symdir = Directory.GetDirectories(subfolder);
        //    foreach (var subdir in symdir)
        //    {
        //        var subdir2 = Directory.GetDirectories(subdir);
        //        if (subdir2.Count() == 1)
        //        {
        //            var getPDB = Directory.GetFiles(subdir);
        //            foreach (var pdb in getPDB)
        //            {
        //                var extension = Path.GetExtension(pdb);
        //                if (extension == ".error")
        //                {

        //                }
        //                Console.WriteLine(extension);
        //            }
        //        }

        //    }
        //}
    }


    public static byte[] GetPublicKeyFromSnk(byte[] snk)
    {
        var snkp = new StrongNameKeyPair(snk);
        byte[] publicKey = snkp.PublicKey;
        return publicKey;
    }

    public static byte[] GetPublicKeyTokenFromSnk(byte[] publicKey)
    {
        using (var csp = new SHA1CryptoServiceProvider())
        {
            byte[] hash = csp.ComputeHash(publicKey);

            byte[] token = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                token[i] = hash[hash.Length - i - 1];
            }

            return token;
        }
    }

    public static void _RSACryptoServiceProvider(byte[] bytes)
    {
        RSACryptoServiceProvider rsa = new();

        rsa.ImportCspBlob(bytes);

        //byte[] publicKey = rsa.ExportRSAPublicKey();
        //byte[] privateKey = rsa.ExportRSAPrivateKey();
    }

    public static List<string> SNKArrayFromSnk(string Snklocation)
    {
        byte[] snk = File.ReadAllBytes(Snklocation);
        byte[] publicKey = GetPublicKeyFromSnk(snk);

        var _publicKey =
#if NET20 || NET35
            Join("", publicKey.ToArray());
#else
            string.Join("", publicKey.ToArray());
#endif
       
        byte[] publicKeyToken = GetPublicKeyTokenFromSnk(publicKey);

        var _publicKeyToken =
#if NET20 || NET35
            Join("", publicKeyToken.ToArray());
#else
            string.Join("", publicKey.ToArray());
#endif
        return [_publicKey, _publicKeyToken];
    }


#if NET20 || NET35
    public static string Join<T>(string separator, IEnumerable<T> values)
    {
        if (values == null)
        {
            throw new ArgumentNullException("values");
        }

        if ((object)separator == null)
        {
            separator = string.Empty;
        }

        using IEnumerator<T> enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return string.Empty;
        }

        StringBuilder stringBuilder = FrameWorkEngine.StringBuilderCache.Acquire();
        if (enumerator.Current != null)
        {
            string text = enumerator.Current.ToString();
            if ((object)text != null)
            {
                stringBuilder.Append(text);
            }
        }

        while (enumerator.MoveNext())
        {
            stringBuilder.Append(separator);
            if (enumerator.Current != null)
            {
                string text2 = enumerator.Current.ToString();
                if ((object)text2 != null)
                {
                    stringBuilder.Append(text2);
                }
            }
        }

        return FrameWorkEngine.StringBuilderCache.GetStringAndRelease(stringBuilder);
    }
#endif



}

