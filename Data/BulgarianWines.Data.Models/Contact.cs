namespace BulgarianWines.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using BulgarianWines.Data.Common.Models;

    public class Contact : BaseDeletableModel<string>
    {
        public Contact()
        {
            this.Id = Guid.NewGuid().ToString();
            this.NewsletterSubscriptions = new HashSet<NewsletterSubscription>();
        }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisallowNull]
        public string Email { get; set; }

        public virtual ICollection<NewsletterSubscription> NewsletterSubscriptions { get; set; }
    }
}
