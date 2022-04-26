using static TaskForKassaOneV2.Models.LoanCalculator;
using System.ComponentModel.DataAnnotations;

namespace TaskForKassaOneV2.Models
{
    public class Loan : IValidatableObject
    {
        double loanAmount;//Сумма кредита
        int loanPeriods;//периоды(в месяцах)
        double rate; //Процентная годовая ставка

//==================================Свойства======================================
        public int MonthLoan { set; get; }


        public DateTime BeginLoan { set; get; }

        public double LoanAmount { set; get; }

        public int LoanPeriods { set; get; }

        public double Rate { set; get; }

        public double EveryPeriodPaymentAmount { get; set; }
        public double BodyPaymentAmount { get; set; }
        public double PercentPaymentAmount { get; set; }

//===============================================================================

        public List<LoanDisplay> loanDisplay = new List<LoanDisplay>();

//==================================Валидация======================================
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.LoanAmount <= 0)
            {
                errors.Add(new ValidationResult("Сумма займа должна быть больше нуля"));
            }
            if (this.LoanPeriods <= 0)
            {
                errors.Add(new ValidationResult("Количество периодов должно быть больше нуля"));
            }
            if (this.Rate <= 0)
            {
                errors.Add(new ValidationResult("Процентная ставка дожна быть больше нуля"));
            }
            if (this.BeginLoan == default(DateTime))
            {
                errors.Add(new ValidationResult("Введите дату"));
            }

            return errors;
        }
//===============================================================================
//==================================Конструкторы======================================
        public Loan(double lA, int lP, double r, DateTime bL, int MDbool)
        {
            MonthLoan = MDbool;
            bool isMonth = Convert.ToBoolean(MDbool);
            Rate = r;
            if (isMonth == true)//Месяц
            {

                LoanAmount = lA;
                LoanPeriods = lP;
                BeginLoan = bL;


                double koefAnnuity = KoefAnnuity(Rate/12, lP);
                double everyMonthPayment = EveryPeriodPayment(koefAnnuity, LoanAmount);
                double balanceOwed = LoanAmount;

                for (int i = 0; i < lP; i++)
                {

                    double percentPayment = ProcentLoanPayment(balanceOwed, Rate/12);
                    double bodyPayment = everyMonthPayment - percentPayment;
                    loanDisplay.Add(new LoanDisplay(i + 1, BeginLoan.AddMonths(i), everyMonthPayment, bodyPayment, percentPayment, balanceOwed));
                    balanceOwed -= loanDisplay[i].BodyPayment;
                    EveryPeriodPaymentAmount += loanDisplay[i].EveryPeriodPayment;
                    BodyPaymentAmount += loanDisplay[i].BodyPayment;
                    PercentPaymentAmount += loanDisplay[i].PercentPayment;
                }
            }
            else
            {

                LoanAmount = lA;
                LoanPeriods = lP;
                BeginLoan = bL;


                double koefAnnuity = KoefAnnuity(Rate/30, lP);
                double everyMonthPayment = EveryPeriodPayment(koefAnnuity, LoanAmount);
                double balanceOwed = LoanAmount;

                for (int i = 0; i < lP; i++)
                {

                    double percentPayment = ProcentLoanPayment(balanceOwed, Rate/30);
                    double bodyPayment = everyMonthPayment - percentPayment;
                    loanDisplay.Add(new LoanDisplay(i + 1, BeginLoan.AddDays(i), everyMonthPayment, bodyPayment, percentPayment, balanceOwed));
                    balanceOwed -= loanDisplay[i].BodyPayment;
                    EveryPeriodPaymentAmount += loanDisplay[i].EveryPeriodPayment;
                    BodyPaymentAmount += loanDisplay[i].BodyPayment;
                    PercentPaymentAmount += loanDisplay[i].PercentPayment;
                }
            }
        }
        public Loan(Loan l) : this(l.LoanAmount, l.loanPeriods, l.Rate, l.BeginLoan, l.MonthLoan)
        {}

        public Loan() { }
    }

    public class LoanDisplay
    {
        public int Id { get; set; }
        public DateTime PeriodDatePayment { get; set; } 
        public string PeriodDatePaymentString { get; set; } = null;
        public double EveryPeriodPayment { get; set; }
        public double BodyPayment { get; set; }
        public double PercentPayment { get; set; }
        public double BalanceOwed { get; set; }

        public LoanDisplay(int id, DateTime mDP, double eMP, double bP, double pP, double bO)
        {
            Id = id;
            PeriodDatePayment = mDP;
            PeriodDatePaymentString = PeriodDatePayment.ToShortDateString();
            EveryPeriodPayment = eMP;
            BodyPayment = bP;
            PercentPayment = pP;
            BalanceOwed = bO;
        }
        public LoanDisplay(LoanDisplay ld)
        {
            this.Id = ld.Id;
            this.PeriodDatePayment = ld.PeriodDatePayment;
            this.PeriodDatePaymentString = ld.PeriodDatePaymentString;
            this.EveryPeriodPayment = ld.EveryPeriodPayment;
            this.BodyPayment = ld.BodyPayment;
            this.PercentPayment = ld.PercentPayment;
            this.BalanceOwed = ld.BalanceOwed;
        }
        public LoanDisplay() { }
    }

}
