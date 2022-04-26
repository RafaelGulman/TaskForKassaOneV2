using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskForKassaOneV2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskForKassaOneV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Loan loan = null;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() => View();
        [HttpPost]
        public IActionResult Index(Loan l)
        {

            if (l.LoanAmount < 0)
            {
                ModelState.AddModelError("LoanAmount", "Сумма займа не должна быть меньше нуля");
            }
            else if (l.LoanPeriods < 0)
            {
                ModelState.AddModelError("LoanPeriods", "Количество периодов не должно быть меньше нуля");
            }
            else if (l.Rate < 0)
                ModelState.AddModelError("Rate", "Процентная ставка не должна быть меньше нуля");

            if (ModelState.IsValid)
            {
               
                return RedirectToAction("Result", "Home", new Loan(l)) ;
            }

            string errorMessages = "";

            foreach (var item in ModelState)
            {
                // если для определенного элемента имеются ошибки
                if (item.Value.ValidationState == ModelValidationState.Invalid)
                {
                    errorMessages = $"{errorMessages}\nОшибки для свойства {item.Key}:\n";
                    // пробегаемся по всем ошибкам
                    foreach (var error in item.Value.Errors)
                    {
                        errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
                    }
                }
            }
            return View();



        }

        public IActionResult Result(Loan l)
        {
            Loan loan = new Loan(l.LoanAmount, l.LoanPeriods,l.Rate,l.BeginLoan, l.MonthLoan);

            return View(loan);
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