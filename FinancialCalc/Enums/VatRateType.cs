using FinancialCalc.Attributes;

namespace FinancialCalc.Enums
{
    public enum VatRateType
    {
        [Percentage(0.05)]
        VatA,
        [Percentage(0.08)]
        VatB,
        [Percentage(0.23)]
        VatC
    }
}
