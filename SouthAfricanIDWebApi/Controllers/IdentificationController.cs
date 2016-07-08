using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SouthAfricanIDWebApi.Models;

namespace SouthAfricanIDWebApi.Controllers
{
    public class IdentificationController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "South African ID Verifier";

            return View();
        }

        [HttpGet]
        public JsonResult Generate()
        {
            APIResponse response = new APIResponse();

            try
            {
                SAIdentification identification = new SAIdentification();

                string dateSection = "", genderSection = "", countryIdSection = "", previouslyRaceSection = "", lastDigit = "";
                bool isValidDateGenerated = false;
                DateTime testDate;

                while (!isValidDateGenerated)
                {
                    dateSection = SAIdentification.generateDateSection();

                    try
                    {
                        testDate = new DateTime(
                            Int32.Parse(dateSection.Substring(0, 2)),
                            Int32.Parse(dateSection.Substring(2, 2)),
                            Int32.Parse(dateSection.Substring(4, 2))
                        );

                        genderSection = SAIdentification.generateGenderSection().PadLeft(4, '0');
                        countryIdSection = SAIdentification.generateCountryIdSection();
                        previouslyRaceSection = SAIdentification.generatePreviouslyRaceSection();
                        lastDigit = SAIdentification.generateLastDigit();

                        string fullIdNumber = dateSection + genderSection + countryIdSection + previouslyRaceSection + lastDigit;
                        identification.IdNumber = fullIdNumber;
                        identification.verify(true);
                        response.Data = identification;

                        isValidDateGenerated = true;
                    }
                    catch (Exception ex)
                    {
                        // An exception occured, no need to do anything
                    }
                }
            }
            catch(Exception ex)
            {
                response.Data = ex.Data;
                response.Error = ex.Message;
            }
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult verify(string id)
        {
            APIResponse response = new APIResponse();

            try
            {
                SAIdentification identification = new SAIdentification { IdNumber = id };
                identification.verify(false);
                response.Data = identification;
            }
            catch (Exception ex)
            {
                response.Data = ex.Data;
                response.Error = ex.Message;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
