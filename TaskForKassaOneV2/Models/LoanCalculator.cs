

namespace TaskForKassaOneV2.Models
{
    public static class LoanCalculator
    {

        static public List<DateTime> DatesOfPayments(DateTime firstPayment, int periods)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            dateTimes.Add(firstPayment);
            for (int i = 0; i < periods; i++)
            {
                firstPayment.AddMonths(1);
                dateTimes.Add(firstPayment);
            }

            return dateTimes;
        }

        static public double KoefAnnuity(double Rate, double countsOfPeriods)
        {
            double n = 0; //numenator
            double d = 0; //denumenator
            double monthRate = Rate/100;
            n = monthRate * Math.Pow(1 + monthRate, countsOfPeriods);
            d = Math.Pow(1 + monthRate, countsOfPeriods) - 1;

            return n / d;
        }

        static public double EveryPeriodPayment(double koefAnnuity, double loanAmount)
        {
            return (koefAnnuity * loanAmount);
        }

        static public double ProcentLoanPayment(double extremeLoan, double Rate)
        {
            return (extremeLoan * (Rate/100));
        }

        static public double Amortization(double monthPayment, double procentLoan)
        {
            return monthPayment - procentLoan;
        }
    }
}
