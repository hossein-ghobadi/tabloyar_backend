

//namespace Endpoint.Site.Controllers
//{
//    using Microsoft.EntityFrameworkCore;

//    using Microsoft.AspNetCore.Hosting;
//    using Microsoft.AspNetCore.Http;
//    using Microsoft.AspNetCore.Mvc;
//    using OfficeOpenXml;
//    using Radin.Application.Interfaces.Contexts;
//    using Radin.Application.Interfaces.FacadPatterns;
//    using Radin.Application.Services.Excelloading;
//    using Radin.Application.Services.Excelloading.FacadPattern;
//    using System.ComponentModel;
//    using System.IO;
//    using LicenseContext = OfficeOpenXml.LicenseContext;
//    using Persistence.Contexts;
//    using Endpoint.Site.Models.ViewModels.ExcellViewModel;
//    using Radin.Application.Services.StateInfoLoadingExcel;
//    using Endpoint.Site.Models.ViewModels;
//    using Newtonsoft.Json;
//    using Radin.Application.Services.OtherExcelloading;

//    public class ExcelController : Controller
//    {
//        private readonly IWebHostEnvironment _environment;
//        private readonly IPriceFeeDataBaseContext _context;
//        private readonly IExcelLoadingFacad _excelLoadingFacad;
//        private readonly PriceFeeDataBaseContext __context;
//        private readonly DataBaseContext _dataBaseContext;
//        private readonly ExcelloadingForCities _excelloadingForCities;
//        private readonly CustomerExcelLoading _customerExcelLoading;
//        private readonly MainFactorExcelLoading _mainFactorExcelLoading;
//        private readonly JobCategoryExcelLoading _jobCategoryExcelLoading;
//        private readonly AcessoryExcelloading _acessoryExcelLoading;
//        public ExcelController(
//            IWebHostEnvironment environment,
//            IPriceFeeDataBaseContext context,
//            IExcelLoadingFacad excelLoadingFacad,
//            PriceFeeDataBaseContext context2,
//            DataBaseContext dataBaseContext,
//            ExcelloadingForCities excelloadingForCities,
//            CustomerExcelLoading customerExcelLoading,
//            MainFactorExcelLoading mainFactorExcelLoading,
//            JobCategoryExcelLoading jobCategoryExcelLoading,
//            AcessoryExcelloading acessoryExcelloading

//            )
//        {
//            _environment = environment;
//            _context = context;
//            _excelLoadingFacad = excelLoadingFacad;
//            __context= context2;
//            _dataBaseContext = dataBaseContext;
//            _excelloadingForCities = excelloadingForCities;
//            _customerExcelLoading = customerExcelLoading;
//            _mainFactorExcelLoading = mainFactorExcelLoading;
//            _jobCategoryExcelLoading = jobCategoryExcelLoading;
//            _acessoryExcelLoading = acessoryExcelloading;
//        }
//        // Other database operations




//        public IActionResult Address()
//        {
//            return View();
//        }


//        public async Task<IActionResult> Geocode(GeocodeRequest request)
//        {
//            string apiKey = "YOUR_OPENCAGE_API_KEY"; // Replace with your actual API key
//            string url = $"https://api.opencagedata.com/geocode/v1/json?q={request.Address}&key={apiKey}";

//            using (HttpClient client = new HttpClient())
//            {
//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var json = await response.Content.ReadAsStringAsync();
//                    dynamic data = JsonConvert.DeserializeObject(json);
//                    if (data.results.Count > 0)
//                    {
//                        ViewBag.Latitude = data.results[0].geometry.lat;
//                        ViewBag.Longitude = data.results[0].geometry.lng;
//                    }
//                }
//            }

//            return View("Address");
//        }

//        public ActionResult Map()
//        {
//            return View();
//        }
//        public IActionResult Upload()
//        {
//            return View();
//        }

//        public IActionResult UploadState()
//        {
//            return View();
//        }


