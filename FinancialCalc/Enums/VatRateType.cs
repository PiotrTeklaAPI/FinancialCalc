using FinancialCalc.Attributes;

namespace FinancialCalc.Enums
{
    public enum VatRateType
    {
        [Percentage(0.05)]
        [Description("5%")]
        VatA,
        [Percentage(0.08)]
        [Description("8%")]
        VatB,
        [Percentage(0.23)]
        [Description("23%")]
        VatC
    }
}
