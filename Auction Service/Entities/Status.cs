using System.ComponentModel.DataAnnotations.Schema;

namespace Auction_Service.Entities
{
    public enum Status
    {
        Live,
        Finished,
        ReserveNotMet
        
    }
}
