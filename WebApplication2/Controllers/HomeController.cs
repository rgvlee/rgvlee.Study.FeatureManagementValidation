using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using WebApplication2.Models;
using WebApplication2.Validation;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;
        private readonly IFeatureManagerSnapshotValidator _featureManagerSnapshotValidator;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IFeatureManagerSnapshot featureManagerSnapshot, IFeatureManagerSnapshotValidator featureManagerSnapshotValidator)
        {
            _logger = logger;
            _featureManagerSnapshot = featureManagerSnapshot;
            _featureManagerSnapshotValidator = featureManagerSnapshotValidator;
        }

        public async Task<IActionResult> Index()
        {
            // var validationResult = await _featureManagerSnapshotValidator.ValidateAsync(_featureManagerSnapshot);
            // if (!validationResult.IsValid)
            // {
            //     validationResult.AddToModelState(ModelState, string.Empty);
            //     return BadRequest();
            // }

            _logger.LogDebug("Invoking IsEnabledAsync");
            var isArtIntegrationEnabled = await _featureManagerSnapshot.IsEnabledAsync("ArtIntegration");
            _logger.LogDebug($"isArtIntegrationEnabled: {isArtIntegrationEnabled}");
            var areTrackElementsEnabled = await _featureManagerSnapshot.IsEnabledAsync("TrackElements");
            _logger.LogDebug($"areTrackElementsEnabled: {areTrackElementsEnabled}");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}