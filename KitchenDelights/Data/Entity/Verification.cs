using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Verification
{
    public int VerificationId { get; set; }

    public int? UserId { get; set; }

    public string? CardFront { get; set; }

    public string? CardBack { get; set; }

    public string? VerificationFront { get; set; }

    public string? VerificationBack { get; set; }

    public int VerificationStatus { get; set; }

    public DateTime VerificationDate { get; set; }

    public virtual User? User { get; set; }
}
