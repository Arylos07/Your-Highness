using System;
using System.Collections.Generic;
using System.Text;

namespace Budtender.Orders
{
    /// <summary>
    /// The result of attempting to sell a product to a customer.
    /// Contains whether the sale succeeded, the payment amount, and a breakdown of why deductions were applied.
    /// </summary>
    [Serializable]
    public class SaleResult
    {
        public bool accepted;
        public int payment;
        public string rejectReason;
        public List<string> deductions = new List<string>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!accepted)
            {
                sb.AppendLine($"REJECTED: {rejectReason}");
            }
            else
            {
                sb.AppendLine($"SOLD for ${payment}");
                if (deductions.Count > 0)
                {
                    sb.AppendLine("Deductions:");
                    foreach (var d in deductions)
                        sb.AppendLine($"  - {d}");
                }
            }
            return sb.ToString();
        }
    }
}
