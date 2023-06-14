using System.Security.Cryptography;

namespace HE.InvestmentLoans.WWW.Models;

/// <summary>
/// Nonces are required for each javascript snippet to support Content Security Policy.
///
/// Usage:
/// 1) Create a separate string property and generate a NewNonce() for them in this class for each script element.
///
/// 2) In each razor template page with a <script> element, add at the top:
/// @inject NonceModel
/// In the script elements add a nonce attribute with the value of the property you just created in this class e.g.:
/// <script nonce="@NonceModel.GaNonce"> js code here </script>
///
/// 3) Add the nonce to the response header through the HeaderSecurityMiddleware. This needs to be added to the
/// script-src
/// section and prefixed with a "nonce-" (see GaNonce usage for a sample)
/// </summary>
public class NonceModel
{
    private readonly RandomNumberGenerator rand;

    public NonceModel()
    {
        rand = RandomNumberGenerator.Create();
        GaNonce = NewNonce();
    }

    public string GaNonce { get; }

    private string NewNonce()
    {
        var byteArray = new byte[16];
        // 16 bytes is 128 bits - according to mdn this is the minimum recommended size.
        rand.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray);
    }
}