//        public IActionResult UploadCustomerFactor()
//        {
//            return View();
//        }


//        [HttpPost]
//        public IActionResult ReseedDatabase(int p)
//        {
//            try
//            {
//                if (p == 122) 
//                {
//                    //_dataBaseContext.Services.RemoveRange(_dataBaseContext.Services);
//                    //_dataBaseContext.ProductFactors.RemoveRange(_dataBaseContext.ProductFactors);
//                    //_dataBaseContext.Accessories.RemoveRange(_dataBaseContext.Accessories);


                    
//                    _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('ProductFactors', RESEED, 10000);");
//                    _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Accessories', RESEED, 4000);");
//                    _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Services', RESEED, 8000);");

//                    //_
//                    return Ok("File processed successfully.");

//                }
//                return Ok("nothing happen");

//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"An error occurred: {ex.Message}");
//            }
//        }







//            [HttpPost]
//        public IActionResult UploadCF(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest("Upload a valid Excel file.");
//            }
//            string filePath = Path.GetTempFileName();



//            try
//            {
//                _dataBaseContext.CustomerInfo.RemoveRange(_dataBaseContext.CustomerInfo);
//                _dataBaseContext.MainFactors.RemoveRange(_dataBaseContext.MainFactors);
//                //_dataBaseContext.JobCategoryInfo.RemoveRange(_dataBaseContext.JobCategoryInfo);
//                //_dataBaseContext.Accessories.RemoveRange(_dataBaseContext.Accessories);


//                _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('CustomerInfo', RESEED, 12200);");
//                _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('MainFactors', RESEED, 10000);");
//                //_dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('JobCategoryInfo', RESEED, 0);");
//                _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('SubFactors', RESEED, 0);");
//                _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('ProductFactors', RESEED, 10000);");
//                //_dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Accessories', RESEED, 0);");

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    file.CopyTo(stream);
//                }

//                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust according to your license

//                _dataBaseContext.CustomerInfo.AddRange(_customerExcelLoading.ReadDataFromExcel(filePath, 0).CustomerInfos);
//                _dataBaseContext.MainFactors.AddRange(_mainFactorExcelLoading.ReadDataFromExcel(filePath, 1).MainFactors);
//                //_dataBaseContext.JobCategoryInfo.AddRange(_jobCategoryExcelLoading.ReadDataFromExcel(filePath, 2).JobCategories);
//                //_dataBaseContext.Accessories.AddRange(_acessoryExcelLoading.ReadDataFromExcel(filePath, 0).AccessoriesList);

//                _dataBaseContext.SaveChanges();
//                //Attempt to open the file with or without a password
//                using (var package = TryOpenPackage(filePath))
//                {
//                    // Example: Read something from the package
//                    var worksheet = package.Workbook.Worksheets[0];
//                    var value = worksheet.Cells[1, 1].Text;
//                    // Process the file as needed
//                }

//                System.IO.File.Delete(filePath); // Clean up the temp file
//                return Ok("File processed successfully.");
//            }
//            catch (InvalidDataException)
//            {
//                return BadRequest("The file is not a valid Excel file or it is encrypted.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"An error occurred: {ex.Message}");
//            }
//        }

//        [HttpPost]
//        public IActionResult UploadStateCity(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest("Upload a valid Excel file.");
//            }
//            string filePath = Path.GetTempFileName();



//            try
//            {
//                _dataBaseContext.Cities.RemoveRange(_dataBaseContext.Cities);
//                _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Cities', RESEED, 0);");

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    file.CopyTo(stream);
//                }

//                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust according to your license

//                _dataBaseContext.Cities.AddRange(_excelloadingForCities.ReadDataFromExcel(filePath, 0).Infos);
//                _dataBaseContext.SaveChanges();
//                //Attempt to open the file with or without a password
//                using (var package = TryOpenPackage(filePath))
//                {
//                    // Example: Read something from the package
//                    var worksheet = package.Workbook.Worksheets[0];
//                    var value = worksheet.Cells[1, 1].Text;
//                    // Process the file as needed
//                }

