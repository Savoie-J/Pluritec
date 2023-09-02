using P.Data;
using P.Data.Models;
using P.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace P.Web.Controllers.API
{
    [AllowAnonymous]
    public class MeasurementController : ApiController
    {
        private readonly SQLContext _sql = new SQLContext();

        public async Task<IHttpActionResult> GetMeasurements(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest();

                Log log = await _sql.Logs.FindAsync(id);
                if (log == null)
                    return NotFound();

                int serialNumber = log.SerialNumber.Value - 1;

                return Ok(log.Measurements.OrderBy(l => l.Time).Select(m => {

                    do
                    {
                        serialNumber++;
                    }
                    while (log.SkippedSerialNumbers.Any(s => s.SerialNumber == serialNumber));

                    return new
                    {
                        m.ID,
                        Time = m.Time.ToString("G"),
                        SerialNumber = serialNumber,
                        ABSposX11 = m.HasMeasurements ? m.ABSposX11.ToString("0.00000") : string.Empty,
                        ABSposX12 = m.HasMeasurements ? m.ABSposX12.ToString("0.00000") : string.Empty,
                        ABSposX21 = m.HasMeasurements ? m.ABSposX21.ToString("0.00000") : string.Empty,
                        ABSposX22 = m.HasMeasurements ? m.ABSposX22.ToString("0.00000") : string.Empty,
                        ABSposY11 = m.HasMeasurements ? m.ABSposY11.ToString("0.00000") : string.Empty,
                        ABSposY12 = m.HasMeasurements ? m.ABSposY12.ToString("0.00000") : string.Empty,
                        ABSposY21 = m.HasMeasurements ? m.ABSposY21.ToString("0.00000") : string.Empty,
                        ABSposY22 = m.HasMeasurements ? m.ABSposY22.ToString("0.00000") : string.Empty
                    };
                }));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> GetLayers(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return BadRequest();

                Measurement measurement = await _sql.Measurements.FindAsync(id);
                if (measurement == null)
                    return NotFound();

                return Ok(measurement.Layers.Select(m => new {
                    m.ID,
                    m.NLayer,
                    m.ABSposX11,
                    m.ABSposX12,
                    m.ABSposX21,
                    m.ABSposX22,
                    m.ABSposY11,
                    m.ABSposY12,
                    m.ABSposY21,
                    m.ABSposY22
                }));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult GetFiles()
        {
            try
            {
                var allFiles = MdbManager.GetMdbFiles();

                var filenames = allFiles.Select(f => f.FileName).ToArray();

                var logs = _sql.Logs.Where(l => filenames.Contains(l.UploadName)).ToList();

                var newFiles = allFiles.Where(f => !logs.Any(l => l.UploadName == f.FileName)).ToList();

                return Ok(newFiles);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
