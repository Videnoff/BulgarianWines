// ReSharper disable VirtualMemberCallInConstructor

namespace BulgarianWines.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BulgarianWines.Data.Common.Models;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.UserImages = new HashSet<UserImage>();
            this.FavoriteProducts = new HashSet<FavoriteProduct>();
            this.Reviews = new HashSet<Review>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] ProfilePicture { get; set; }

        public int UsernameChangeLimit { get; set; } = 10;

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<UserImage> UserImages { get; set; }

        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        //[Required]
        [ForeignKey(nameof(ShoppingCart))]
        public string ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
