using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DealerOn.Core
{
    public class SalesTaxes
    {
        private Dictionary<Items, double> _Items10Taxes;
        private Dictionary<Items, double> _Items5Taxes;
        private Dictionary<Items, double> _Items0Taxes;
        private Dictionary<Items, double> _Items15Taxes;

        /// <summary>
        /// print the ticket
        /// </summary>
        /// <param name="items">all items that the user wants to buy</param>
        /// <param name="Items15Taxes">list of items with taxes</param>
        /// <param name="Items10Taxes">list of items with taxes</param>
        /// <param name="Items5Taxes">list of items with taxes</param>
        /// <param name="Items0Taxes">list of items with taxes</param>
        /// <returns>the format of printing</returns>
        public string PrintTicket(List<Items> items, Dictionary<Items, double> Items15Taxes, Dictionary<Items, double> Items10Taxes, Dictionary<Items, double> Items5Taxes, Dictionary<Items, double> Items0Taxes)
        {
            _Items15Taxes = Items15Taxes;
            _Items10Taxes = Items10Taxes;
            _Items5Taxes = Items5Taxes;
            _Items0Taxes = Items0Taxes;
            StringBuilder result = new StringBuilder();
            (double itemTax, double sTaxes, double Total) rCal = (0, 0, 0);
            double salesTaxes = 0, total = 0;
            var distinctItem = items.GroupBy(i => i).Select(dItem => dItem.First()).ToList();
            foreach (var dItem in distinctItem)
            {
                var item2Print = items.Where(i => i == dItem).ToList();
                rCal = DoCaculation(item2Print, dItem, salesTaxes, total);
                salesTaxes = rCal.sTaxes;
                total = rCal.Total;
                if (item2Print.Count > 1)
                    result.Append($"{GetDescriptionString(dItem)}: {rCal.itemTax} ({item2Print.Count} @ {rCal.itemTax / item2Print.Count}) \n");
                else
                    result.Append($"{GetDescriptionString(dItem)}: {rCal.itemTax} \n");
            }
            result.Append($"Sales Taxes: {Math.Round(rCal.sTaxes, 2)} \n");
            result.Append($"Total: {Math.Round(rCal.Total, 2)} \n");
            return result.ToString();
        }

        /// <summary>
        /// generate the calculation with the amount and taxes
        /// </summary>
        /// <param name="items">itmes to buy</param>
        /// <param name="currentItem">current distinct item</param>
        /// <param name="ReSTaxes">amount of taxes</param>
        /// <param name="ReTotal">amount of total</param>
        /// <returns>item plus taxes if it has, sum of taxes and total amount</returns>
        private (double itemTax, double sTaxes, double Total) DoCaculation(List<Items> items, Items currentItem, double ReSTaxes, double ReTotal)
        {
            double resultItemTax = 0;
            var i15Tax = _Items15Taxes.FirstOrDefault(t => t.Key == currentItem);
            var i10Tax = _Items10Taxes.FirstOrDefault(t => t.Key == currentItem);
            var i5Tax = _Items5Taxes.FirstOrDefault(t => t.Key == currentItem);
            var i0Tax = _Items0Taxes.FirstOrDefault(t => t.Key == currentItem);
            //if has values
            if (i15Tax.Key != 0)
            {
                ReSTaxes += (Math.Round(i15Tax.Value * .10, 2) + Math.Round(i15Tax.Value * .05, 1)) * items.Count;
                resultItemTax = (i15Tax.Value + (Math.Round(i15Tax.Value * .10, 2) + Math.Round(i15Tax.Value * .05, 1))) * items.Count;
                resultItemTax = Math.Round(resultItemTax, 2);
            }
            else if (i10Tax.Key != 0)
            {
                ReSTaxes += Math.Round(i10Tax.Value * .1 ,1) * items.Count;
                resultItemTax = (i10Tax.Value + (Math.Round(i10Tax.Value * .1, 1))) * items.Count;
                resultItemTax = Math.Round(resultItemTax, 2);
            }
            else if (i5Tax.Key != 0)
            {
                ReSTaxes += Math.Round(i5Tax.Value * .05 ,1) * items.Count;
                resultItemTax = (i5Tax.Value + (Math.Round(i5Tax.Value * .05, 1))) * items.Count;
                resultItemTax = Math.Round(resultItemTax, 2);
            }
            else if (i0Tax.Key != 0)
            {
                resultItemTax = i0Tax.Value * items.Count;
            }
            ReTotal += resultItemTax;
            return (itemTax: resultItemTax, sTaxes: ReSTaxes, Total: ReTotal);
        }

        /// <summary>
        /// get the description of enum Item
        /// </summary>
        /// <param name="val">enum value</param>
        /// <returns>the description</returns>
        public string GetDescriptionString(Items val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public enum Items
    {
        [Description("Music CD")]
        MusicCD = 1,
        [Description("Bottle of perfume")]
        BottleOfPerfume,
        [Description("Imported box of chocolates")]
        ImportedBoxOfChocolates,
        [Description("Imported box of Perfume")]
        ImportedBottleOfPerfume,
        [Description("Book")]
        Book,
        [Description("Chocalete Bar")]
        ChocaleteBar,
        [Description("Packet of headache pills")]
        PacketOfHeadachePills,
    }


}
