using P.Data;
using P.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace P.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly SQLContext _sql = new SQLContext();
        public async Task<ActionResult> Index()
        {
            List<Specification> specifications = await _sql.Specifications
                .Where(s => s.LayerOne.HasValue
                            && s.LayerTwo.HasValue
                            && s.Tolerance.HasValue
                            && s.Offset.HasValue)
                .ToListAsync();

            return View(specifications);
        }

        [HttpPost]
        public string Index(Specification specification)
        {
            try
            {
                if (specification == null)
                    return "Invalid form submission.";

                ModelState.Remove("ID");

                if (!specification.LayerOne.HasValue)
                    ModelState.AddModelError("LayerOne", "Layer One is required.");

                if (!specification.LayerTwo.HasValue)
                    ModelState.AddModelError("LayerTwo", "Layer Two is required.");

                if (!specification.Offset.HasValue)
                    ModelState.AddModelError("Offset", "Offset is required.");

                if (!specification.Tolerance.HasValue)
                    ModelState.AddModelError("Tolerance", "Tolerance is required.");

                if (!ModelState.IsValid)
                    return string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

                Specification saving = _sql.Specifications.FirstOrDefault(s => s.PartNumber == specification.PartNumber);
                if (saving == null)
                    saving = _sql.Specifications.Add(new Specification
                    {
                        PartNumber = specification.PartNumber
                    });

                saving.LayerOne = specification.LayerOne;
                saving.LayerTwo = specification.LayerTwo;
                saving.Tolerance = specification.Tolerance;
                saving.Offset = specification.Offset;

                _sql.SaveChanges();

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<ActionResult> Log(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Specification specification = await _sql.Specifications.FindAsync(id);
            if (specification == null)
                return HttpNotFound();

            if (!specification.IsSetup)
                return HttpNotFound();

            return View(specification);
        }
    }
}