//                System.IO.File.Delete(filePath); // Clean up the temp file
//                return Ok("File processed successfully.");
//            }
//            catch (InvalidDataException)
//            {
//                return BadRequest("The file is not a valid Excel file or it is encrypted.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"An error occurred: {ex.Message}");
//            }
//        }






//        [HttpPost]
//        public IActionResult UploadFile(IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest("Upload a valid Excel file.");
//            }
//            //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {file.Length.ToString()}");
//            string filePath = Path.GetTempFileName();

//            try
//            {


//                _context.Materials.RemoveRange(_context.Materials);
//                _context.EdgeProperties.RemoveRange(_context.EdgeProperties);
//                _context.EdgePunchs.RemoveRange(_context.EdgePunchs);
//                _context.Smds.RemoveRange(_context.Smds);
//                _context.Crystals.RemoveRange(_context.Crystals);
//                _context.ColorCosts.RemoveRange(_context.ColorCosts);
//                _context.Punchs.RemoveRange(_context.Punchs);
//                _context.Glues.RemoveRange(_context.Glues);
//                _context.Powers.RemoveRange(_context.Powers);
//                _context.Margins.RemoveRange(_context.Margins);
//                _context.Titles.RemoveRange(_context.Titles);
//                _context.QualityDegrees.RemoveRange(_context.QualityDegrees);
//                _context.MaterialEdgeSizes.RemoveRange(_context.MaterialEdgeSizes);
//                _context.MaterialEdgeColors.RemoveRange(_context.MaterialEdgeColors);
//                //_context.SecondEdgeColors.RemoveRange(_context.SecondEdgeColors);
//                _context.MaterialColors.RemoveRange(_context.MaterialColors);
//                _context.SecondLayerMaterials.RemoveRange(_context.SecondLayerMaterials);
//                _dataBaseContext.Accessories.RemoveRange(_dataBaseContext.Accessories);



