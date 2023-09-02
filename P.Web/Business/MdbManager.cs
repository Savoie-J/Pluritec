using P.Data.Models;
using P.Data;
using P.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace P.Web.Business
{
    public static class MdbManager
    {
        private const string _extension = ".mdb";

        private static readonly DirectoryInfo _remote = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigContext.FilePath));

        private static readonly string _local = HostingEnvironment.MapPath("~/App_Data/Databases");

        public static bool ValidateFile(HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength == 0)
                return false;

            string extension = Path.GetExtension(upload.FileName);

            return string.Equals(extension, _extension, StringComparison.InvariantCultureIgnoreCase);
        }

        public static List<DataFile> GetMdbFiles()
        {
            return _remote
                .GetFiles($"*{_extension}")
                //.Where(f => f.CreationTime >= DateTime.Today)
                .Select(f => new DataFile { FileName = f.Name, DateCreated = f.CreationTime })
                .ToList();
        }

        public static FileInfo SaveCopy(string remote_filename, string local_filename)
        {
            FileInfo selectedFile = _remote
                .GetFiles(remote_filename)
                .FirstOrDefault();

            if (selectedFile == null || !selectedFile.Exists || selectedFile.CreationTime <= DateTime.Today)
                return null;

            string path = Path.Combine(_local, local_filename);

            return selectedFile.CopyTo(path);
        }

        public static FileInfo SaveCopy(HttpPostedFileBase upload, string local_filename)
        {
            if (upload == null || upload.ContentLength == 0)
                return null;

            string path = Path.Combine(_local, local_filename);

            upload.SaveAs(path);

            return new FileInfo(path);
        }

        public static byte[] GetData(FileInfo fileInfo)
        {
            using (BinaryReader reader = new BinaryReader(fileInfo.Open(FileMode.Open, FileAccess.Read)))
            {
                return reader.ReadBytes((int)fileInfo.Length);
            }
        }

        public static List<Measurement> Extract(string fileName)
        {
            try
            {
                string path = Path.Combine(_local, fileName);

                using (MdbContext mdbContext = new MdbContext(path))
                {
                    List<LayerData> data = mdbContext.GetMeasurementData();

                    List<Measurement> measurements = data
                        .GroupBy(d => d.time)
                        .Select(t => new Measurement
                        {
                            Time = DateTimeOffset.FromUnixTimeSeconds(t.Key).DateTime,
                            Layers = t.Select(l => new Layer
                            {
                                NLayer = l.N_layer,
                                ABSposX11 = l.ABSposX11,
                                ABSposX12 = l.ABSposX12,
                                ABSposX21 = l.ABSposX21,
                                ABSposX22 = l.ABSposX22,
                                ABSposY11 = l.ABSposY11,
                                ABSposY12 = l.ABSposY12,
                                ABSposY21 = l.ABSposY21,
                                ABSposY22 = l.ABSposY22
                            }).ToList()
                        }).ToList();

                    return measurements;
                }
            }
            catch
            {
                throw new Exception("Invalid file format.");
            }
        }
    }
}