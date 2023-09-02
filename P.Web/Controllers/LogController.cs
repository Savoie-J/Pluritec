using Newtonsoft.Json;
using P.Data;
using P.Data.Models;
using P.Web.Business;
using P.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace P.Web.Controllers
{
    public class LogController : Controller
    {
        private readonly SQLContext _sql = new SQLContext();

        public async Task<ActionResult> Index()
        {
            var logs = await _sql.Logs./*Where(l => l.LogDate >= DateTime.Today).*/ToListAsync();

            return View(logs);
        }

        [HttpPost]
        public string Create(CreateViewModel create)
        {
            try
            {
                if (create == null)
                    return "Invalid form submission.";

                if (!ModelState.IsValid)
                    return string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                bool exists = _sql.Logs.Any(l => l.UploadName == create.SelectedFile);

                if (exists)
                    return $"An entry for this file: {create.SelectedFile}, already exists.";

                string partNumber = _sql.LookupPartNumber(create.JobNumber);

                if (string.IsNullOrWhiteSpace(partNumber))
                    return $"The ID: {create.JobNumber}, lacks an accompanying part number.";

                Specification specification = _sql.Specifications.FirstOrDefault(s => s.PartNumber == partNumber);

                if (specification == null)
                    specification = _sql.Specifications.Add(new Specification
                    {
                        PartNumber = partNumber,
                        Logs = new List<Log>()
                    });

                Log saving = new Log
                {
                    JobNumber = create.JobNumber,
                    SerialNumber = create.SerialNumber,
                    Operator = create.EmployeeID,
                    Comments = create.Comments,
                    UploadName = create.SelectedFile,
                    LogDate = DateTime.Now,
                    UploadGuid = Guid.NewGuid()
                };

                var fileCopy = MdbManager.SaveCopy(saving.UploadName, saving.FileName);

                if (fileCopy == null)
                    return $"Entry lacks an accompanying file upload.";

                saving.UploadData = MdbManager.GetData(fileCopy);

                saving.Measurements = MdbManager.Extract(saving.FileName);

                if (!string.IsNullOrWhiteSpace(create.SkippedSerialNumbers))
                {
                    List<string> serialNumbers = JsonConvert.DeserializeObject<List<string>>(create.SkippedSerialNumbers);
                    saving.SkippedSerialNumbers = serialNumbers
                        .Select(s =>
                        {
                            int output;
                            if (int.TryParse(s, out output))
                                return (int?)output;

                            return null;
                        })
                        .Where(s => s.HasValue)
                        .Select(s => new SkippedSerialNumber { SerialNumber = s.Value })
                        .ToList();
                }

                specification.Logs.Add(saving);

                _sql.SaveChanges();

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string Manual(ManualViewModel manual)
        {
            try
            {
                if (manual == null)
                    return "Invalid form submission.";

                if (!ModelState.IsValid)
                    return string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                if (!MdbManager.ValidateFile(manual.UploadedFile))
                    return "Only MDB files are accepted for upload.";

                bool exists = _sql.Logs.Any(l => l.UploadName == manual.UploadedFile.FileName);

                if (exists)
                    return $"An entry for this file: {manual.UploadedFile.FileName}, already exists.";

                string partNumber = _sql.LookupPartNumber(manual.JobNumber);

                if (string.IsNullOrWhiteSpace(partNumber))
                    return $"The ID: {manual.JobNumber}, lacks an accompanying part number.";

                Specification specification = _sql.Specifications.FirstOrDefault(s => s.PartNumber == partNumber);

                if (specification == null)
                    specification = _sql.Specifications.Add(new Specification
                    {
                        PartNumber = partNumber,
                        Logs = new List<Log>()
                    });

                Log saving = new Log
                {
                    JobNumber = manual.JobNumber,
                    SerialNumber = manual.SerialNumber,
                    Operator = manual.EmployeeID,
                    Comments = manual.Comments,
                    UploadName = manual.UploadedFile.FileName,
                    LogDate = DateTime.Now,
                    UploadGuid = Guid.NewGuid()
                };

                var fileCopy = MdbManager.SaveCopy(manual.UploadedFile, saving.FileName);

                if (fileCopy == null)
                    return $"Entry lacks an accompanying file upload.";

                saving.UploadData = MdbManager.GetData(fileCopy);

                saving.Measurements = MdbManager.Extract(saving.FileName);

                if (!string.IsNullOrWhiteSpace(manual.SkippedSerialNumbers))
                {
                    List<string> serialNumbers = JsonConvert.DeserializeObject<List<string>>(manual.SkippedSerialNumbers);
                    saving.SkippedSerialNumbers = serialNumbers
                        .Select(s =>
                        {
                            int output;
                            if (int.TryParse(s, out output))
                                return (int?)output;

                            return null;
                        })
                        .Where(s => s.HasValue)
                        .Select(s => new SkippedSerialNumber { SerialNumber = s.Value })
                        .ToList();
                }

                specification.Logs.Add(saving);

                _sql.SaveChanges();

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string Edit(Log log)
        {
            try
            {
                if (log == null)
                    return "Invalid form submission.";

                if (!ModelState.IsValid)
                    return string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                Log saving = _sql.Logs.Find(log.ID);

                saving.Comments = log.Comments;
                saving.SerialNumber = log.SerialNumber;

                if (!string.IsNullOrWhiteSpace(log.SerialNumbers))
                {
                    List<string> serialNumbers = JsonConvert.DeserializeObject<List<string>>(log.SerialNumbers);
                    List<int> parsed = serialNumbers
                        .Select(s =>
                        {
                            int output;
                            if (int.TryParse(s, out output))
                                return (int?)output;

                            return null;
                        })
                        .Where(s => s.HasValue)
                        .Select(s => s.Value)
                        .ToList();

                    var remove = saving.SkippedSerialNumbers
                        .Where(s => !parsed.Contains(s.SerialNumber))
                        .ToList();

                    _sql.SkippedSerialNumbers.RemoveRange(remove);

                    var add = parsed
                        .Where(p => !saving.SkippedSerialNumbers.Any(s => s.SerialNumber == p))
                        .Select(s => new SkippedSerialNumber { SerialNumber = s })
                        .ToList();

                    saving.SkippedSerialNumbers.AddRange(add);
                }

                _sql.SaveChanges();

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}