using DataAccess.Contracts;
using DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Helpers
{
    public class Helper
    {
        private readonly IRepositoryWrapper _repository;
        private readonly string _securityKey;

        public Helper(IRepositoryWrapper repository)
        {
            _repository = repository;
            _securityKey = "raju";
        }

        public string Encrypt(string eCode, string Pwd)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(eCode + _securityKey + Pwd));

            //Convert and return the encrypted data/byte into string format.
            return BitConverter.ToString(hashedDataBytes, 0, hashedDataBytes.Length);
        }

        public string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public string EncryptText(string password)
		{
			RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();

			byte[] key = rc2CSP.Key;
			byte[] IV = rc2CSP.IV;

			ICryptoTransform encryptor = rc2CSP.CreateEncryptor(key, IV);

			MemoryStream msEncrypt = new MemoryStream();
			CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

			byte[] toEncrypt = Encoding.ASCII.GetBytes(password);

			csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
			csEncrypt.FlushFinalBlock();
			byte[] encrypted = msEncrypt.ToArray();

			return Convert.ToBase64String(encrypted);
		}

        // (RESIZE an image in a byte[] variable.)  
        public byte[] ReduceImageSize(byte[] bytes, int size)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var originalImage = new Bitmap(memoryStream);
            var resized = new Bitmap(size, size);
            using var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(originalImage, 0, 0, size, size);
            graphics.CompositingQuality = CompositingQuality.Default;
            using var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public byte[] IncreaseImageSize(byte[] bytes, int size)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var originalImage = new Bitmap(memoryStream);
            var resized = new Bitmap(size, size);
            using var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.Default;
            graphics.SmoothingMode = SmoothingMode.Default;
            graphics.InterpolationMode = InterpolationMode.Low;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(originalImage, 0, 0, size, size);
            graphics.CompositingQuality = CompositingQuality.Default;
            using var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();

        }
    }
}
