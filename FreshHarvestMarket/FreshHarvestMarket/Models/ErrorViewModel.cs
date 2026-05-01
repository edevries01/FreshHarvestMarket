/*
 * ErrorViewModel.cs
 * FreshHarvestMarket
 *
 * This class represents data used for displaying application error information.
 *
 * It is primarily used by the error view to show debugging or request tracking details.
 *
 * It includes:
 * - A RequestId used to identify and trace specific requests
 * - A helper property that determines whether the RequestId should be displayed
 */

namespace FreshHarvestMarket.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
