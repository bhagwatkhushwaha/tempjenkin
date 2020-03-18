using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Autumn.FinancialCalc
{
    public static class FinancialCalculations
    {
        public static double PV(double rate, double nPer, double pmt,[Optional] double FV,[Optional] int due)
        {
            try
            {
                double num;

                if (rate == 0)
                    return (-FV - (pmt * nPer));
                if (due != 0)
                    num = 1 + rate;
                else
                    num = 1;
                double x = 1 + rate;
                double num2 = Math.Pow(x, nPer);
                return (-(FV + ((pmt * num) * ((num2 - 1) / rate))) / num2);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static double PMT(double rate, double nPer, double PV, [Optional]double FV, [Optional] int due)
        {
            try
            {
                double num;
                if (nPer == 0.0)
                {
                    throw new Exception("nPer can not be zero.");
                }
                if (rate == 0.0)
                {
                    num = (-FV - PV) / nPer;
                }
                else
                {
                    double num2 = (due == 0) ? 1.0 : (1.0 + rate);
                    double num3 = Math.Pow(rate + 1.0, nPer);
                    num = ((-FV - (PV * num3)) / (num2 * (num3 - 1.0))) * rate;
                }
                return num;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static double FV(double rate, double nPer, double pmt, [Optional]double PV, [Optional] int due)
        {
            try
            {
                double num;
                if (rate == 0.0)
                {
                    num = -PV - (pmt * nPer);
                }
                else
                {
                    double num2 = (due == 0) ? 1.0 : (1.0 + rate);
                    double num3 = Math.Pow(1.0 + rate, nPer);
                    num = (-PV * num3) - (((pmt / rate) * num2) * (num3 - 1.0));
                }
                return num;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
