using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace StockSalesOrderList.Helpers
{
    public static class FileHash
    {
        public static string GetSHA1Hash(string filePath)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(filePath, sha1);
        }
        public static string GetSHA1Hash(Stream s)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(s, sha1);
        }
        public static string GetMD5Hash(string filePath)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(filePath, md5);
        }
        public static string GetMD5Hash(Stream s)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(s, md5);
        }
        private static string GetHash(string filePath, HashAlgorithm hasher)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return GetHash(fs, hasher);
        }
        private static string GetHash(Stream s, HashAlgorithm hasher)
        {
            var hash = hasher.ComputeHash(s);
            var hashStr = Convert.ToBase64String(hash);
            return hashStr.TrimEnd('=');
        }
    }

    public struct ShortGuid
    {
        #region Static
        public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);
        #endregion

        #region Fields
        Guid _guid;
        string _value;
        #endregion

        #region Contructors
        public ShortGuid(string value)
        {
            _value = value;
            _guid = Decode(value);
        }
        public ShortGuid(Guid guid)
        {
            _value = Encode(guid);
            _guid = guid;
        }
        #endregion

        #region Properties
        public Guid Guid
        {
            get { return _guid; }
            set
            {
                if (value != _guid)
                {
                    _guid = value;
                    _value = Encode(value);
                }
            }
        }
        public string Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    _guid = Decode(value);
                }
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return _value;
        }
        #endregion

        #region Equals
        public override bool Equals(object obj)
        {
            if (obj is ShortGuid)
                return _guid.Equals(((ShortGuid)obj)._guid);
            if (obj is Guid)
                return _guid.Equals((Guid)obj);
            if (obj is string)
                return _guid.Equals(((ShortGuid)obj)._guid);
            return false;
        }
        #endregion

        #region GetHashCode
        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }
        #endregion

        #region NewGuid
        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }
        #endregion

        #region Encode
        public static string Encode(string value)
        {
            Guid guid = new Guid(value);
            return Encode(guid);
        }
        public static string Encode(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded
              .Replace("/", "_")
              .Replace("+", "-");
            return encoded.Substring(0, 22);
        }
        #endregion

        #region Decode
        public static Guid Decode(string value)
        {
            value = value
              .Replace("_", "/")
              .Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }
        #endregion

        #region Operators
        public static bool operator ==(ShortGuid x, ShortGuid y)
        {
            if ((object)x == null) return (object)y == null;
            return x._guid == y._guid;
        }
        public static bool operator !=(ShortGuid x, ShortGuid y)
        {
            return !(x == y);
        }
        public static implicit operator string(ShortGuid shortGuid)
        {
            return shortGuid._value;
        }
        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid._guid;
        }
        public static implicit operator ShortGuid(string shortGuid)
        {
            return new ShortGuid(shortGuid);
        }
        public static implicit operator ShortGuid(Guid guid)
        {
            return new ShortGuid(guid);
        }
       #endregion
    }
}
