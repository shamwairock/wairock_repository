using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    internal class WilkinsonExtended
    {
        double Floored_mod(double a, double n)
        {
            return a - n * Math.Floor(a / n);
        }

        double Simplicity(int qpos, int Qlen, double j, double lmin, double lmax, double lstep)
        {
            int v = 0;
            if (Floored_mod(lmin, lstep) < 1e-10 && lmin <= 0 && lmax >= 0)
                v = 1;

            return 1.0 - (qpos + 1 - 1.0) / (Qlen - 1.0) + v - j;
        }

        double Simplicity_max(int qpos, int Qlen, uint qlen, double j)
        {
            return 1.0 - (qpos + 1 - 1.0) / (Qlen - 1.0) - j + 1.0;
        }

        double Coverage(double dmin, double dmax, double lmin, double lmax)
        {
            return 1.0 - 0.5 * (Math.Pow((dmax - lmax), 2.0) + Math.Pow((dmin - lmin), 2.0)) / Math.Pow((0.1 * (dmax - dmin)), 2.0);
        }

        double Coverage_max(double dmin, double dmax, double span)
        {
            double trange = dmax - dmin;
            if (span > trange)
            {
                double half = (span - trange) / 2.0;
                return 1.0 - 0.5 * (Math.Pow(half, 2.0) + Math.Pow(half, 2.0)) / Math.Pow((0.1 * trange), 2.0);
            }
            return 1.0;
        }

        double Density(double k, double m, double dmin, double dmax, double lmin, double lmax)
        {
            double r = (k - 1.0) / (lmax - lmin);
            double rt = (m - 1.0) / (Math.Max(lmax, dmax) - Math.Min(lmin, dmin));
            return 2.0 - Math.Max(r / rt, rt / r);
        }

        double Density_max(double k, double m)
        {
            if (k >= m)
                return 2.0 - (k - 1.0) / (m - 1.0);
            return 1.0;
        }

        double Legibility(double lmin, double lmax, double lstep)
        {
            return 1.0;
        }

        double Score(double[] weights, double simplicityVal, double coverageVal, double densityVal, double legibilityVal)
        {
            return weights[0] * simplicityVal + weights[1] * coverageVal + weights[2] * densityVal + weights[3] * legibilityVal;
        }

        public bool Wilk_ext(double dmin, double dmax, double m, int only_inside, double[] Q, double[] w, out double outlmin, out double outlmax, out double outlstep)
        {
            outlmin = double.NaN;
            outlmax = double.NaN;
            outlstep = double.NaN;

            int Qlen = Q.Length;

            //    if (dmin >= dmax) or (m < 1):
            //        return (dmin, dmax, dmax - dmin, 1, 0, 2, 0);

            bool rst = false;

            double best_score = -2.0;
            double j = 1.0;
            while (j < double.MaxValue)
            {
                for (int pos = 0; pos < Qlen; pos++)
                {
                    double q = Q[pos];

                    double sm = 1.0 - (pos) / (Qlen - 1.0) - j + 1.0;

                    if (Score(w, sm, 1.0, 1.0, 1.0) < best_score)
                    {
                        j = double.MaxValue;
                        break;
                    }

                    double k = 2.0;
                    while (k < double.MaxValue)
                    {
                        double dm = Density_max(k, m);

                        if (Score(w, sm, 1, dm, 1) < best_score)
                            break;

                        double delta = (dmax - dmin) / (k + 1.0) / j / q;
                        if (delta < 0.00000000000001)
                        {
                            break;
                        }
                        double z = Math.Ceiling(Math.Log10(delta));

                        while (z < double.MaxValue)
                        {
                            double step = j * q * Math.Pow(10.0, z);
                            double cm = Coverage_max(dmin, dmax, step * (k - 1.0));

                            if (Score(w, sm, cm, dm, 1) < best_score)
                                break;

                            double min_start = Math.Floor(dmax / step) * j - (k - 1.0) * j;
                            double max_start = Math.Ceiling(dmin / step) * j;

                            if (min_start > max_start)
                            {
                                z += 1;
                                break;
                            }

                            for (double start = min_start; start <= max_start; start += 1.0)
                            {
                                double lmin = start * (step / j);
                                double lmax = lmin + step * (k - 1.0);
                                double lstep = step;

                                double s = Simplicity(pos, Qlen, j, lmin, lmax, lstep);
                                double c = Coverage(dmin, dmax, lmin, lmax);
                                double g = Density(k, m, dmin, dmax, lmin, lmax);
                                double l = Legibility(lmin, lmax, lstep);
                                double scr = Score(w, s, c, g, l);

                                if ((scr > best_score) &&
                                        ((only_inside <= 0) || ((lmin >= dmin) && (lmax <= dmax))) &&
                                        ((only_inside >= 0) || ((lmin <= dmin) && (lmax >= dmax))))
                                {
                                    best_score = scr;

                                    outlmin = lmin;
                                    outlmax = lmax;
                                    outlstep = lstep;
                                    rst = true;
                                }
                            }
                            z += 1.0;
                        }//end of z-while-loop
                        k += 1.0;
                    }//end of k-while-loop
                }
                j += 1.0;
            }

            return rst;
        }

        public bool Easy_wilk_ext(double dmin, double dmax, double m, int only_inside, out double outlmin, out double outlmax, out double outlstep)
        {
            double[] Q = new double[] { 1.0, 5.0, 2.0, 2.5, 4.0, 3.0 };
            double[] w = { 0.25, 0.2, 0.5, 0.05 };

            return Wilk_ext(dmin, dmax, m, only_inside, Q, w, out outlmin, out outlmax, out outlstep);
        }
    }
}
