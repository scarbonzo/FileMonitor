using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor.SVC.Models
{
    public class file
    {
        public file(System.IO.FileInfo file)
        {
            Id = Guid.NewGuid();
            Name = file.Name;
            Folder = file.Directory.ToString();
            Extension = file.Extension;
            FullPath = file.FullName;
            Length = file.Length;
            Created = file.CreationTime;
            Modified = file.LastWriteTime;
            Accessed = file.LastAccessTime;
            Owner = System.IO.File.GetAccessControl(file.FullName).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            Checksum = GenerateChecksum(file.FullName);
            LastPolled = DateTime.Now;
            HashCode = file.GetHashCode();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }
        public long Length { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }
        public string Owner { get; set; }
        public string Checksum { get; set; }
        public int HashCode { get; set; }
        public DateTime LastPolled { get; set; }

        public static string GenerateChecksum(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            return FullPath == ((file)obj).FullPath ? true : false;
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}
