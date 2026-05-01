/*
 * Favorite.cs
 * FreshHarvestMarket
 *
 * This class represents a favorited produce item in the system.
 *
 * It is used to track products that users mark as favorites for quick access.
 *
 * It includes:
 * - A primary key for the Favorite record
 * - A foreign key linking to a Produce item
 * - A navigation property to the associated Produce
 *
 * ******** FIX WHEN UPDATED:****** User-based favorites are not implemented yet. Currently, favorites are
 * treated as universal (not tied to a specific user). Future updates may
 * include linking favorites to user accounts using Identity.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshHarvestMarket.Models
{
    /// <summary>
    /// Represents a favorited product
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// Primary key for Favorite table
        /// </summary>
        [Key]
        public int FavoriteId { get; set; }

        /// <summary>
        /// The ID of the favorited produce item, a foreign key
        /// </summary>
        public int ProduceId { get; set; }

        /// <summary>
        /// The favorited produce item
        /// </summary>
        [Required]
        [ForeignKey("ProduceId")]
        public Produce Produce { get; set; } = null!;

        

        /*We won't add these for now. Until we know how to use Identity. For now, favorites are "universal"
        Later, when we have auth, favorites can be specific to a user*/

        //public User User {get; set;}

        //public string UserId {get; set;}

        //In the future, might make ProduceId + UserId the composite key instead, could add to context class
    }
}