//                // Reset all identity columns
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Materials', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('EdgeProperties', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('EdgePunchs', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Smds', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Crystals', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('ColorCosts', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Punchs', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Glues', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Powers', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Margins', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Titles', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('QualityDegrees', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('MaterialEdgeSizes', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('MaterialEdgeColors', RESEED, 0);");
//                //_context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('SecondEdgeColors', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('MaterialColors', RESEED, 0);");
//                __context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('SecondLayerMaterials', RESEED, 0);");
//                _dataBaseContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Accessories', RESEED, 4000);");

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    file.CopyTo(stream);
//                }
                
//                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Adjust according to your license
//                _context.Materials.AddRange(_excelLoadingFacad.MaterialsLoading.ReadDataFromExcel(filePath, 0).Materials);
//                _context.EdgeProperties.AddRange(_excelLoadingFacad.EdgePropertiesLoading.ReadDataFromExcel(filePath, 1).EdgeProperties);
//                _context.EdgePunchs.AddRange(_excelLoadingFacad.EdgePunchsLoading.ReadDataFromExcel(filePath, 2).EdgePunchs);
//                _context.Smds.AddRange(_excelLoadingFacad.SmdsLoading.ReadDataFromExcel(filePath, 3).Smds);
//                _context.Crystals.AddRange(_excelLoadingFacad.CrystalsLoading.ReadDataFromExcel(filePath, 4).Crystals);
//                _context.ColorCosts.AddRange(_excelLoadingFacad.ColorCostsLoading.ReadDataFromExcel(filePath, 5).ColorCosts);
//                _context.Punchs.AddRange(_excelLoadingFacad.PunchsLoading.ReadDataFromExcel(filePath, 6).Punchs);
//                _context.Glues.AddRange(_excelLoadingFacad.GluesLoading.ReadDataFromExcel(filePath, 7).GLues);
//                _context.Powers.AddRange(_excelLoadingFacad.PowersLoading.ReadDataFromExcel(filePath, 8).Powers);
//                _context.Margins.AddRange(_excelLoadingFacad.MarginsLoading.ReadDataFromExcel(filePath, 9).Margins);
//                _context.Titles.AddRange(_excelLoadingFacad.TitlesLoading.ReadDataFromExcel(filePath, 10).Titles);
//                _context.QualityDegrees.AddRange(_excelLoadingFacad.QualityDegreeLoading.ReadDataFromExcel(filePath, 11).QualityDegrees);
//                _context.MaterialEdgeSizes.AddRange(_excelLoadingFacad.MaterialEdgeSizesLoading.ReadDataFromExcel(filePath, 13).MaterialEdgeSizes);
//                _context.MaterialEdgeColors.AddRange(_excelLoadingFacad.MaterialEdgeColorsLoading.ReadDataFromExcel(filePath, 12).MaterialEdgeColors);
//                //_context.SecondEdgeColors.AddRange(_excelLoadingFacad.SecondEdgeColorsLoading.ReadDataFromExcel(filePath, 14).MaterialEdgeColors);
//                _context.MaterialColors.AddRange(_excelLoadingFacad.MaterialColorsLoading.ReadDataFromExcel(filePath, 14).MaterialColors);
//                _context.SecondLayerMaterials.AddRange(_excelLoadingFacad.SecondLayerMaterialLoading.ReadDataFromExcel(filePath, 15).SecondLayerMaterials);
//                _dataBaseContext.Accessories.AddRange(_acessoryExcelLoading.ReadDataFromExcel(filePath, 16).AccessoriesList);


//                _context.SaveChanges();
//                _dataBaseContext.SaveChanges();
//                //Attempt to open the file with or without a password
//                using (var package = TryOpenPackage(filePath))
//                {
//                    // Example: Read something from the package
//                    var worksheet = package.Workbook.Worksheets[0];
//                    var value = worksheet.Cells[1, 1].Text;
//                    // Process the file as needed
//                }

//                System.IO.File.Delete(filePath); // Clean up the temp file
//                return Ok("File processed successfully.");
//            }
//            catch (InvalidDataException)
//            {
//                return BadRequest("The file is not a valid Excel file or it is encrypted.");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"An error occurred: {ex.Message}");
//            }
//        }

//        private ExcelPackage TryOpenPackage(string filePath, string password = null)
//        {
//            try
//            {
//                var fileInfo = new FileInfo(filePath);
//                if (string.IsNullOrEmpty(password))
//                {
//                    return new ExcelPackage(fileInfo);
//                }
//                else
//                {
//                    return new ExcelPackage(fileInfo, password);
//                }
//            }
//            catch (InvalidDataException ex)
//            {
//                // Rethrow if the file cannot be opened
//                throw new InvalidDataException("Could not open the Excel package.", ex);
//            }
//        }






//        public IActionResult DisplayData()
//        {
//            var viewModel = new ExcelDataViewModel
//            {
//                Materials = _context.Materials.ToList(),
//                EdgeProperties = _context.EdgeProperties.ToList(),
//                Crystals = _context.Crystals.ToList(),
//                ColorCosts = _context.ColorCosts.ToList(),
//                EdgePunches = _context.EdgePunchs.ToList(),
//                Glues = _context.Glues.ToList(),
//                Powers = _context.Powers.ToList(),
//                Punches = _context.Punchs.ToList(),
//                SecondLayerMaterials = _context.SecondLayerMaterials.ToList(),
//                Smds = _context.Smds.ToList(),
//                Margins = _context.Margins.ToList(),
//                MaterialColors = _context.MaterialColors.ToList(),
//                MaterialEdgeColors = _context.MaterialEdgeColors.ToList(),
//                MaterialEdgeSizes = _context.MaterialEdgeSizes.ToList(),
//                QualityDegrees = _context.QualityDegrees.ToList(),
//                Titles = _context.Titles.ToList()
//            };

//            return View(viewModel);
//        }



//    }
//}
